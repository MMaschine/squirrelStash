using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Helpers;
using SquirrelStash.Requests;
using SquirrelStash.Resources;


namespace SquirrelStash.Logic
{
    internal class CategoryService(StashContext context, ILogger<CategoryService> logger) : ICategoryService
    {
        private readonly DbSet<Category> _categoriesSet = context.Set<Category>();

        private readonly DbSet<PropertyDefinition> _propertyDefinitionsSet = context.Set<PropertyDefinition>();
        private readonly DbSet<Item> _itemsSet = context.Set<Item>();

        /// <inheritdoc />
        public async Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync()
        {
            try
            {
                var data = await _categoriesSet.AsQueryable()
                    .Include(x => x.Properties)
                    .Include(x => x.Items)
                        .ThenInclude(x => x.PropertyEntries)
                            .ThenInclude(x => x.Definition)
                    .AsNoTracking().ToListAsync() ?? [];

                return data.AsReadOnly();
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, "Failed to load categories.", logger);
                return Result.Fail(AppText.CannotGetCategories);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Category>> CreateCategoryAsync(EditCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Properties);

            if (string.IsNullOrEmpty(request.Title))
            {
                throw new ArgumentException(nameof(request.Title));
            }

            try
            {
                var categoryToAdd = new Category()
                {
                    Title = request.Title
                };

                categoryToAdd.Properties.AddRange(request.Properties.Select(x => new PropertyDefinition()
                {
                    TypeCode = (int)x.Type,
                    Name = x.Name,
                    AllowedValues = x.AllowedValues
                }));

                await _categoriesSet.AddAsync(categoryToAdd);
                await context.SaveChangesAsync();

                return Result.Ok(categoryToAdd);
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to create category with title {request.Title}.", logger);
                return Result.Fail(AppText.FailedToCreateCategory);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Category>> UpdateCategoryAsync(EditCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Properties);
            ArgumentNullException.ThrowIfNull(request.CategoryId);

            if (string.IsNullOrEmpty(request.Title))
            {
                throw new ArgumentException(nameof(request.Title));
            }

            try
            {
                var category = await _categoriesSet
                    .Include(x=> x.Properties)
                    .Include(x=> x.Items)
                    .ThenInclude(x=>x.PropertyEntries)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId);

                if (category == null)
                {
                    return Result.Fail("Category with given Id not found");
                }

                category.Title = request.Title;

                //Update existing properties 
                foreach (var toUpdate in request.Properties.Where(x => !x.IsNew).ToArray())
                {
                    var property = category.Properties.FirstOrDefault(x => x.Id == toUpdate.Id);

                    if (property == null)
                    {
                        continue;
                    }

                    property.Name = toUpdate.Name;
                    property.AllowedValues = property.TypeCode == (int)PropertyTypes.AllowedValues
                        ? toUpdate.AllowedValues
                        : null;
                }

                //Properties to add: 
                category.Properties.AddRange(request.Properties.Where(x => x.IsNew).Select(x => new PropertyDefinition()
                {
                    TypeCode = (int)x.Type,
                    Name = x.Name,
                    AllowedValues = x.AllowedValues
                }));

                if (request.PropertiesToRemove is { Length: > 0 })
                {
                    //Clean category
                    category.Properties.RemoveAll(x => request.PropertiesToRemove.Contains(x.Id));

                    _propertyDefinitionsSet.RemoveRange(
                        _propertyDefinitionsSet.Where(x => request.PropertiesToRemove.Contains(x.Id)));
                }

                await context.SaveChangesAsync();

                return Result.Ok(category);
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to update category {request.CategoryId}/{request.Title}.", logger);
                return Result.Fail(AppText.FailedToCreateCategory);
            }
        }

        /// <inheritdoc />
        public async Task<Result> RemoveCategoryAsync(int id)
        {
            try
            {
                var category = await _categoriesSet.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return Result.Fail(AppText.CategoryNotFound);
                }

                _categoriesSet.Remove(category);
                await context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to delete category {id}.", logger);
                return Result.Fail(AppText.FailedToDeleteCategory);
            }
        }
    }
}

using FluentResults;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Models;
using SquirrelStash.Resources;

namespace SquirrelStash.Logic
{
    internal class OverviewService(StashContext context) : IOverviewService
    {
        private readonly DbSet<Item> _itemSet = context.Items;
        private readonly DbSet<Category> _categorySet = context.Categories;

        public async Task<Result<Overview>> GetOverviewAsync()
        {
            try
            {
                var overviewResult = new Overview
                {
                    TotalCategoriesCount = await _categorySet.CountAsync(),
                    TotalItemsCount = await _itemSet.CountAsync()
                };

                var itemsBelowThreshold = await _itemSet.AsNoTracking()
                    .Where(x => x.Quantity < x.WarningThreshold)
                    .Select(x => new OverviewItem
                    {
                        Category = x.Category.Title,
                        Quantity = x.Quantity,
                        IsCritical = x.Quantity <= x.CriticalThreshold,
                        PropertiesValues = x.PropertyEntries.Select(x => x.Value).ToArray()
                    }).ToListAsync();

                overviewResult.ItemsToHighlight.AddRange(itemsBelowThreshold);

                return Result.Ok(overviewResult);
            }
            catch (Exception ex)
            {
                //TODO: add log
                return Result.Fail(AppText.FailedToBuildOverview);
            }
        }
    }
}

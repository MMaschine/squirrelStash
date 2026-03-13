using FluentResults;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Abstractions;

public interface ICategoryService
{
    Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync();

    Task<Result> CreateCategoryAsync(CreateCategoryRequest request);
}
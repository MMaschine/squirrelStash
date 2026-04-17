using FluentResults;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Abstraction of category data operations service.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Gets all categories with their related data.
    /// </summary>
    /// <returns>A result containing the available categories.</returns>
    Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync();

    /// <summary>
    /// Creates a new category from the provided request.
    /// </summary>
    /// <param name="request">The category creation request.</param>
    /// <returns>A result containing the created category.</returns>
    Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request);
}

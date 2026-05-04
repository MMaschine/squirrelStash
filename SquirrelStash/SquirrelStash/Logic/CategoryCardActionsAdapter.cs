using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Logic;

internal sealed class CategoryCardActionsAdapter(Func<Category, Task> editCategoryAsync)
    : ICategoryCardActions
{
    /// <inheritdoc />
    public Task EditCategoryAsync(Category category)
    {
        return editCategoryAsync(category);
    }
}

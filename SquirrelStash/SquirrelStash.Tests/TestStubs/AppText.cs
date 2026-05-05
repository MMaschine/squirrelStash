namespace SquirrelStash.Resources;

internal static class AppText
{
    public const string CategoryTitleRequired = "Title must be set for category";
    public const string CategoryPropertyRequired = "Set at least one property for the category";
    public const string FillPropertyName = "All properties must have names";
    public const string AllowedValuesInvalidFormat = "Allowed values for '{0}' must be comma-separated and unique.";
    public const string ItemValueRequiredFormat = "Set a value for property '{0}'.";
    public const string FailedToGetImage = "Failed to get the image";
    public const string CreateCategoryPageTitle = "Create Category";
    public const string EditCategoryPageTitle = "Edit Category";
    public const string CreateItemPageTitle = "Create Item";
    public const string EditItemPageTitle = "Edit Item";
    public const string DefaultQuantity = "Default quantity";
    public const string CurrentQuantity = "Current quantity";
    public const string FailedToAddItem = "Failed to add item!";
    public const string FailedToUpdateItem = "Failed to update item";
    public const string FailedToDeleteItem = "Failed to delete item";
    public const string CannotGetCategories = "Can't get categories";
    public const string ItemNotFound = "Item not found";
    public const string WrongIncrement = "Wrong increment";

    public static string FormatCategoryExists(string categoryTitle) =>
        $"Category {categoryTitle} already exists";

    public static string FormatDeleteCategoryConfirmation(string categoryTitle) =>
        $"Do you really want to delete category {categoryTitle}?";
}

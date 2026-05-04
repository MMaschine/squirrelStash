using FluentResults;

namespace SquirrelStash.Resources;

public static class AppText
{
    public const string AlertOk = "OK";
    public const string AlertSend = "Send";
    public const string AlertCancel = "Cancel";
    public const string AlertErrorTitle = "Error";
    public const string AlertInformationTitle = "Information";
    public const string AlertWarningTitle = "Warning";
    public const string AlertConfirmationTitle = "Confirmation";
    public const string SomethingWentWrongShareLogs = "Something went wrong.\nWould you like to send logs?";
    public const string SendLogFileTitle = "Send log file";

    public const string AppTitle = "Squirrel Stash";

    public const string CreateCategoryPageTitle = "Create Category";
    public const string EditCategoryPageTitle = "Edit Category";
    public const string EnterCategoryTitlePlaceholder = "Enter category title";
    public const string PropertiesLabel = "Properties";
    public const string PropertyNamePlaceholder = "Property name";
    public const string SelectPropertyTypeTitle = "Select property type";
    public const string AllowedValuesPlaceholder = "Allowed values (comma-separated)";

    public const string CreateItemPageTitle = "Create Item";
    public const string EditItemPageTitle = "Edit Item";
    public const string TapToAddImage = "Tap to add image";
    public const string ThresholdsLabel = "Thresholds";
    public const string WarningThresholdLabel = "Warning threshold";
    public const string CriticalThresholdLabel = "Critical threshold";
    public const string EnterValuePlaceholder = "Enter value";
    public const string SelectValueTitle = "Select value";

    public const string SelectOrderTitle = "Order by";
    public const string SearchCategoryPlaceholder = "Search category";
    public const string LoadingCategories = "Loading categories...";
    public const string LoadingOverview = "Loading overview...";

    public const string OverviewTitle = "Overview";
    public const string WarningsReachedLabel = "Warnings reached";
    public const string CriticalReachedLabel = "Critical reached";
    public const string EverythingLooksCalm = "Everything looks calm";

    public const string FailedToAddNewCategory = "Failed to add new category!";
    public const string FailedToCreateCategory = "Can't add category because of exception";
    public const string FailedToUploadCategories = "Failed to upload categories";
    public const string FailedToDeleteCategory = "Failed to delete category";
    public const string FailedToAddItem = "Failed to add item!";
    public const string FailedToCopyItem = "Failed to copy item";
    public const string FailedToDeleteItem = "Failed to delete item";
    public const string FailedToBuildOverview = "Failed to build overview";
    public const string FailedToGetImage = "Failed to get the image";
    public const string FailedToGetImageResult = "Failed to get the image";
    public const string CameraAccessInfo = "Provide access to the camera for the app";
    public const string QuantityChangeError = "Error in quantity changing! Contact developer";

    public const string CategoryTitleRequired = "Title must be set for category";
    public const string CategoryPropertyRequired = "Set at least one property for the category";
    public const string ItemValueRequiredFormat = "Set a value for property '{0}'.";
    public const string AllowedValuesInvalidFormat = "Allowed values for '{0}' must be comma-separated and unique.";
    public const string FillPropertyName = "All properties must have names";

    public const string CannotGetCategories = "Can't get categories";
    public const string CategoryNotFound = "Category not found";
    public const string FailedToUpdateItem = "Failed to update item";
    public const string WrongIncrement = "Wrong increment";
    public const string ItemNotFound = "Item not found";
    public const string CannotGetImage = "Can't get image";

    public const string DefaultQuantity = "Default quantity";
    public const string CurrentQuantity = "Current quantity";
    public const string CategoryDeletedMessage = "Category deleted";
    public const string ItemDeletedMessage = "Item deleted";
    public const string DeleteCategoryConfirmationFormat = "Do you really want to delete category {0}?";
    public const string DeleteItemConfirmationFormat = "Do you really want to delete item {0}?";

    public const char ItemNameSeparator = '/';

    public static string FormatCategoryAdded(string categoryTitle) =>
        $"Category {categoryTitle} added";

    public static string FormatCategoryExists(string categoryTitle) =>
        $"Category {categoryTitle} already exists";

    public static string FormatDeleteCategoryConfirmation(string categoryTitle) =>
        string.Format(DeleteCategoryConfirmationFormat, categoryTitle);

    public static string FormatItemAdded(string categoryTitle) =>
        $"New item added to the category {categoryTitle}";

    public static string FormatItemUpdate(string categoryTitle, string itemName) =>
        $"Item {itemName} updated in the category {categoryTitle}";

    public static string FormatDeleteItemConfirmation(string itemName) =>
        string.Format(DeleteItemConfirmationFormat, itemName);

    public static string FormatItemsHeader(int itemsCount) =>
        $"Items ({itemsCount})";

    public static string FormatVersion(string version) =>
        version.Equals("0.1-alpha.1", StringComparison.OrdinalIgnoreCase)
            ? "Version 0.1 Alpha 1"
            : $"Version {version}";
}

namespace SquirrelStash.Resources;

public static class AppText
{
    public const string AlertOk = "OK";
    public const string AlertErrorTitle = "Error";
    public const string AlertInformationTitle = "Information";
    public const string AlertWarningTitle = "Warning";

    public const string AppTitle = "Squirrel Stash";

    public const string CreateCategoryPageTitle = "Create Category";
    public const string CreateCategoryHeader = "Create Category";
    public const string EnterCategoryTitlePlaceholder = "Enter category title";
    public const string PropertiesLabel = "Properties";
    public const string PropertyNamePlaceholder = "Property name";
    public const string SelectPropertyTypeTitle = "Select property type";
    public const string AllowedValuesPlaceholder = "Allowed values (comma-separated)";

    public const string CreateItemPageTitle = "Create Item";
    public const string CreateItemHeader = "Create Item";
    public const string TapToAddImage = "Tap to add image";
    public const string ThresholdsLabel = "Thresholds";
    public const string WarningThresholdLabel = "Warning threshold";
    public const string CriticalThresholdLabel = "Critical threshold";
    public const string EnterValuePlaceholder = "Enter value";
    public const string SelectValueTitle = "Select value";

    public const string SelectFilterTitle = "Select filter";
    public const string SearchCategoryPlaceholder = "Search category";
    public const string LoadingCategories = "Loading categories...";
    public const string LoadingOverview = "Loading overview...";

    public const string OverviewTitle = "Overview";
    public const string CategoriesLabel = "Categories";
    public const string ItemsLabel = "Items";
    public const string WarningsReachedLabel = "Warnings reached";
    public const string CriticalReachedLabel = "Critical reached";
    public const string EverythingLooksCalm = "Everything looks calm";

    public const string FailedToAddNewCategory = "Failed to add new category!";
    public const string FailedToCreateCategory = "Can't add category because of exception";
    public const string FailedToUploadCategories = "Failed to upload categories";
    public const string FailedToAddItem = "Failed to add item!";
    public const string FailedToBuildOverview = "Failed to build overview";
    public const string FailedToGetImage = "Failed to get the image";
    public const string FailedToGetImageResult = "Failed to get the image";
    public const string CameraAccessInfo = "Provide access to the camera for the app";
    public const string QuantityChangeError = "Error in quantity changing! Contact developer";

    public const string CategoryTitleRequired = "Title must be set for category";
    public const string CategoryPropertyRequired = "Set at least one property for the category";
    public const string ItemValueRequiredFormat = "Set a value for property '{0}'.";
    public const string AllowedValuesInvalidFormat = "Allowed values for '{0}' must be comma-separated.";

    public const string CannotGetCategories = "Can't get categories";
    public const string FailedToCreateItem = "Failed to create item";
    public const string FailedToUpdateItem = "Failed to update item";
    public const string WrongIncrement = "Wrong increment";
    public const string ItemNotFound = "Item not found";
    public const string CannotGetImage = "Can't get image";

    public static string FormatCategoryAdded(string categoryTitle) =>
        $"Category {categoryTitle} added";

    public static string FormatItemAdded(string categoryTitle) =>
        $"New item added to the category {categoryTitle}";

    public static string FormatItemsHeader(int itemsCount) =>
        $"Items ({itemsCount})";

    public static string FormatVersion(string version) =>
        version.Equals("0.1-alpha.1", StringComparison.OrdinalIgnoreCase)
            ? "Version 0.1 Alpha 1"
            : $"Version {version}";
}

using SquirrelStash.Requests;
using SquirrelStash.Enums;

namespace SquirrelStash.Models;

public sealed class EditCategoryDialogResult
{
    private EditCategoryDialogResult(ChangeCategoryAction action, EditCategoryRequest? data, string? errorMessage)
    {
        Action = action;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public ChangeCategoryAction Action { get; }

    public bool IsCanceled => Action == ChangeCategoryAction.Canceled;

    public bool IsFailed => Action == ChangeCategoryAction.ChangeFailed;

    public bool IsChangesApplied => Action == ChangeCategoryAction.ChangesApplied;

    public bool IsDeleted => Action == ChangeCategoryAction.Deleted;

    public EditCategoryRequest? Data { get; }

    public string? ErrorMessage { get; }

    public static EditCategoryDialogResult GetCanceled()
        => new(ChangeCategoryAction.Canceled, null, null);

    public static EditCategoryDialogResult GetChangesApplied(EditCategoryRequest data)
        => new(ChangeCategoryAction.ChangesApplied, data, null);

    public static EditCategoryDialogResult GetFailed(string message)
        => new(ChangeCategoryAction.ChangeFailed, null, message);

    public static EditCategoryDialogResult GetDeleted()
        => new(ChangeCategoryAction.Deleted, null, null);
}

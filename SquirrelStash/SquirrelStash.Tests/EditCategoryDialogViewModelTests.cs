using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Models;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Tests;

public sealed class EditCategoryDialogViewModelTests
{
    [Test]
    public async Task SaveCommand_WhenEditingCategory_IncludesExistingPropertyChanges()
    {
        var category = new Category
        {
            Id = 7,
            Title = "Clothes",
            Properties =
            [
                new PropertyDefinition
                {
                    Id = 11,
                    Name = "Name",
                    TypeCode = (int)PropertyTypes.Basic
                },
                new PropertyDefinition
                {
                    Id = 12,
                    Name = "Size",
                    TypeCode = (int)PropertyTypes.AllowedValues,
                    AllowedValues = "S,M"
                }
            ]
        };

        var viewModel = new EditCategoryDialogViewModel(["Clothes"], category);
        EditCategoryDialogResult? result = null;
        viewModel.RequestCompleted += request => result = request;

        viewModel.Properties[0].Name = "Display name";
        viewModel.Properties[1].AllowedValues = "S, M, L";

        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsChangesApplied, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Properties, Has.Length.EqualTo(2));

        var renamedProperty = result.Data.Properties.Single(x => x.Id == 11);
        Assert.That(renamedProperty.Name, Is.EqualTo("Display name"));
        Assert.That(renamedProperty.Type, Is.EqualTo(PropertyTypes.Basic));

        var allowedValuesProperty = result.Data.Properties.Single(x => x.Id == 12);
        Assert.That(allowedValuesProperty.AllowedValues, Is.EqualTo("S,M,L"));
    }
}

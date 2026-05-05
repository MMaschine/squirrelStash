using FluentResults;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Tests;

public sealed class EditItemDialogViewModelTests
{
    [Test]
    public async Task SaveCommand_WhenEditingItem_PreservesThresholdsAndNote()
    {
        var property = new PropertyDefinition
        {
            Id = 20,
            Name = "Name",
            TypeCode = (int)PropertyTypes.Basic
        };

        var category = new Category
        {
            Id = 3,
            Title = "Supplies",
            Properties = [property]
        };

        var item = new Item
        {
            Id = 42,
            CategoryId = category.Id,
            WarningThreshold = 9,
            CriticalThreshold = 4,
            Quantity = 12,
            Note = "Keep sealed",
            PropertyEntries =
            [
                new PropertyEntry
                {
                    PropertyDefinitionId = property.Id,
                    Value = "Tape"
                }
            ]
        };

        var viewModel = new EditItemDialogViewModel(category, item, new StubImageService());
        DialogResult<EditItemRequest>? result = null;
        viewModel.RequestCompleted += request => result = request;

        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsSuccess, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Data!.WarningThreshold, Is.EqualTo(9));
            Assert.That(result.Data.CriticalThreshold, Is.EqualTo(4));
            Assert.That(result.Data.DefaultQuantity, Is.EqualTo(12));
            Assert.That(result.Data.Note, Is.EqualTo("Keep sealed"));
        });
    }

    private sealed class StubImageService : IImageService
    {
        public Task<Result<string>> PickAndStoreImageAsync(
            ItemImageSource source,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Ok("test.png"));
        }
    }
}

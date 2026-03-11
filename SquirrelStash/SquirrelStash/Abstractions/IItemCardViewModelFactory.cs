using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Abstractions;

public interface IItemCardViewModelFactory
{
    ItemCardViewModel Create(Item item);
}
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories;

public interface IItemCardViewModelFactory
{
    ItemCardViewModel GetViewModel(Item item);
}

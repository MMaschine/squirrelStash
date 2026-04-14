using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories;

public interface ICategoryCardViewModelFactory
{
    CategoryCardViewModel GetViewModel(Category category);
}

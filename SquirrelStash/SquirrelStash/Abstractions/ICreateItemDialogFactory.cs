using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Views;

namespace SquirrelStash.Logic.Factories;

public interface ICreateItemDialogFactory
{
    CreateItemDialog CreateDialog(Category category);
}

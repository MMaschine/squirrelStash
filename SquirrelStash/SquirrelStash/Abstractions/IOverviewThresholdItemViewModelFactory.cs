using SquirrelStash.Models;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories;

public interface IOverviewThresholdItemViewModelFactory
{
    OverviewThresholdItemViewModel GetViewModel(OverviewItem item);
}

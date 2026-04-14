using Microsoft.Extensions.Logging;
using SquirrelStash.Models;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories;

internal class OverviewThresholdItemViewModelFactory(ILogger<OverviewThresholdItemViewModel> logger)
    : IOverviewThresholdItemViewModelFactory
{
    public OverviewThresholdItemViewModel GetViewModel(OverviewItem item)
    {
        return new OverviewThresholdItemViewModel(item, logger);
    }
}

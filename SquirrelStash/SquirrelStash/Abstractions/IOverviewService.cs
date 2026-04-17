using FluentResults;
using SquirrelStash.Models;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Abstraction of the service to provide overview report data for the stash.
/// </summary>
public interface IOverviewService
{
    /// <summary>
    /// Builds the overview report.
    /// </summary>
    /// <returns>A result containing overview data.</returns>
    Task<Result<Overview>> GetOverviewAsync();
}

using FluentResults;
using SquirrelStash.Models;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Abstraction of service to build general report about items in store 
/// </summary>
public interface IOverviewService
{
    /// <summary>
    /// Provide data for overview report
    /// </summary>
    Task<Result<Overview>> GetOverviewAsync();
}
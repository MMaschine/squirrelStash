using FluentResults;
using SquirrelStash.Enums;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Provides image selection and image storage operations.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Picks an image from the requested source, stores it locally, and returns the saved path.
    /// </summary>
    /// <param name="source">The source used to select the image.</param>
    /// <param name="cancellationToken">A token used to cancel image storage work.</param>
    /// <returns>A result containing the saved image path.</returns>
    Task<Result<string>> PickAndStoreImageAsync(
        ItemImageSource source,
        CancellationToken cancellationToken = default);
}

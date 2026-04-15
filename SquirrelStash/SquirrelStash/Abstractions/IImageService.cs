using FluentResults;
using SquirrelStash.Enums;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Abstraction of the service to pick the images 
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Get image from requested source (gallery, camera), save in folder and return the path  
    /// </summary>
    /// <param name="source">The source of the image</param>
    /// <returns>The path where image was saved</returns>
    Task<Result<string>> PickAndStoreImageAsync(
        ItemImageSource source,
        CancellationToken cancellationToken = default);
}

using FluentResults;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.Enums;
using SquirrelStash.Helpers;
using SquirrelStash.Resources;

namespace SquirrelStash.Logic
{
    internal class ImageService(ILogger<ImageService> logger) : IImageService
    {
        public const string ItemImagePlaceholder = "grey_tshirt.png";

        public async Task<Result<string>> PickAndStoreImageAsync(
            ItemImageSource source,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fileResult = source switch
                {
                    ItemImageSource.Camera => await CapturePhotoAsync(),
                    ItemImageSource.Gallery => await MediaPicker.Default.PickPhotoAsync(),
                    _ => null
                };

                if (fileResult is null)
                {
                    logger.LogWarning("Image selection failed for source {ImageSource}: no file was returned.", source);
                    return Result.Fail(AppText.CannotGetImage);
                }

                var imagesFolder = Path.Combine(FileSystem.AppDataDirectory, "items-images");
                Directory.CreateDirectory(imagesFolder);

                var extension = Path.GetExtension(fileResult.FileName);
                if (string.IsNullOrWhiteSpace(extension))
                {
                    extension = ".jpg";
                }

                var fileName = $"{Guid.NewGuid():N}{extension}";
                var destinationPath = Path.Combine(imagesFolder, fileName);

                await using var sourceStream = await fileResult.OpenReadAsync();
                await using var destinationStream = File.Create(destinationPath);

                await sourceStream.CopyToAsync(destinationStream, cancellationToken);

                return destinationPath;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to pick and store image from source {ImageSource}.", source);
                return Result.Fail(AppText.FailedToGetImageResult);
            }
        }

        private static async Task<FileResult?> CapturePhotoAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted)
            {
                await MessageHelper.ShowInfoAsync(AppText.CameraAccessInfo);
                return null;
            }

            return await MediaPicker.Default.CapturePhotoAsync();
        }
    }
}

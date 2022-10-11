using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Stellarity.Avalonia.Extensions;

namespace Stellarity.Avalonia.Models;

public static class ImagePlaceholder
{
    private const string PlaceholderUri = "avares://Stellarity.Avalonia/Assets/Images/placeholder.png";

    public static Bitmap GetBitmap()
    {
        var placeholderStream = GetAsset();
        return new Bitmap(placeholderStream);
    }

    public static byte[] GetBytes()
    {
        var placeholderStream = GetAsset();
        var bm = new Bitmap(placeholderStream);
        return bm.FromBitmap()!;
    }

    private static Stream GetAsset()
    {
        var uri = new Uri(PlaceholderUri);
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
        return assets.Open(uri);
    }

    // public static async Task SaveAsync(string path, Account account)
    // {
    //     var data = await File.ReadAllBytesAsync(path);
    //     await using var context = new StellarisContext();
    //     var image = await context.Images.FirstOrDefaultAsync(img => img.Guid == account.AvatarGuid);
    //     if (image is null)
    //     {
    //         var guid = Guid.NewGuid();
    //         var name = $"{account.Email}";
    //         image = new ImageModel(guid, name, data);
    //         context.Images.Add(image);
    //     }
    //     else
    //     {
    //         image.Data = data;
    //         context.Images.Update(image);
    //     }
    //
    //     account.AvatarGuid = image.Guid;
    //     context.Users.Update(account);
    //     await context.SaveChangesAsync();
    //
    //     // await ImageCacheService.SaveToGlobalCache(image);
    // }
    //
    // public static async Task SaveAsync(string path, Game game)
    // {
    //     var data = await File.ReadAllBytesAsync(path);
    //     await using var context = new StellarisContext();
    //     var image = await context.Images.FirstOrDefaultAsync(img => img.Guid == game.CoverGuid);
    //     if (image is null)
    //     {
    //         var guid = Guid.NewGuid();
    //         var name = $"{game.Name}";
    //         image = new ImageModel(guid, name, data);
    //         context.Images.Add(image);
    //     }
    //     else
    //     {
    //         image.Data = data;
    //         context.Images.Update(image);
    //     }
    //
    //     game.CoverGuid = image.Guid;
    //     context.Update(game);
    //     await context.SaveChangesAsync();
    //
    //     // await ImageCacheService.SaveToGlobalCache(image);
    // }
    //
    // public static async Task<Bitmap?> OpenAsync(Account account)
    // {
    //     if (account.AvatarGuid is null) return await OpenDefaultImageAsync();
    //
    //     var url = account.AvatarGuid.Value.ToString();
    //     // var bitmap = await ImageCacheService.LoadFromGlobalCache(url);
    //     // if (bitmap != null) return bitmap;
    //
    //     try
    //     {
    //         var image = ImageModel.Find(account.AvatarGuid)!;
    //         using var memoryStream = new MemoryStream(image.Data);
    //         // bitmap = new Bitmap(memoryStream);
    //         // await ImageCacheService.SaveToGlobalCache(image);
    //         return null;
    //     }
    //     catch (Exception)
    //     {
    //         return null;
    //     }
    // }
    //
    // public static async Task<Bitmap?> OpenAsync(Game game)
    // {
    //     if (game.CoverGuid is null) return await OpenDefaultImageAsync();
    //
    //     var url = game.CoverGuid.Value.ToString();
    //     // var bitmap = await ImageCacheService.LoadFromGlobalCache(url);
    //     // if (bitmap != null) return bitmap;
    //
    //     try
    //     {
    //         var image = ImageModel.Find(game.CoverGuid)!;
    //         using var memoryStream = new MemoryStream(image.Data);
    //         // bitmap = new Bitmap(memoryStream);
    //         // await ImageCacheService.SaveToGlobalCache(image);
    //         // return bitmap;
    //         return null;
    //     }
    //     catch (Exception)
    //     {
    //         return null;
    //     }
    // }
    //
    // public static Task<Bitmap> OpenDefaultImageAsync()
    // {
    //     var uri = new Uri("avares://Stellarity.Desktop/Assets/Images/placeholder.png");
    //     var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
    //     var asset = assets.Open(uri);
    //
    //     return Task.FromResult(new Bitmap(asset));
    // }
}
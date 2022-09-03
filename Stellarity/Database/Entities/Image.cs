using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public partial class Image
{
    public Image()
    {
        Guid = Guid.NewGuid();
        Games = new HashSet<Game>();
        Users = new HashSet<Account>();
    }

    public Image(Guid guid, string name, byte[] data) : this()
    {
        Guid = guid;
        Data = data;
        Name = name;
    }

    public Image(string name) : this()
    {
        Name = name;
    }

    /// <summary>
    /// Содержит уникальный идентификатор изображения. Может использоваться в качестве имени файла для кэша
    /// </summary>
    public Guid Guid { get; set; }

    public byte[] Data { get; set; } = null!;
    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; }
    public virtual ICollection<Account> Users { get; set; }

    public static Image? Find(Guid? guid)
    {
        using var context = new StellarisContext();
        return context.Images.FirstOrDefault(img => img.Guid == guid);
    }

    public static async Task SaveAsync(string path, Account account)
    {
        var data = await File.ReadAllBytesAsync(path);
        await using var context = new StellarisContext();
        var image = await context.Images.FirstOrDefaultAsync(img => img.Guid == account.AvatarGuid);
        if (image is null)
        {
            var guid = Guid.NewGuid();
            var name = $"{account.Email}";
            image = new Image(guid, name, data);
            context.Images.Add(image);
        }
        else
        {
            image.Data = data;
            context.Images.Update(image);
        }

        account.AvatarGuid = image.Guid;
        context.Users.Update(account);
        await context.SaveChangesAsync();

        // await ImageCacheService.SaveToGlobalCache(image);
    }

    public static async Task SaveAsync(string path, Game game)
    {
        var data = await File.ReadAllBytesAsync(path);
        await using var context = new StellarisContext();
        var image = await context.Images.FirstOrDefaultAsync(img => img.Guid == game.CoverGuid);
        if (image is null)
        {
            var guid = Guid.NewGuid();
            var name = $"{game.Name}";
            image = new Image(guid, name, data);
            context.Images.Add(image);
        }
        else
        {
            image.Data = data;
            context.Images.Update(image);
        }

        game.CoverGuid = image.Guid;
        context.Update(game);
        await context.SaveChangesAsync();

        // await ImageCacheService.SaveToGlobalCache(image);
    }

    public static async Task<Bitmap?> OpenAsync(Account account)
    {
        if (account.AvatarGuid is null) return await OpenDefaultImageAsync();

        var url = account.AvatarGuid.Value.ToString();
        // var bitmap = await ImageCacheService.LoadFromGlobalCache(url);
        // if (bitmap != null) return bitmap;

        try
        {
            var image = Find(account.AvatarGuid)!;
            using var memoryStream = new MemoryStream(image.Data);
            // bitmap = new Bitmap(memoryStream);
            // await ImageCacheService.SaveToGlobalCache(image);
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<Bitmap?> OpenAsync(Game game)
    {
        if (game.CoverGuid is null) return await OpenDefaultImageAsync();

        var url = game.CoverGuid.Value.ToString();
        // var bitmap = await ImageCacheService.LoadFromGlobalCache(url);
        // if (bitmap != null) return bitmap;

        try
        {
            var image = Find(game.CoverGuid)!;
            using var memoryStream = new MemoryStream(image.Data);
            // bitmap = new Bitmap(memoryStream);
            // await ImageCacheService.SaveToGlobalCache(image);
            // return bitmap;
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static Task<Bitmap> OpenDefaultImageAsync()
    {
        var uri = new Uri("avares://Stellarity/Assets/placeholder.png");
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
        var asset = assets.Open(uri);

        return Task.FromResult(new Bitmap(asset));
    }

    public static Bitmap OpenDefaultImage()
    {
        var uri = new Uri("avares://Stellarity/Assets/placeholder.png");
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
        var asset = assets.Open(uri);

        return new Bitmap(asset);
    }

    public static Bitmap GetBitmap(string fileName)
    {
        var bm = new Bitmap(fileName);
        return bm;
    }
}
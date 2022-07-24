using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;
using Stellarity.Database;

namespace Stellarity.Models;

public partial class Image
{
    public Image()
    {
        Games = new HashSet<Game>();
        Users = new HashSet<User>();
    }

    public Image(Guid guid, string name, string alter, byte[] data) : this()
    {
        Guid = guid;
        Data = data;
        Name = name;
        Alter = alter;
    }

    /// <summary>
    /// Содержит уникальный идентификатор изображения. Может использоваться в качестве имени файла для кэша
    /// </summary>
    public Guid Guid { get; set; }

    public byte[] Data { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Alter { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; }
    public virtual ICollection<User> Users { get; set; }

    public static Image? Find(Guid? guid)
    {
        using var context = new StellarisContext();
        return context.Images.FirstOrDefault(img => img.Guid == guid);
    }

    public static async Task SaveAsync(string path, User user)
    {
        var data = await File.ReadAllBytesAsync(path);
        await using var context = new StellarisContext();
        var image = await context.Images.FirstOrDefaultAsync(img => img.Guid == user.AvatarGuid);
        if (image is null)
        {
            var guid = Guid.NewGuid();
            var name = $"{user.Email}";
            var alter = $"{user.Email} avatar";
            image = new Image(guid, name, alter, data);
            context.Images.Add(image);
        }
        else
        {
            image.Data = data;
            context.Images.Update(image);
        }

        user.AvatarGuid = image.Guid;
        context.Users.Update(user);
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
            var alter = $"{game.Name} cover";
            image = new Image(guid, name, alter, data);
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

    public static async Task<Bitmap?> OpenAsync(User user)
    {
        if (user.AvatarGuid is null) return await OpenDefaultImageAsync();

        var url = user.AvatarGuid.Value.ToString();
        // var bitmap = await ImageCacheService.LoadFromGlobalCache(url);
        // if (bitmap != null) return bitmap;

        try
        {
            var image = Find(user.AvatarGuid)!;
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
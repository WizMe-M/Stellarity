using System.Threading.Tasks;
using Avalonia.Media.Imaging;


namespace Stellarity.Desktop.Basic;

/// <summary>
/// Represents async image loader
/// </summary>
public interface IAsyncImageLoader
{
    Task LoadAsync();
}
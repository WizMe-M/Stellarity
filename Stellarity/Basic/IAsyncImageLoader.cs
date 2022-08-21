using System.Drawing;
using System.Threading.Tasks;

namespace Stellarity.Basic;

/// <summary>
/// Represents async image loader
/// </summary>
public interface IAsyncImageLoader
{
    Task<Bitmap?> LoadAsync();
}
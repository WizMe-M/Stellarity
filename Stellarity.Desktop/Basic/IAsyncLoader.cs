using System.Threading.Tasks;

namespace Stellarity.Desktop.Basic;

/// <summary>
/// Represents async image loader
/// </summary>
public interface IAsyncLoader
{
    Task LoadAsync();
}
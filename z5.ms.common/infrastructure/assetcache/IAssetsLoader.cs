using System.Threading.Tasks;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Assets loader</summary>
    public interface IAssetsLoader
    {
        /// <summary>Load all assets from a backing storage into cache</summary>
        Task LoadAll();

        /// <summary>Report on asset loading progress.</summary>
        /// <returns>An number indiciating the percentage of assets loaded</returns>
        double Progress { get; }

        /// <summary>Indicate location from where assets will be loaded from. </summary>
        int Location { get; }
    }
}
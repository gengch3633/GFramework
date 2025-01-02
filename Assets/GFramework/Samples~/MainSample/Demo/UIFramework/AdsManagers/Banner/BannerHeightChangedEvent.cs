using System.Collections;
namespace Solitaire
{
    public class BannerHeightChangedEvent
    {
        public float bannerHeight;

        public BannerHeightChangedEvent(float bannerHeight)
        {
            this.bannerHeight = bannerHeight;
        }
    }
}


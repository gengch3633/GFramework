using UnityEngine.UI;

namespace GameFramework
{
    public class ImageExt:Image
    {
        private ImageAdapter ImageAdapter;
        public void Init(ImageAdapter imageAdapter)
        {
            this.ImageAdapter = imageAdapter;
        }

        public override void SetAllDirty()
        {
            base.SetAllDirty();
            ImageAdapter?.UpdateBgSize();
        }
    }
}


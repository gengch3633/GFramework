using GameFramework;
using Framework;
using UnityEngine.UI;

namespace GinRummy
{
    public class ThemeItem : MonoController
    {
        private Image themeSprite;
        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            themeSprite = this.GetComponent<Image>();
            themeSystem.BkgIndex.RegisterOnValueChanged(OnThemeChanged, true)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnThemeChanged(int themeIndex)
        {
            if (themeSprite == null)
                return;

            var themeSpriteNameWithIndex = themeSprite.sprite.name;
            themeSprite.sprite = resourceSystem.GetThemeSpriteFromResource(themeSpriteNameWithIndex, themeSystem.BkgIndex.Value);
        }
    }
}



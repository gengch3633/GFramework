using UnityEngine;
using Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.U2D;

namespace GameFramework
{
    public partial interface IResourceSystem : ISystem
    {
        Sprite GetSpriteFromAtlas(string atlasName, string spriteName);
        Sprite GetSpriteFromResource(string parentFolder, string spriteName);
        List<Sprite> GetSpritesFromAtlas(string atlasName);
        Sprite GetThemeSpriteFromResource(string themeSpriteNameWithIndex, int themeId);
    }

    public partial class ResourceSystem : AbstractSystem, IResourceSystem
    {
        private Dictionary<string, Dictionary<string, Sprite>> spriteAtlastDict = new Dictionary<string, Dictionary<string, Sprite>>();
        public List<Sprite> GetSpritesFromAtlas(string atlasName)
        {
            TryLoadSpriteAtlas(atlasName);
            var retSpriteDict = spriteAtlastDict[atlasName];
            var spriteList = retSpriteDict.Values.ToList();
            return spriteList;
        }

        public Sprite GetSpriteFromAtlas(string atlasName, string spriteName)
        {
            TryLoadSpriteAtlas(atlasName);
            var retSpriteDict = spriteAtlastDict[atlasName];
            var sprite = retSpriteDict[spriteName];
            return sprite;
        }

        public Sprite GetSpriteFromResource(string parentFolder, string spriteName)
        {
            var sprite = Resources.Load<Sprite>($"Sprites/{parentFolder}/{spriteName}");
            return sprite;
        }

        public Sprite GetThemeSpriteFromResource(string themeSpriteNameWithIndex, int themeId)
        {
            var subInfos = themeSpriteNameWithIndex.Split(new char[] { '_' }).ToList();
            var themeSpriteName = string.Join("_", subInfos.GetRange(0, subInfos.Count - 1));
            var spritePath = $"Sprites/theme/{themeSpriteName}_{themeId}";
            var sp = Resources.Load<Sprite>(spritePath);
            return sp;
        }

        private void TryLoadSpriteAtlas(string atlasName)
        {
            if (!spriteAtlastDict.ContainsKey(atlasName))
            {
                var spriteAtlas = Resources.Load<SpriteAtlas>($"Sprites/{atlasName}");

                Sprite[] spriteArray = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(spriteArray);
                var spriteList = spriteArray.ToList();

                var spriteDict = spriteList.ToDictionary(keySelector: item => item.name.Replace("(Clone)", ""), elementSelector: item => item);
                spriteAtlastDict.Add(atlasName, spriteDict);
            }
        }
    }
}


using Newtonsoft.Json;
using UnityEngine;

namespace Framework
{
    public class FileSaver: IFileSaver
    {
        private bool IsLogEnabled()
        {
            return false;
        }

        public void SaveInfo<T>(T t) where T : new()
        {
            string key = GetKey<T>();

            var infoString = JsonConvert.SerializeObject(t);
            if (IsLogEnabled()) Debug.LogError($"==> [FileSaver] SaveInfo: {key}\n{infoString}");
            PlayerPrefs.SetString(key, infoString);
        }

        public string GetKey<T>() where T : new()
        {
            var key = typeof(T).FullName;
            return key;
        }

        public T ReadInfoWithReturnNew<T>() where T : new()
        {
            var key = GetKey<T>();
            if (!HasKey(key))
                return new T();

            var infoString = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(infoString);
        }

        public T ReadInfoWithReturnNull<T>() where T : new()
        {
            var key = GetKey<T>();
            if (!HasKey(key))
                return default(T);

            var infoString = PlayerPrefs.GetString(key);
            if (IsLogEnabled()) Debug.LogError($"==> [FileSaver] ReadInfoWithReturnNull: {key}\n{infoString}");
            return JsonConvert.DeserializeObject<T>(infoString);
        }

        private bool HasKey(string key)
        {
            var hasKey = PlayerPrefs.HasKey(key);
            return hasKey;
        }

        public void Clear<T>() where T : new()
        {
            var key = GetKey<T>();
            if (HasKey(key))
                PlayerPrefs.DeleteKey(key);
        }
        public void CopyBindableClass<T>(T selfModel, T otherModel)
        {
            var selfReflection = new BindableTypeReflection<T>(selfModel);
            var otherReflection = new BindableTypeReflection<T>(otherModel);

            selfReflection.CopyBindableProperties(otherReflection);
            selfReflection.CopyFields(otherReflection);
        }
    }
}
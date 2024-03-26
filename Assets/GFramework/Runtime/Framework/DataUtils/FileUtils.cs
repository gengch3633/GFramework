using System;

namespace Framework
{
    public class FileUtils: Singleton<FileUtils>, IFileSaver
    {
        private FileSaver fileSaver = new FileSaver();

        public void Clear<T>() where T : new()
        {
            fileSaver.Clear<T>();
        }

        public void CopyBindableClass<T>(T selfModel, T otherModel,  Action callback = null)
        {
            fileSaver.CopyBindableClass(selfModel, otherModel, callback);
        }

        public string GetKey<T>() where T : new()
        {
            return fileSaver.GetKey<T>();
        }

        public T ReadInfoWithReturnNew<T>() where T : new()
        {
            return fileSaver.ReadInfoWithReturnNew<T>();
        }

        public T ReadInfoWithReturnNull<T>() where T : new()
        {
            return fileSaver.ReadInfoWithReturnNull<T>();
        }

        public void SaveInfo<T>(T t) where T : new()
        {
            fileSaver.SaveInfo(t);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface IFileSaver
    {
         void Clear<T>() where T : new();
        void CopyBindableClass<T>(T selfModel, T otherModel, Action callback = null);
        string GetKey<T>() where T : new();
        T ReadInfoWithReturnNew<T>() where T : new();
        T ReadInfoWithReturnNull<T>() where T : new();
        void SaveInfo<T>(T t) where T : new();
        void SaveInfo();
    }
}
﻿using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IArchitecture
    {
        void RegisterSystem<T>(T instance) where T : ISystem;
        void RegisterModel<T>(T instance) where T : IModel;

        void RegisterUtility<T>(T instance) where T : IUtility;

        T GetSystem<T>() where T : class, ISystem;
        T GetModel<T>() where T : class, IModel;
        T GetUtility<T>() where T : class, IUtility;

        void SendCommand<T>() where T : ICommand, new();

        void SendCommand<T>(T command) where T : ICommand;

        void SendEvent<T>() where T : new();

        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        void UnRegisterEvent<T>(Action<T> onEvent);
        List<ISystem> Systems { get; }
        List<IModel> Models { get; }
    }

    public abstract class Architecture<T1> : IArchitecture where T1 : Architecture<T1>, new()
    {
        private bool mInited = false;
        
        private List<ISystem> mSystems = new List<ISystem>();

        private List<IModel> mModels = new List<IModel>();
        public List<ISystem> Systems { get { return mSystems; } }
        public List<IModel> Models { get { return mModels; } }

        /// <summary>
        /// 注册System
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterSystem<T>(T instance) where T : ISystem
        {
            instance.SetArchitecture(this);
            mContainer.Register(instance);
            
            if (mInited)
            {
                instance.Init();
            }
            else
            {
                // 添加到 Model 缓存中，用于初始化
                mSystems.Add(instance);
            }
        }

        /// <summary>
        /// 注册Model
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterModel<T>(T instance) where T : IModel
        {
            instance.SetArchitecture(this);
            mContainer.Register<T>(instance);

            if (mInited)
            {
                instance.Init();
            }
            else
            {
                mModels.Add(instance);
            }
        }

        /// <summary>
        /// 注册Utility
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterUtility<T>(T instance) where T : IUtility
        {
            mContainer.Register<T>(instance);
        }

        /// <summary>
        /// 获取System
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSystem<T>() where T : class, ISystem
        {
            return mContainer.Get<T>();
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModel<T>() where T : class, IModel
        {
            return mContainer.Get<T>();
        }
        
        public static Action<T1> OnRegisterPatch = architecture=> { };

        private static T1 mArchitecture = null;

        public static IArchitecture Interface
        {
            get
            {
                if(mArchitecture == null) MakeSureArchitecture();

                return mArchitecture;
            }
        }

        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T1();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                foreach (var architectureModel in mArchitecture.mModels)
                    architectureModel.Init();

                foreach (var architectureSystem in mArchitecture.mSystems)
                    architectureSystem.Init();

                mArchitecture.mInited = true;
            }
        }

        private IOCContainer mContainer = new IOCContainer();

        protected abstract void Init();

        public void Register<T>(T instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register(instance);
        }

        public T GetUtility<T>() where T : class, IUtility
        {
            return mContainer.Get<T>();
        }

        public void SendCommand<T>() where T : ICommand, new()
        {
            var command = new T();
            command.SetArchitecture(this);
            command.Excute();
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            command.SetArchitecture(this);
            command.Excute();
        }

        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<T>() where T : new()
        {
            mTypeEventSystem.Send<T>();
        }

        public void SendEvent<T>(T e)
        {
            mTypeEventSystem.Send(e);
        }

        public IUnRegister RegisterEvent<T>(Action<T> onEvent)
        {
            return mTypeEventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<T>(Action<T> onEvent)
        {
            mTypeEventSystem.UnRegister(onEvent);
        }
    }
}
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public interface IAudioSystem : ISystem
    {
        void PlaySound(string soundName);
        void PlayBGM(string musicName);
        void StopBGM();
        public BindableProperty<bool> IsSoundOn { get; }
        public BindableProperty<bool> IsMusicOn { get; }
    }

    public class AudioSystem : AbstractSystem, IAudioSystem, ITypeLog
    {
        private List<AudioSource> _audioSources = new List<AudioSource>();
        private GameObject audioObj;
        public BindableProperty<bool> IsSoundOn { get; } = new BindableProperty<bool>() { Value = true };
        public BindableProperty<bool> IsMusicOn { get; } = new BindableProperty<bool>() { Value = true };

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetModel<IDebugModel>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(AudioSystem).FullName);
            return ret;
        }

        protected override void OnInit()
        {
            base.OnInit();
            var audioSystem = ReadInfoWithReturnNew<AudioSystem>();
            CopyBindableClass(this, audioSystem, ()=> SaveInfo(this));

            audioObj = new GameObject(typeof(AudioSystem).FullName);
            GameObject.DontDestroyOnLoad(audioObj);
        }

      

        public void StopBGM()
        {
            foreach (var VARIABLE in _audioSources)
                VARIABLE.Stop();
        }


        public void PlaySound(string soundName)
        {
            if(IsTypeLogEnabled()) Debug.LogError($"==> PlaySound 1: {soundName}");

            if (!IsSoundOn.Value)
                return;

            if (IsTypeLogEnabled()) Debug.LogError($"==> PlaySound 2: {soundName}");
            var clip = Resources.Load<AudioClip>($"Audios/{soundName}");
            PlaySound(clip, false);
        }

        public void PlayBGM(string bgName)
        {
            if (IsTypeLogEnabled()) Debug.LogError($"==> PlayBGM 1: {bgName}");
            if (!IsMusicOn.Value)
                return;

            if (IsTypeLogEnabled()) Debug.LogError($"==> PlayBGM 2: {bgName}");
            var clip = Resources.Load<AudioClip>($"Audios/{bgName}");
            PlaySound(clip, true);
        }

        private void PlaySound(AudioClip clip, bool loop)
        {
            foreach (var VARIABLE in _audioSources)
            {
                if (!VARIABLE.isPlaying)
                {
                    VARIABLE.clip = clip;
                    VARIABLE.loop = loop;
                    VARIABLE.Play();
                    return;
                }
            }

            var audio = audioObj.AddComponent<AudioSource>();
            audio.loop = loop;
            audio.clip = clip;
            _audioSources.Add(audio);
            audio.Play();
        }
    }
}
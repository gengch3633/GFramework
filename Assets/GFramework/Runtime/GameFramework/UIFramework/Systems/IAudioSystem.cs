using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public interface IAudioSystem : ISystem
    {
        void PlaySound(string soundName);
        void PlayMusic(string musicName);
        void StopMusic();
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
            return GameUtils.IsTypeLogEnabled(this);
        }

        protected override void OnInit()
        {
            base.OnInit();
            var audioSystem = ReadInfoWithReturnNew<AudioSystem>();
            CopyBindableClass(this, audioSystem, ()=> SaveInfo(this));

            audioObj = new GameObject(typeof(AudioSystem).FullName);
            GameObject.DontDestroyOnLoad(audioObj);
        }

      

        public void StopMusic()
        {
            foreach (var VARIABLE in _audioSources)
                VARIABLE.Stop();
        }


        public void PlaySound(string soundName)
        {
            GameUtils.Log(this, $"{soundName}");

            if (!IsSoundOn.Value)
                return;

            var clip = Resources.Load<AudioClip>($"Audios/{soundName}");
            PlaySound(clip, false);
        }

        public void PlayMusic(string bgName)
        {
            GameUtils.Log(this, $"bgName: {bgName}");
            if (!IsMusicOn.Value)
                return;

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
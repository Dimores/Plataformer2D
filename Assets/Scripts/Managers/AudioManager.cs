using System.Collections;
using System.Collections.Generic;
using Orby.Core.Singleton;
using UnityEngine;
using UnityEngine.Audio;
using static Orby.Managers.AudioManager;

namespace Orby.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public enum AudioType
        {
            SHOOT,
            JUMP,
            FALL,
            DAMAGE,
            SLIMEDAMAGE,
            DASH,
            MENUBUTTONHOVER,
            FIREBALLATTACK,
            FIREBALLEXPLOSION,
            SHOOTERATTACK,
            PORTALOPEN,
            PORTALCLOSE,
            LOADLASER,
            SHOOTLASER
        }

        public List<AudioManagerSetup> audioSetup;

        #region METHODS
        public void PlayMenuButtonHoverAudio(float volume)
        {
            AudioClip clip = GetAudioClip(AudioType.MENUBUTTONHOVER);
            if (clip == null)
            {
                return;
            }

            GameObject audioObj = new GameObject($"Audio_MENUBUTTONHOVER");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;

            audioSource.Play();

            Destroy(audioObj, clip.length);
        }

        public void PlayAudioByType(AudioType audioType)
        {
            AudioClip clip = GetAudioClip(audioType);
            if (clip == null)
            {
                return;
            }

            GameObject audioObj = new GameObject($"Audio_{audioType}");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;

            audioSource.Play();

            Destroy(audioObj, clip.length);
        }

        public void PlayAudioByType(AudioType audioType, float volume)
        {
            AudioClip clip = GetAudioClip(audioType);
            if (clip == null)
            {
                return;
            }

            GameObject audioObj = new GameObject($"Audio_{audioType}");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;

            audioSource.Play();

            Destroy(audioObj, clip.length);
        }

        public void PlayAudioByTypeWithRandomPitch(AudioType audioType, Vector2 random,
            float volume)
        {
            AudioClip clip = GetAudioClip(audioType);
            if (clip == null)
            {
                Debug.LogWarning($"Áudio do tipo {audioType} não encontrado!");
                return;
            }

            GameObject audioObj = new GameObject($"Audio_{audioType}");
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f);

            if (random.x > random.y)
            {
                (random.x, random.y) = (random.y, random.x); 
            }

            audioSource.pitch = Random.Range(random.x, random.y);

            audioSource.Play();

            Destroy(audioObj, clip.length / audioSource.pitch); 
        }


        private AudioClip GetAudioClip(AudioType audioType)
        {
            foreach (var setup in audioSetup)
            {
                if (setup.audioType == audioType)
                    return setup.audioClip;
            }
            return null; 
        }

        #endregion


    }

    [System.Serializable]
    public class AudioManagerSetup
    {
        public AudioManager.AudioType audioType;
        public AudioClip audioClip;
        public AudioMixerGroup outputGroup; 
    }
}

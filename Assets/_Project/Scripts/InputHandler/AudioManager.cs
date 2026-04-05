using ByNorth.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace ByNorth.InputHandler {
    public class AudioManager : Singleton<AudioManager> {
        [SerializeField] private AudioMixer audioMixer;

        protected override void Awake()
        {
            base.Awake();
            LoadVolume("masterVolume");
            LoadVolume("soundFXVolume");
            LoadVolume("musicVolume");
            LoadVolume("voiceVolume");
        }
        public void SetVolume(string parameter, float value)
        {
            // dB 변환 후 AudioMixer에 적용
            float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat(parameter, dB);
            
            // 값 저장
            PlayerPrefs.SetFloat(parameter, value);
            PlayerPrefs.Save();  // 디스크에 즉시 반영
        }
        private void LoadVolume(string parameter)
        {
            float value = PlayerPrefs.GetFloat(parameter, 1f);
            SetVolume(parameter, value);
        }

        // 개별 볼륨 설정용 메서드
        public void SetMasterVolume(float value) => SetVolume("masterVolume", value);
        public void SetMusicVolume(float value) => SetVolume("musicVolume", value);
        public void SetVoiceVolume(float value) => SetVolume("voiceVolume", value);
        public void SetSoundFXVolume(float value) => SetVolume("soundFXVolume", value);
        
        // 외부에서 현재 볼륨 조회 (UI 초기화용)
        public float GetVolume(string parameter)
        {
            return PlayerPrefs.GetFloat(parameter, 1f);
        }
    }
}

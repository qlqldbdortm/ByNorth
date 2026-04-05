using System.Collections;
using System.Collections.Generic;
using ByNorth.InputHandler;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth
{
    public class VolumeSliderControl : MonoBehaviour
    {
        [Header("음량 슬라이더")]
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider voiceSlider;
        public Slider soundFXSlider;

        private void Start()
        {
            // PlayerPrefs에서 슬라이더 값 불러오기
            float master = PlayerPrefs.GetFloat("masterVolume", 1f);
            float music = PlayerPrefs.GetFloat("musicVolume", 1f);
            float voice = PlayerPrefs.GetFloat("voiceVolume", 1f);
            float soundFX = PlayerPrefs.GetFloat("soundFXVolume", 1f);

            // 슬라이더의 이벤트를 임시로 제거한 다음, 초기값 세팅
            masterSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.RemoveAllListeners();
            voiceSlider.onValueChanged.RemoveAllListeners();
            soundFXSlider.onValueChanged.RemoveAllListeners();

            masterSlider.value = master;
            musicSlider.value = music;
            voiceSlider.value = voice;
            soundFXSlider.value = soundFX;

            // 이벤트 다시 연결
            masterSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.SetMasterVolume(value);
            });

            musicSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.SetMusicVolume(value);
            });

            voiceSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.SetVoiceVolume(value);
            });

            soundFXSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.SetSoundFXVolume(value);
            });
        }
    }
}

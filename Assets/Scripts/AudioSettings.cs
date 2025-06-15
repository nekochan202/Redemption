using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
    private const float DisabledVolume = -80f;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _mixerParameter;
    [SerializeField] private float _minimumVolume = -40f; 


    private void Start()
    {
        _volumeSlider.SetValueWithoutNotify(GetMixerVolume());
        _volumeSlider.onValueChanged.AddListener(UpdateMixerVolume);
    }

    public void UpdateMixerVolume(float sliderValue)
    {
        SetMixerVolume(sliderValue);
    }

    private void SetMixerVolume(float sliderValue)
    {
        float mixerVolume;

        if (sliderValue <= 0.001f)
        {
            mixerVolume = DisabledVolume;
        }
        else
        {
            mixerVolume = Mathf.Lerp(_minimumVolume, 0f, sliderValue);
        }

        _audioMixer.SetFloat(_mixerParameter, mixerVolume);
        PlayerPrefs.SetFloat(_mixerParameter, sliderValue);
        PlayerPrefs.Save();
    }

    private float GetMixerVolume()
    {
        if (!_audioMixer.GetFloat(_mixerParameter, out float mixerVolume))
        {
            Debug.LogError($"Failed to get parameter: {_mixerParameter}");
            return 1f;
        }

        if (mixerVolume <= DisabledVolume + 0.1f)
        {
            return 0f;
        }

        return Mathf.InverseLerp(_minimumVolume, 0f, mixerVolume);
    }
}
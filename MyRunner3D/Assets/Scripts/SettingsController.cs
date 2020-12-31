using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class SettingsController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceEffects;
    [SerializeField] private AudioSource audioSourceBG;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderEffects;
    [SerializeField] private Button musicOn, musicOff;

    [SerializeField] private Dropdown dropdownLvl;
    [SerializeField] private Text textLvl;

    [SerializeField] private Text coins;

    private void Start()
    {
        if (coins != null)
        {
            if (PlayerPrefs.HasKey("Coins"))
                coins.text = "Coins: " + PlayerPrefs.GetInt("Coins");
        }

        audioSourceEffects.mute = Parametrs.ASAudioMute;
        audioSourceBG.mute = Parametrs.ASAudioMute;

        sliderEffects.value = Parametrs.ASEffectsV;
        sliderMusic.value = Parametrs.ASBackgroundV;

        musicOn.gameObject.SetActive(!Parametrs.ASAudioMute);
        musicOff.gameObject.SetActive(Parametrs.ASAudioMute);

        if (dropdownLvl != null)
            dropdownLvl.value  = Parametrs.NUMBER_LVL;
        if (textLvl != null)
        {
            switch (Parametrs.NUMBER_LVL)
            {
                case 0:
                    textLvl.text = "EASY";
                    break;
                case 1:
                    textLvl.text = "MEDIUM";
                    break;
                default:
                    textLvl.text = "HARD";
                    break;
            }
        }
    }
    public void SoundOnOff(bool isSound)
    {
        audioSourceEffects.mute = isSound;
        audioSourceBG.mute = isSound;
        Parametrs.ASAudioMute = isSound;
    }
    public void VolumeMusic(float vol)
    {
        audioSourceBG.volume = vol;
        Parametrs.ASBackgroundV = vol;
    }
    public void VolumeSound(float vol)
    {
        audioSourceEffects.volume = vol;
        Parametrs.ASEffectsV = vol;
    }
    public void SelectLevel(int lvl)
    {
        Parametrs.LVL = Parametrs.LVLs[lvl];
        Parametrs.NUMBER_LVL = Parametrs.NUMBERs_LVL[lvl];
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button[] lvls;
    [SerializeField] private Text coinText;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < lvls.Length; i++)
            {
                lvls[i].interactable = i <= PlayerPrefs.GetInt("Lvl");
            }
        if (!PlayerPrefs.HasKey("hp"))
            PlayerPrefs.SetInt("hp", 0);
        if (!PlayerPrefs.HasKey("bg"))
            PlayerPrefs.SetInt("bg", 0);
        if (!PlayerPrefs.HasKey("gg"))
            PlayerPrefs.SetInt("gg", 0);
    }
    private void Update()
    {
        coinText.text = PlayerPrefs.HasKey("coins") ? PlayerPrefs.GetInt("coins").ToString() : "0";
    }
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelKey()
    {
        PlayerPrefs.DeleteAll();
        print("PlayerPrefs удалено");
    }
    public void Buy_hp(int cost)
    {
        if(PlayerPrefs.GetInt("coins")>=cost)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
    public void Buy_bg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("bg", PlayerPrefs.GetInt("bg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
    public void Buy_gg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("gg", PlayerPrefs.GetInt("gg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
}

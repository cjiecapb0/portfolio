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
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
#pragma warning disable 0649
public class Loading : MonoBehaviour
{
    [SerializeField] private Image[] gears;
    [SerializeField] private Slider slider;

    private void Start()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
    private void Update()
    {
        RotateGears();
    }
    private void RotateGears()
    {
        gears[0].transform.Rotate(new Vector3(0, 0, 40f * Time.deltaTime));
        gears[1].transform.Rotate(new Vector3(0, 0, -60f * Time.deltaTime));
        gears[2].transform.Rotate(new Vector3(0, 0, -80f * Time.deltaTime));
    }

    IEnumerator LoadScene(int number)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(number);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            if (operation.progress >= 0.9f)
            {
                if (number == 0)
                {
                    slider.value = 1;
                    yield return new WaitForSeconds(2f);
                    gameObject.SetActive(false);
                }
                else
                {
                    yield return null;
                    slider.value = 1;
                    gameObject.SetActive(false);
                }
            }
        }

        yield return null;
    }
}


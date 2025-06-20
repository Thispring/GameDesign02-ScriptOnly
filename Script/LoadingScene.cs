using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;
    [SerializeField]
    Image Bar;

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    void Update()
    {
        
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager. LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                Bar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                Bar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (Bar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
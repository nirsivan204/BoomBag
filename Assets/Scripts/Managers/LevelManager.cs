using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelMgr;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    private float target;
    private float startLoadingTime;
    bool isLoadingScene = false;
    [SerializeField] int timeToLoad = 5;

    // Start is called before the first frame update
    void Awake()
    {
        if (levelMgr)
        {
            Destroy(gameObject);
        }
        else
        {
            levelMgr = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    public async void loadScene(string sceneName)
    {
        startLoadingTime = Time.time;
        isLoadingScene = true;
        loadingBar.fillAmount = 0;
        target = 0;
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);


        await Task.Delay(timeToLoad*1000);
        target = scene.progress;
        isLoadingScene = false;
        scene.allowSceneActivation = true;
        await Task.Delay(1000);
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        if (isLoadingScene)
        {
            loadingBar.fillAmount = (Time.time - startLoadingTime) / timeToLoad;

        }
    }
}

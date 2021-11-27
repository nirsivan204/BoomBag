using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
public class LevelManager : MonoBehaviour
{
    public static LevelManager levelMgr;
    private GameObject loadingScreen;
    [SerializeField] GameObject loadingScreen_Mobile;
    [SerializeField] GameObject loadingScreen_PC;
    Image loadingBar;
    [SerializeField] GameManager GM;
    public VideoPlayer vid1;
    public VideoPlayer vid2;

    private float target;
    private float startLoadingTime;
    bool isLoadingScene = false;
    [SerializeField] int timeToLoad = 3;
    

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
        if(GM.isMobileGame)
        {
            loadingScreen = loadingScreen_Mobile;
        }
        else
        {
            loadingScreen = loadingScreen_PC;
        }
        loadingBar = loadingScreen.transform.Find("Image").Find("barProgress").GetComponent<Image>();
    }

    
    public async void loadScene(string sceneName)
    {
        startLoadingTime = Time.time;
        isLoadingScene = true;
        loadingBar.fillAmount = 0;
        target = 0;
        vid1.Play();
        vid2.Play();
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);


        await Task.Delay(timeToLoad*1000);
        target = scene.progress;
        isLoadingScene = false;
        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
        vid1.Stop();
        vid2.Stop();

    }

    private void Update()
    {
        if (isLoadingScene)
        {
            loadingBar.fillAmount = (Time.time - startLoadingTime) / timeToLoad;

        }
    }
}

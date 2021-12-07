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
    public VideoPlayer vid1;
    public VideoPlayer vid2;

    private float target;
    private float startLoadingTime;
    bool isLoadingScene = false;
    [SerializeField] int timeToLoad;
    private bool isInit = false;
    public static GameObject levelMgrPrefab;

    public enum Scenes {Manu,CharacterSelect,Game};

    private string getNameOfScene(Scenes sceneEnum)
    {
        switch (sceneEnum)
        {
            case Scenes.Manu:
                return "MainMenuScene";
            case Scenes.CharacterSelect:
                return "PickACharacterScene";
            case Scenes.Game:
                return "InitialTestScene";
        }
        return null;
    }

    // Start is called before the first frame update
/*    void Awake()
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
    }*/

    public  static LevelManager getInstance()
    {
        if (!levelMgr)
        {
            GameObject clone = Instantiate(Resources.Load("LevelMgr")) as GameObject;
            levelMgr = clone.GetComponent<LevelManager>();
            levelMgr.init();
        }
        return levelMgr;
    }

    public void init()
    {
        if (!isInit)
        {
            DontDestroyOnLoad(gameObject);

            isInit = true;
            if (gameParams.isMobile)
            {
                loadingScreen = loadingScreen_Mobile;
            }
            else
            {
                loadingScreen = loadingScreen_PC;
            }
            loadingBar = loadingScreen.transform.Find("Image").Find("barProgress").GetComponent<Image>();
        }
    }

    
    public async void loadScene(Scenes sceneName)
    {
        startLoadingTime = Time.time;
        isLoadingScene = true;
        loadingBar.fillAmount = 0;
        target = 0;
        vid1.Play();
        vid2.Play();
        AsyncOperation scene = SceneManager.LoadSceneAsync(getNameOfScene(sceneName));
        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        if(sceneName == Scenes.Game)
        {
            await Task.Delay(timeToLoad * 1000);
        }
        do
        {
            await Task.Delay(100);
        }
        while (scene.progress < 0.9f);
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

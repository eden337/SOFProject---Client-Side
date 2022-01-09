using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuController : MonoBehaviour
{
    public GameObject playBtn, quitBtn, randomBtn, therapyBtn, loadingPanel, numPadGO, backBtn, noSessionPanel,submitFailPanel;

    public GameObject sumbmitResults, tryAgain;

    public TextMeshProUGUI MazeAmountText, mazeSizeText;
    public Slider amountSlider, sizeSlider;

    public SessionManager sessionManager;

    public GameObject mazeConfigPanel;
    public Slider slider;

    public static MenuController instance = null;

    public static MenuController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public void EnableSessionButtons()
    {
        playBtn.SetActive(false);
        quitBtn.SetActive(false);
        numPadGO.SetActive(false);
        randomBtn.SetActive(true);
        therapyBtn.SetActive(true);
        backBtn.SetActive(true);

    }

    public void Quit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (MazeAmountText && mazeSizeText)
        {
            MazeAmountText.text = "Maze Amount: " + amountSlider.value;
            mazeSizeText.text = "Maze Size: " + sizeSlider.value;
        }
    }

    public void OpenMazeGenerationOption()
    {
        mazeConfigPanel.SetActive(true);
        playBtn.SetActive(false);
        quitBtn.SetActive(false);
        numPadGO.SetActive(false);
        randomBtn.SetActive(false);
        therapyBtn.SetActive(false);
        backBtn.SetActive(true);
    }

    public void LoadRandomizedSession()
    {
        // SceneManager.LoadScene("RandomizedSessionScene",LoadSceneMode.Single);
        if (SessionManager.instance == null) { return; }
        SessionManager.instance.StoreRandomizedSessionData((int)amountSlider.value, (int)sizeSlider.value);

        StartCoroutine(LoadAsyncronously(1));

    }

    public void LoadTherapySession()
    {
        // SceneManager.LoadScene("RandomizedSessionScene",LoadSceneMode.Single);
        StartCoroutine(LoadAsyncronously(2));

    }

    public void BackToMenu()
    {
        backBtn.SetActive(false);
        mazeConfigPanel.SetActive(false);
        playBtn.SetActive(true);
        quitBtn.SetActive(true);
        numPadGO.SetActive(false);
        randomBtn.SetActive(false);
        therapyBtn.SetActive(false);

    }

    public IEnumerator LoadAsyncronously(int sceneIndex)
    {
        instance.gameObject.SetActive(false);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }

    public void StartTherapySession()
    {
        numPadGO.SetActive(true);
        randomBtn.SetActive(false);
        therapyBtn.SetActive(false);
    }

    public void SubmitResults()
    {
        Debug.Log("Submmiting Results");
        if(SessionManager.instance!=null && SessionManager.instance.CurrentGameSession is TherapySession){
            ServerTalker.instance.ProcessSessionToDatabase(MazeManager.instance.mazeDataObjects);
        }else{
            StartCoroutine(LoadAsyncronously(0));
        }
        
    }

    public void TryAgain()
    {
        StartCoroutine(LoadAsyncronously(SceneManager.GetActiveScene().buildIndex));
    }

}

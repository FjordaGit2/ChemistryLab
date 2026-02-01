using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public static string UserName;
    public static bool IsVR;
    public static bool PlayerFreeze = false;
    [SerializeField] protected GameObject MainCamera = null;
    [SerializeField] protected GameObject VRCamera = null;
    [SerializeField] GameObject StartButton = null;
    [SerializeField] protected Canvas TaskCanvas = null;
    [HideInInspector] public bool isStarted = false;
    bool TaskLevel = true;
    void Start()
    {
        if (TaskLevel)
        {
            AudioListener.volume = 0;
            Time.timeScale = 0;
        }

        if (MainCamera && VRCamera)
        {
            MainCamera.SetActive(!IsVR);
            VRCamera.SetActive(IsVR);
        }
        if(!IsVR && StartButton)
        {
            StartButton.SetActive(false);
        }
    }
    public void StartTask()
    {
        if (TaskCanvas) TaskCanvas.enabled = false;
        isStarted = true;
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

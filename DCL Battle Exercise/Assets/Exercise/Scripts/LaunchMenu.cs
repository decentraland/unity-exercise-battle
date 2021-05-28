using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchMenu : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStart);
    }

    void OnStart()
    {
        SceneManager.LoadScene("Exercise/Scenes/Battle");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Toggle fullScreenToggle;

    private void Awake()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Resolution maxResolution = Screen.resolutions[^1];
        Resolution littleResolution = new();

        if (maxResolution.width == 1920 && maxResolution.height == 1080)
        {
            littleResolution.width = 1600;
            littleResolution.height = 900;
        }
        else if (maxResolution.width == 2560 && maxResolution.height == 1440)
        {
            littleResolution.width = 1920;
            littleResolution.height = 1080;
        }
        else return;

        if (isFullScreen) Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        else Screen.SetResolution(littleResolution.width, littleResolution.height, false);
    }

    public void SetAudio(bool isPlaying)
    {
        if (isPlaying) AudioManager.Instance.Play("Music");
        else AudioManager.Instance.Stop("Music");
    }
}

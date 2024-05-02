using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

[Serializable]
public class SceneNav : MonoBehaviour
{
    public enum Scenes
    {
        MainMenu,
        Red1Blue2,
        Red2Blue1,
        RedWin,
        BlueWin,
    }

    #region UNITY METHODS

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion UNITY METHODS
}
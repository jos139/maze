using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public enum SceneType
    {
        title = 0,
        setting = 1,
        main = 2
    }

    public static void MoveScene(SceneType type)
    {
        switch(type)
        {
            case SceneType.title:
                SceneManager.LoadScene("title");
                break;
            case SceneType.setting:
                SceneManager.LoadScene("setting");
                break;
            case SceneType.main:
                SceneManager.LoadScene("main");
                break;
        }
    }
}

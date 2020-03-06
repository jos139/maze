using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Advertisements;

public class TitlePresenter : MonoBehaviour
{
    private void Start()
    {
        Advertisement.Initialize("3133479");
    }

    public void MoveToMain()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }

        Scene.MoveScene(Scene.SceneType.main);
    }

    public void MoveToSetting()
    {
        Scene.MoveScene(Scene.SceneType.setting);
    }
}

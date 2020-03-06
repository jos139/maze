using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPresenter : MonoBehaviour
{
    [SerializeField] private SettingView view = null;

    private void Start()
    {
        this.view.Init();
    }

    public void ReturnToTitle()
    {
        Scene.MoveScene(Scene.SceneType.title);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingView : MonoBehaviour
{
    [SerializeField] private Bar etaBar = null;
    [SerializeField] private Bar gammaBar = null;
    [SerializeField] private Bar epsilonBar = null;
    [SerializeField] private Bar shrinkRateBar = null;
    [SerializeField] private Bar episodeMaxBar = null;
    [SerializeField] private Bar limit = null;

    public void Init()
    {
        this.etaBar.Init(Bar.ParamType.eta);
        this.gammaBar.Init(Bar.ParamType.gamma);
        this.epsilonBar.Init(Bar.ParamType.epsilon);
        this.shrinkRateBar.Init(Bar.ParamType.shrinkRate);
        this.episodeMaxBar.Init(Bar.ParamType.episodeMax);
        this.limit.Init(Bar.ParamType.limit);
    }
}

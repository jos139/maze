using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private Text text = null;

    private ParamType paramType = 0;

    public enum ParamType
    {
        eta = 0,
        gamma = 1,
        epsilon = 2,
        shrinkRate = 3,
        episodeMax = 4,
        limit = 5
    }

    public void Init(ParamType paramType)
    {
        this.paramType = paramType;
        switch (paramType)
        {
            case ParamType.eta:
                this.slider.value = Setting.Eta; ;
                break;
            case ParamType.gamma:
                this.slider.value = Setting.Gamma;
                break;
            case ParamType.epsilon:
                this.slider.value = Setting.Epsilon;
                break;
            case ParamType.shrinkRate:
                this.slider.value = Setting.ShrinkRate;
                break;
            case ParamType.episodeMax:
                this.slider.value = ((float)Setting.episodeMax) / 100000;
                break;
            case ParamType.limit:
                this.slider.value = ((float)Setting.limit) / 100000;
                break;
        }
        this.slider.onValueChanged.AddListener(value => Renew());
        Renew();
    }

    public void Renew()
    {
        float val = this.slider.value;
        switch (paramType)
        {
            case ParamType.episodeMax:
                this.text.text = ((int)(val * 100000)).ToString();
                break;
            case ParamType.limit:
                this.text.text = ((int)(val * 100000)).ToString();
                break;
            default:
                this.text.text = string.Format("{0:0.00}", val);
                break;
        }
        switch (paramType)
        {
            case ParamType.eta:
                Setting.Eta = val;
                break;
            case ParamType.gamma:
                Setting.Gamma = val;
                break;
            case ParamType.epsilon:
                Setting.Epsilon = val;
                break;
            case ParamType.shrinkRate:
                Setting.ShrinkRate = val;
                break;
            case ParamType.episodeMax:
                Setting.episodeMax = (int)(val * 100000);
                break;
            case ParamType.limit:
                Setting.limit = (int)(val * 100000);
                break;
        }
    }
}

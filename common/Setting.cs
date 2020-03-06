using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public static AlgorithmType algorithmType = AlgorithmType.Q;
    private static float eta = 0.1f;
    private static float gamma = 0.9f;
    private static float epsilon = 0.5f;
    private static float shrinkRate = 0.9f;
    public static int episodeMax = 10000;
    public static int limit = 100000;

    public enum AlgorithmType
    {
        Q = 0,
        Sarsa = 1
    }

    public static float Eta
    {
        get { return eta; }
        set { eta = Clip(value); }
    }

    public static float Gamma
    {
        get { return gamma; }
        set { gamma = Clip(value); }
    }

    public static float Epsilon
    {
        get { return epsilon; }
        set { epsilon = Clip(value); }
    }

    public static float ShrinkRate
    {
        get { return shrinkRate; }
        set { shrinkRate = Clip(value); }
    }

    public static float Clip(float f)
    {
        if (f > 1)
        {
            return 1;
        } else if (f < 0)
        {
            return 0;
        }
        return f;
    }
}

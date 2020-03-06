using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModel : MonoBehaviour
{
    readonly public int actionNum = 4;
    public int fieldSize = 0; // 迷路のサイズは(fieldSize)×(fieldSize)
    public int[] goalPos = null;
    public bool isSelecting = false; // 頂点を選択中かどうか
    public int[] selectedIndexs = null; // 選択中の頂点のインデックス
    public bool[,] activeLines_line = null; 
    public bool[,] activeLines_row = null;
    public bool canSelect = true;
    public int[] solution = null;
    public int[] answer = null;
    public bool isComplete = false;
    public float eta = 0.1f;
    public float gamma = 0.9f;
    public float epsilon = 0.5f;
    public float shrinkRate = 0.9f;
    public int episodeMax = 10000;
    public int limit = 100000;
    public int statusValue = 0;

    public MainModel(int fieldSize, float eta, float gamma, float epsilon, float shrinkRate, int episodeMax, int limit)
    {
        this.fieldSize = fieldSize;
        this.goalPos = new int[2] { fieldSize - 1, fieldSize - 1 };
        this.eta = eta;
        this.gamma = gamma;
        this.epsilon = epsilon;
        this.shrinkRate = shrinkRate;
        this.episodeMax = episodeMax;
        this.limit = limit;
    }

    /// <summary>
    /// θの初期値を取得
    /// </summary>
    /// <returns></returns>
    public MatrixUtil GetInitialThetaMatrix()
    {
        MatrixUtil matrix = MatrixUtil.Arange(fieldSize * fieldSize, actionNum);
        for (int state = 0; state < matrix.shape[0]; state++)
        {
            for (int action = 0; action < matrix.shape[1]; action++)
            {
                int i = state % fieldSize;
                int j = state / fieldSize;
                switch(action)
                {
                    case 0:
                        matrix.Set(state, action, activeLines_line[i, j + 1] ? 0 : 1);
                        break;
                    case 1:
                        matrix.Set(state, action, activeLines_row[i + 1, j] ? 0 : 1);
                        break;
                    case 2:
                        matrix.Set(state, action, activeLines_line[i, j] ? 0 : 1);
                        break;
                    case 3:
                        matrix.Set(state, action, activeLines_row[i, j] ? 0 : 1);
                        break;
                }
            }
        }
        return matrix;
    }

    public void SetSolution(int[] answer)
    {
        this.answer = answer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    private MainModel model = null;

    [SerializeField] private Drawer drawer = null;
    [SerializeField] private Text learning = null;
    [SerializeField] private Text status = null;

    private MazePanel[,] panels = null;
    private GameObject[,] vertexs = null;

    public void Init(MainModel model, MazePanel[,] panels, GameObject[,]vertexs)
    {
        this.model = model;
        this.panels = panels;
        this.vertexs = vertexs;

        Vector2[,] vertexPoss = new Vector2[model.fieldSize + 1, model.fieldSize + 1];
        for (int i = 0; i < model.fieldSize + 1; i++)
        {
            for (int j = 0; j < model.fieldSize + 1; j++)
            {
                vertexPoss[i, j] = vertexs[i, j].transform.position;
            }
        }
        this.drawer.Init(model.fieldSize, vertexPoss);
    }

    public void DrawOutLine()
    {
        // 上
        for (int i = 0; i < model.fieldSize; i++)
        {
            drawer.DrawLine(i, model.fieldSize, Drawer.LineType.line, false);
        }

        // 下
        for (int i = 0; i < model.fieldSize; i++)
        {
            drawer.DrawLine(i, 0, Drawer.LineType.line, false);
        }

        // 右
        for (int j = 0; j < model.fieldSize - 1; j++)
        {
            drawer.DrawLine(model.fieldSize, j, Drawer.LineType.row, false);
        }

        // 左
        for (int j = 1; j < model.fieldSize; j++)
        {
            drawer.DrawLine(0, j, Drawer.LineType.row, false);
        }
    }

    public void DrawLongLine(int startIndex, int endIndex, int fixedIndex, Drawer.LineType type)
    {
        switch(type)
        {
            case Drawer.LineType.line:
                for (int index = startIndex; index < endIndex; index++)
                {
                    drawer.DrawLine(index, fixedIndex, type, true);
                }
                break;
            case Drawer.LineType.row:
                for (int index = startIndex; index < endIndex; index++)
                {
                    drawer.DrawLine(fixedIndex, index, type, true);
                }
                break;
        }
    }

    /// <summary>
    /// 頂点を点滅させる
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public IEnumerator FlashVertex(int i, int j)
    {
        while(model.isSelecting)
        {
            vertexs[i, j].gameObject.SetActive(!vertexs[i, j].gameObject.activeSelf);
            yield return new WaitForSeconds(0.1f);
        }
        vertexs[i, j].gameObject.SetActive(true);
    }

    public IEnumerator FadeLearning()
    {
        this.learning.gameObject.SetActive(true);
        float speed = 0.01f;
        while(!model.isComplete)
        {
            Color color = this.learning.color;
            this.learning.color = new Color(color.r, color.g, color.b, color.a - speed);
            if (this.learning.color.a <= 0) this.learning.color = new Color(color.r, color.g, color.b, 1);
            yield return null;
        }
        this.learning.gameObject.SetActive(false);
    }

    public IEnumerator ShowStatus()
    {
        this.status.gameObject.SetActive(true);
        while(!model.isComplete)
        {
            this.status.text = $"{this.model.statusValue}%";
            yield return null;
        }
        this.status.gameObject.SetActive(false);
    }

    public bool[,] GetActiveLines_line()
    {
        return drawer.GetActiveLines_line();
    }

    public bool[,] GetActiveLines_row()
    {
        return drawer.GetActiveLines_row();
    }

    public void Undo()
    {
        this.drawer.UnableLastPanel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    private GameObject[,] lines_line = null;
    private GameObject[,] lines_row = null;

    Stack<PanelDto> queue_lines = null;
    private int fieldSize = 0;

    public enum LineType
    {
        line = 0,
        row = 1
    }

    public void Init(int fieldSize, Vector2[,] vertexPoss)
    {
        this.queue_lines = new Stack<PanelDto>();
        this.fieldSize = fieldSize;

        // 横
        lines_line = new GameObject[fieldSize, fieldSize + 1];
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize + 1; j++)
            {
                lines_line[i, j] = Instantiate(Resources.Load("Prefabs/main/line")) as GameObject;
                CreateLine(lines_line[i, j], vertexPoss[i, j], vertexPoss[i + 1, j]);
            }
        }
        // 縦
        lines_row = new GameObject[fieldSize + 1, fieldSize];
        for (int i = 0; i < fieldSize + 1; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                lines_row[i, j] = Instantiate(Resources.Load("Prefabs/main/line")) as GameObject;
                CreateLine(lines_row[i, j], vertexPoss[i, j], vertexPoss[i, j + 1]);
            }
        }

        DeleteAllLines();
    }

    public void CreateLine(GameObject line, Vector2 startPos, Vector2 endPos)
    {
        LineRenderer renderer = line.GetComponent<LineRenderer>();

        // 線の幅
        renderer.SetWidth(0.1f, 0.1f);
        // 頂点の数
        renderer.SetVertexCount(2);
        // 頂点を設定
        renderer.SetPosition(0, startPos);
        renderer.SetPosition(1, endPos);
    }

    public void DeleteAllLines()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize + 1; j++)
            {
                lines_line[i, j].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < fieldSize + 1; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                lines_row[i, j].gameObject.SetActive(false);
            }
        }
    }

    public void DrawLine(int i, int j, LineType type, bool push)
    {
        switch(type)
        {
            case LineType.line:
                lines_line[i, j].gameObject.SetActive(true);
                if (push) this.queue_lines.Push(new PanelDto() { index = new int[2] { i, j }, lineType = LineType.line }); 
                break;
            case LineType.row:
                lines_row[i, j].gameObject.SetActive(true);
                if (push) this.queue_lines.Push(new PanelDto() { index = new int[2] { i, j }, lineType = LineType.row });
                break;
        }
    }

    public void DeleteLine(int i, int j, LineType type)
    {
        switch (type)
        {
            case LineType.line:
                lines_line[i, j].gameObject.SetActive(false);
                break;
            case LineType.row:
                lines_row[i, j].gameObject.SetActive(false);
                break;
        }
    }

    public bool[,] GetActiveLines_line()
    {
        bool[,] activeLines_line = new bool[fieldSize, fieldSize + 1];
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize + 1; j++)
            {
                activeLines_line[i, j] = lines_line[i, j].activeSelf;
            }
        }
        return activeLines_line;
    }

    public bool[,] GetActiveLines_row()
    {
        bool[,] activeLines_row = new bool[fieldSize + 1, fieldSize];
        for (int i = 0; i < fieldSize + 1; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                activeLines_row[i, j] = lines_row[i, j].activeSelf;
            }
        }
        return activeLines_row;
    }

    public void UnableLastPanel()
    {
        if (this.queue_lines.Count != 0)
        {
            PanelDto dto = this.queue_lines.Pop();
            DeleteLine(dto.index[0], dto.index[1], dto.lineType);
        }
    }
}

public class PanelDto
{
    public int[] index = null;
    public Drawer.LineType lineType = 0;
}

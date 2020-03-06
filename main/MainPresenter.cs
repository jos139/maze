using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MainPresenter : MonoBehaviour
{
    private MainModel model = null;
    [SerializeField] private MainView view = null;
    [SerializeField] private Q_learning q_learning = null;
    [SerializeField] private Button solveButton = null;
    [SerializeField] private Button returnButton = null;
    [SerializeField] private Button againButton = null;
    [SerializeField] private Button undoButton = null;

    private MazePanel[,] panels = null;
    private GameObject[,] vertexs = null;

    private void Start()
    {
        Application.targetFrameRate = 60;

        // モデルを初期化
        model = new MainModel(
            8,
            Setting.Eta,
            Setting.Gamma,
            Setting.Epsilon,
            Setting.ShrinkRate,
            Setting.episodeMax,
            Setting.limit
        );

        // 自身を初期化
        Init();

        // viewを初期化
        view.Init(model, panels, vertexs);
        // 迷路の輪郭を描画
        view.DrawOutLine();
    }

    private void Update()
    {
        if (model.canSelect & Input.GetMouseButtonUp(0))
        {
            // Vector3でマウス位置座標を取得する
            Vector3 position = Input.mousePosition;
            // Z軸修正
            position.z = 10f;
            // マウス位置座標をスクリーン座標からワールド座標に変換する
            Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
            Vector2 mousePos = new Vector2(screenToWorldPointPosition.x, screenToWorldPointPosition.y);
            int[] indexs = GetSelectedVertex(mousePos);
            if (indexs != null)
            {
                int i = indexs[0];
                int j = indexs[1];

                if (model.isSelecting)
                {
                    // 同じ頂点を選択した場合には選択解除するだけ
                    if (!(i == model.selectedIndexs[0] & j == model.selectedIndexs[1]))
                    {
                        // 横に引く
                        if(j == model.selectedIndexs[1])
                        {
                            int startIndex = Mathf.Min(i, model.selectedIndexs[0]);
                            int endIndex = Mathf.Max(i, model.selectedIndexs[0]);
                            view.DrawLongLine(startIndex, endIndex, j, Drawer.LineType.line);
                        }
                        // 縦に引く
                        else if (i == model.selectedIndexs[0])
                        {
                            int startIndex = Mathf.Min(j, model.selectedIndexs[1]);
                            int endIndex = Mathf.Max(j, model.selectedIndexs[1]);
                            view.DrawLongLine(startIndex, endIndex, i, Drawer.LineType.row);
                        }
                    }
                    SyncIsSelecting(false);
                } else
                {
                    SyncIsSelecting(true);
                    SyncSelectedIndexs(indexs);
                    StartCoroutine(view.FlashVertex(i, j));
                }
            }
        }
    }

    private void Init()
    {
        panels = new MazePanel[model.fieldSize, model.fieldSize];
        vertexs = new GameObject[model.fieldSize + 1, model.fieldSize + 1];
        CreatePanels();
        CreateVertexs();
    }

    private void CreatePanels()
    {
        for (int i = 0; i < model.fieldSize; i++)
        {
            for (int j = 0; j < model.fieldSize; j++)
            {
                panels[i, j] = (Instantiate(Resources.Load("Prefabs/main/panel")) as GameObject).GetComponent<MazePanel>();
                float width = panels[i, j].transform.localScale.x;
                panels[i, j].transform.position = new Vector3(i * width, j * width, 1) + new Vector3(-(model.fieldSize - 1) * width / 2, -(model.fieldSize - 1) * width / 2, 0);
            }
        }

        panels[0, 0].SetStart();
        panels[model.fieldSize - 1, model.fieldSize - 1].SetGoal();
    }
    
    private void CreateVertexs()
    {
        float width = panels[0, 0].transform.localScale.x;
        for (int i = 0; i < model.fieldSize + 1; i++)
        {
            for (int j = 0; j < model.fieldSize + 1; j++)
            {
                vertexs[i, j] = Instantiate(Resources.Load("Prefabs/main/vertex")) as GameObject;
                vertexs[i, j].transform.position = new Vector2(i * width, j * width) + new Vector2(-width / 2, -width / 2) + new Vector2(-(model.fieldSize - 1) * width / 2, -(model.fieldSize - 1) * width / 2);
            }
        }
    }

    /// <summary>
    /// マウスが選択した頂点のインデックスを取得
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    public int[] GetSelectedVertex(Vector2 mousePos)
    {
        for (int i = 0; i < model.fieldSize + 1; i++)
        {
            for (int j = 0; j < model.fieldSize + 1; j++)
            {
                if ((mousePos - new Vector2(vertexs[i, j].transform.position.x, vertexs[i, j].transform.position.y)).magnitude < 0.2f)
                {
                    return new int[2] { i, j };
                }
            }
        }
        return null;
    }

    public void SyncActiveLines()
    {
        this.model.activeLines_line = this.view.GetActiveLines_line();
        this.model.activeLines_row = this.view.GetActiveLines_row();

        // スタートとゴールの部分の補正
        this.model.activeLines_row[0, 0] = true;
        this.model.activeLines_row[model.fieldSize, model.fieldSize - 1] = true;
    }

    public void SyncSelectedIndexs(int[] indexs)
    {
        this.model.selectedIndexs = indexs;
    }

    public void SyncIsSelecting(bool isSelecting)
    {
        this.model.isSelecting = isSelecting;
    }

    public void SyncCanSelect(bool canSelect)
    {
        this.model.canSelect = canSelect;
    }

    /// <summary>
    /// 迷路を解く
    /// </summary>
    public void Solve()
    {
        if (model.canSelect)
        {
            StartCoroutine(SolveCoroutine());
        }
    }

    public IEnumerator SolveCoroutine()
    {
        this.solveButton.gameObject.SetActive(false);
        this.undoButton.gameObject.SetActive(false);
        SyncActiveLines();
        SyncCanSelect(false);
        MazeChecker checker = new MazeChecker();
        yield return StartCoroutine(checker.MazeCheck(model));
        if (!checker.canBeSolved)
        {
            Again();
            yield break;
        }
        this.q_learning.Init(this.model, model.fieldSize, model.GetInitialThetaMatrix());
        StartCoroutine(this.view.FadeLearning());
        StartCoroutine(this.view.ShowStatus());
        StartCoroutine(this.q_learning.Solve());
        StartCoroutine(WaitAndTrace());
    }

    public IEnumerator WaitAndTrace()
    {
        while (!model.isComplete)
        {
            yield return null;
        }
        foreach(int state in model.answer)
        {
            int i = state % model.fieldSize;
            int j = state / model.fieldSize;
            this.panels[i, j].ChangeColor();
        }
        this.returnButton.gameObject.SetActive(true);
        this.againButton.gameObject.SetActive(true);
    }

    public void ReturnToTitle()
    {
        Scene.MoveScene(Scene.SceneType.title);
    }

    public void Again()
    {
        Scene.MoveScene(Scene.SceneType.main);
    }

    public void Undo()
    {
        this.view.Undo();
    }
}

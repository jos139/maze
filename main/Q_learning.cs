using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Threading;

public class Q_learning : MonoBehaviour
{
    private MainModel model;
    private int fieldSize = 0;
    private float eta = 0.1f;
    private float gamma = 0.9f;
    private float epsilon = 0.5f;
    private float shrinkRate = 0.9f;
    private int stateNum = 0;
    private int actionNum = 0;
    private MatrixUtil Q = null;
    private int[] answer = null;
    private int episode = 0;
    private int episodeMax = 10000;
    private int limit = 100000;
    private System.Random r = null;
    public int statusValue = 0;

    private enum ActionType
    {
        up = 0,
        right = 1,
        down = 2,
        left = 3
    }

    public IEnumerator Solve()
    {
        Thread t = new Thread(new ThreadStart(() =>
        {
            while (this.episode < this.episodeMax)
            {
                Step();
            }
        }));
        t.Start();
        while (t.IsAlive)
        {
            yield return null;
        }
        SyncIsComplete(true);
    }

    public void Init(MainModel model, int fieldSize, MatrixUtil initialTheta)
    {
        this.model = model;
        this.fieldSize = fieldSize;
        this.eta = this.model.eta;
        this.gamma = this.model.gamma;
        this.epsilon = this.model.epsilon;
        this.shrinkRate = this.model.shrinkRate;
        this.episodeMax = this.model.episodeMax;
        this.limit = this.model.limit;
        this.stateNum = initialTheta.shape[0];
        this.actionNum = initialTheta.shape[1];
        r = new System.Random();
        Q = MatrixUtil.Arange(stateNum, actionNum);
        MatrixUtil.UniFuncArangeAll(Q, () => (float)(r.NextDouble()));
        Q = Q.Multiply(initialTheta);
    }

    private void Step()
    {
        List<int[]> s_a_history = GoalMazeRetSAQ();

        epsilon = epsilon * shrinkRate;
        episode++;
        this.statusValue = (int)((((float)this.episode) / this.episodeMax) * 100);
        SyncStatusValue();
        if (episode == episodeMax - 1)
        {
            int[] footPrint = new int[s_a_history.Count];
            for (int i = 0; i < s_a_history.Count; i++)
            {
                footPrint[i] = s_a_history[i][0];
            }
            this.answer = footPrint;
            this.model.SetSolution(this.answer);
        }
    }
    private List<int[]> GoalMazeRetSAQ()
    {
        List<int[]> s_a_history = new List<int[]>();
        int s = 0;
        int a = (int)GetAction(s);
        int a_next = a;
        s_a_history.Add(new int[2] { s, -1 });

        int counter = 0;
        while (counter < limit)
        {
            a = a_next;

            s_a_history.Last()[1] = a;

            int s_next = GetSNext(s, a);

            s_a_history.Add(new int[2] { s_next, -1 });

            int r = 0;

            if (s_next == stateNum - 1)
            {
                r = 1;
                a_next = -1;
            }
            else
            {
                a_next = (int)GetAction(s_next);
            }

            RenewQ(s, a, r, s_next);

            if (s_next == this.stateNum - 1)
            {
                break;
            }
            else
            {
                s = s_next;
            }

            counter++;
        }
        return s_a_history;
    }
    private ActionType GetAction(int s)
    {
        ActionType[] types = new ActionType[4] { ActionType.up, ActionType.right, ActionType.down, ActionType.left };
        if ((float)r.NextDouble() < this.epsilon)
        {
            int[] choices = Q.GetLine(s).Select((x, i) => new { x, i }).Where(obj => obj.x != 0).Select(obj => obj.i).ToArray();
            return types[choices[r.Next(choices.Length)]];
        }
        else
        {
            return types[Q.GetLine(s).Select((x, i) => new { x, i }).Aggregate((max, working) => (max.x > working.x) ? max : working).i];
        }
    }

    private int GetSNext(int s, int a)
    {
        ActionType[] types = new ActionType[4] { ActionType.up, ActionType.right, ActionType.down, ActionType.left };
        ActionType nextDirection = types[a];

        switch (nextDirection)
        {
            case ActionType.up:
                return s + fieldSize;
            case ActionType.right:
                return s + 1;
            case ActionType.down:
                return s - fieldSize;
            case ActionType.left:
                return s - 1;
        }

        return -1;
    }

    private void RenewQ(int s, int a, int r, int s_next)
    {
        if (s_next == this.stateNum - 1)
        {
            Q.Set(s, a, Q.Get(s, a) + eta * (r - Q.Get(s, a)));
        }
        else
        {
            Q.Set(s, a, Q.Get(s, a) + eta * (r + gamma * Q.GetLine(s_next).Aggregate((max, working) => (max > working) ? max : working) - Q.Get(s, a)));
        }
    }

    public void SyncIsComplete(bool isComplete)
    {
        this.model.isComplete = true;
    }

    public void SyncStatusValue()
    {
        this.model.statusValue = (int)statusValue;
    }
}

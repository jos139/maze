using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using System.Threading;

public class MazeChecker: MonoBehaviour
{
    public bool canBeSolved = false;

    public IEnumerator MazeCheck(MainModel model)
    {
        Agent agent = new Agent();
        Thread t = new Thread(new ThreadStart(() =>
        {
            int counter = 0;
            int limit = model.fieldSize * (model.fieldSize + 1) * 2 * 2;
            while (counter < limit)
            {
                Step(ref agent, model.activeLines_line, model.activeLines_row);
                if (agent.index[0] == model.goalPos[0] & agent.index[1] == model.goalPos[1])
                {
                    this.canBeSolved = true;
                    break;
                }
                counter++;
            }
        }));
        t.Start();
        while (t.IsAlive)
        {
            yield return null;
        }
    }

    public void Step(ref Agent agent, bool[,] activeLines_line, bool[,] activeLines_row)
    {
        switch (agent.vectorType)
        {
            case Agent.VectorType.UP:
                if (!activeLines_row[agent.index[0] + 1, agent.index[1]])
                {
                    agent.index[0] += 1;
                    agent.vectorType = Agent.VectorType.RIGHT;
                }
                else if (!activeLines_line[agent.index[0], agent.index[1] + 1])
                {
                    agent.index[1] += 1;
                    agent.vectorType = Agent.VectorType.UP;
                }
                else if (!activeLines_row[agent.index[0], agent.index[1]])
                {
                    agent.index[0] += -1;
                    agent.vectorType = Agent.VectorType.LEFT;
                }
                else
                {
                    agent.index[1] += -1;
                    agent.vectorType = Agent.VectorType.DOWN;
                }
                break;
            case Agent.VectorType.RIGHT:
                if (!activeLines_line[agent.index[0], agent.index[1]])
                {
                    agent.index[1] += -1;
                    agent.vectorType = Agent.VectorType.DOWN;
                } else if (!activeLines_row[agent.index[0] + 1, agent.index[1]])
                {
                    agent.index[0] += 1;
                    agent.vectorType = Agent.VectorType.RIGHT;
                } else if (!activeLines_line[agent.index[0], agent.index[1] + 1]) {
                    agent.index[1] += 1;
                    agent.vectorType = Agent.VectorType.UP;
                } else if (!activeLines_row[agent.index[0], agent.index[1]])
                {
                    agent.index[0] += -1;
                    agent.vectorType = Agent.VectorType.LEFT;
                }
                break;
            case Agent.VectorType.DOWN:
                if (!activeLines_row[agent.index[0], agent.index[1]])
                {
                    agent.index[0] += -1;
                    agent.vectorType = Agent.VectorType.LEFT;
                }
                else if (!activeLines_line[agent.index[0], agent.index[1]])
                {
                    agent.index[1] += -1;
                    agent.vectorType = Agent.VectorType.DOWN;
                }
                else if (!activeLines_row[agent.index[0] + 1, agent.index[1]])
                {
                    agent.index[0] += 1;
                    agent.vectorType = Agent.VectorType.RIGHT;
                }
                else
                {
                    agent.index[1] += 1;
                    agent.vectorType = Agent.VectorType.UP;
                }
                break;
            case Agent.VectorType.LEFT:
                if (!activeLines_line[agent.index[0], agent.index[1] + 1])
                {
                    agent.index[1] += 1;
                    agent.vectorType = Agent.VectorType.UP;
                }
                else if (!activeLines_row[agent.index[0], agent.index[1]])
                {
                    agent.index[0] += -1;
                    agent.vectorType = Agent.VectorType.LEFT;
                }
                else if (!activeLines_line[agent.index[0], agent.index[1]])
                {
                    agent.index[1] += -1;
                    agent.vectorType = Agent.VectorType.DOWN;
                }
                else
                {
                    agent.index[0] += 1;
                    agent.vectorType = Agent.VectorType.RIGHT;
                }
                break;
        }
    }
}

public class Agent
{
    public int[] index = null;
    public VectorType vectorType = 0;

    public Agent()
    {
        this.index = new int[2] { 0, 0 };
        this.vectorType = VectorType.RIGHT;
    }

    public enum VectorType
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }
}

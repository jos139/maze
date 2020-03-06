using System.Collections;
using System.Collections.Generic;

public class MatrixUtil
{
    private float[,] array = null;
    public int[] shape = null;

    private MatrixUtil(int line, int row)
    {
        this.array = new float[line, row];
        this.shape = new int[2] { line, row };
        Zeros(this);
    } 

    public static MatrixUtil Arange(int line, int row)
    {
        return new MatrixUtil(line, row);
    }

    public static void Zeros(MatrixUtil matrix)
    {
        UniFuncArangeAll(matrix, () => 0);
    }

    public static void Ones(MatrixUtil matrix)
    {
        UniFuncArangeAll(matrix, () => 1);
    }

    public static void UniFuncArangeAll(MatrixUtil matrix, MatrixFunc func)
    {
        for(int i = 0; i < matrix.shape[0]; i++)
        {
            for (int j = 0; j < matrix.shape[1]; j++)
            {
                matrix.Set(i, j, func());
            }
        }
    }

    public MatrixUtil Multiply(MatrixUtil matrix)
    {
        MatrixUtil resultMatrix = Arange(this.shape[0], this.shape[1]);
        for (int i = 0; i < this.shape[0]; i++)
        {
            for (int j = 0; j < this.shape[1]; j++)
            {
                resultMatrix.Set(i, j, this.Get(i, j) * matrix.Get(i, j));
            }
        }
        return resultMatrix;
    }

    public float Get(int i, int j)
    {
        return this.array[i, j];
    }

    public void Set(int i, int j, float value)
    {
        this.array[i, j] = value;
    }

    public float[] GetLine(int i)
    {
        float[] line = new float[this.shape[1]];
        for (int j = 0; j < this.shape[1]; j++)
        {
            line[j] = this.array[i, j];
        }
        return line;
    }

    public float[] GetRow(int j)
    {
        float[] row = new float[this.shape[0]];
        for (int i = 0; i < this.shape[0]; i++)
        {
            row[i] = this.array[i, j];
        }
        return row;
    }

    public delegate float MatrixFunc();
}

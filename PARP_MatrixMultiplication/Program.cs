using System;
using System.Diagnostics;

int[,] a = MatrixHelper.InitializeMatrix(1024, 1024);
int[,] b = MatrixHelper.InitializeMatrix(1024, 1024);

Stopwatch stopwatch = Stopwatch.StartNew();
var resultSeq = MatrixHelper.MultiplyMatricesSequential(a, b);
stopwatch.Stop();
Console.WriteLine($"Sequential: {stopwatch.ElapsedMilliseconds} ms");

stopwatch.Restart();
var resultPar = MatrixHelper.MultiplyMatricesParallel(a, b);
stopwatch.Stop();
Console.WriteLine($"Parallel: {stopwatch.ElapsedMilliseconds} ms");


public static class MatrixHelper
{
    private static Random random = new Random();

    public static int[,] InitializeMatrix(int rows, int cols)
    {
        int[,] matrix = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = random.Next(10); // Random integers between 0 and 9
            }
        }
        return matrix;
    }

    public static int[,] MultiplyMatricesSequential(int[,] a, int[,] b)
    {
        int aRows = a.GetLength(0);
        int aCols = a.GetLength(1);
        int bRows = b.GetLength(0);
        int bCols = b.GetLength(1);
        int[,] result = new int[aRows, bCols];

        for (int i = 0; i < aRows; i++)
        {
            for (int j = 0; j < bCols; j++)
            {
                for (int k = 0; k < aCols; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return result;
    }

    public static int[,] MultiplyMatricesParallel(int[,] a, int[,] b)
    {
        int aRows = a.GetLength(0);
        int bCols = b.GetLength(1);
        int[,] result = new int[aRows, bCols];

        Parallel.For(0, aRows, i =>
        {
            for (int j = 0; j < bCols; j++)
            {
                int temp = 0;
                for (int k = 0; k < a.GetLength(1); k++)
                {
                    temp += a[i, k] * b[k, j];
                }
                result[i, j] = temp;
            }
        });

        return result;
    }

}

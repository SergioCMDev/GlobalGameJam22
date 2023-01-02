using System.Collections.Generic;

public static class ExtensionList
{
    public static List<T> ToList<T>(this T[,] matrixToRotate)
    {
        var newMatrix = new List<T>(matrixToRotate.Length);

        foreach (var VARIABLE in matrixToRotate)
        {
            newMatrix.Add(VARIABLE);
        }

        return newMatrix;
    }

    public static T[,] RotateMatrix<T>(this T[,] oldMatrix)
    {
        var rows = oldMatrix.GetLength(1);
        var columns = oldMatrix.GetLength(0);

        var newMatrix = new T[rows, columns];
        var newRow = rows - 1;
        var newColum = columns - 1;

        for (var oldRow = 0; oldRow < rows; oldRow++)
        {
            for (var oldColumn = 0; oldColumn < columns; oldColumn++)
            {
                newMatrix[newRow, newColum] = oldMatrix[oldRow, oldColumn];
                newRow--;
            }

            newColum--;
            newRow = rows - 1;
        }

        return newMatrix;
    }
}
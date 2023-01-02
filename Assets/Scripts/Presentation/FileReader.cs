using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Presentation
{
    public class AttackRangeData
    {
        public int rowOfArray, columnsOfArray;
        public int[,] array;
        public List<int> arrayList;
        public Vector3Int Area;

        public AttackRangeData(int rowOfArray, int columnsOfArray, int[,] array, List<int> list)
        {
            this.rowOfArray = rowOfArray;
            this.columnsOfArray = columnsOfArray;
            this.array = array;
            this.arrayList = array.ToList();
            Area = new Vector3Int(rowOfArray, columnsOfArray, 1);
        }
    }

    public static class FileReader
    {
        // public AttackRange attackRange;

        public static AttackRangeData ReadFile(string path)
        {
            var lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length < 2) return null;
            var rows = int.Parse(lines[0]);
            var columns = int.Parse(lines[1]);
            var arrayList = new List<int>();
            var effectValue = new int[rows, columns];
            int newRow = 0, newColumn = 0;
            try
            {
                for (int i = 2; i < lines.Length; i++)
                {
                    var values = lines[i].Split(' ');
                    foreach (var value in values)
                    {
                        effectValue[newRow, newColumn] = int.Parse(value);
                        arrayList.Add(int.Parse(value));
                        newColumn++;
                    }

                    newRow++;
                    newColumn = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            effectValue = effectValue.RotateMatrix();
            var attackRange = new AttackRangeData(rows, columns, effectValue, arrayList);
            return attackRange;
        }
    }
}

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
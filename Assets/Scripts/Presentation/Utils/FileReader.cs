using System;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Utils
{
    public class AttackRangeData
    {
        public List<int> arrayList;
        public Vector3Int Area;

        public AttackRangeData(int rowOfArray, int columnsOfArray, int[,] array)
        {
            this.arrayList = array.ToList();
            Area = new Vector3Int(rowOfArray, columnsOfArray, 1);
        }
    }

    public static class FileReader
    {
        public static AttackRangeData ReadFile(string path)
        {
            var lines = path.Split("\r\n");
            if (lines.Length < 2) return null;
            var rows = int.Parse(lines[0]);
            var columns = int.Parse(lines[1]);
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
            var attackRange = new AttackRangeData(rows, columns, effectValue);
            return attackRange;
        }
    }
}
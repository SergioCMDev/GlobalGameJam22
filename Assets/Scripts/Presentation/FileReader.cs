using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Presentation
{
    public class AttackRange
    {
        public int rowOfArray, columnsOfArray;
        public int[,] array;

        public AttackRange(int rowOfArray, int columnsOfArray, int[,] array)
        {
            this.rowOfArray = rowOfArray;
            this.columnsOfArray = columnsOfArray;
            this.array = array;
        }
    }

    public class FileReader : MonoBehaviour
    {
        public AttackRange attackRange;

        void Start()
        {
            var path = "Assets/Test.txt";
            var lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length < 2) return;
            var rows = int.Parse(lines[0]);
            var columns = int.Parse(lines[1]);

            var effectValue = new int[rows, columns];
            int newRow = 0, newColumn = 0;
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

            attackRange = new AttackRange(rows, columns, effectValue);

            Debug.Log("D");
        }
    }
}
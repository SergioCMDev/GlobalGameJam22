using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileReader : MonoBehaviour
{
    void Start()
    {
        var path = "Assets/Test.txt";
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
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


        Debug.Log("D");
    }
}
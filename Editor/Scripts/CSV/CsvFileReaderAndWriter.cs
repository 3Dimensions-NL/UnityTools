using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Tools.Editor.Scripts.CSV
{
    public static class CsvFileReaderAndWriter
    {
        public static void WriteCsv(string[] rows)
        {
            string path = EditorUtility.SaveFilePanel("Save Csv", Application.dataPath, "NewCsv.csv", "csv");

            //First row to declare base string;
            string textToSave = rows[0] + "\n";

            for (int i = 1; i < rows.Length; i++)
            {
                textToSave += rows[i] + "\n";
            }
            
            File.WriteAllText(path, textToSave);
            Debug.Log("CSV file written to: " + path + "\nString:\n" +  textToSave);
        }

        public static string GetCsvStringFromFile()
        {
            string path = EditorUtility.OpenFilePanel("Open Csv", Application.dataPath, "csv");
            return File.ReadAllText(path);
        }
        
        public static string[][] GetCsvData(bool includeHeader)
        {
            string path = EditorUtility.OpenFilePanel("Open Csv", Application.dataPath, "csv");

            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("File not found!", "File could not be found at path: " + path, "Cancel import");
                return null;
            }
            

            string csvString = File.ReadAllText(path);

            List<List<string>> lines = new List<List<string>>();
            
            int file_length = csvString.Length;

            // read char by char and when a , or \n, perform appropriate action
            int cur_file_index = 0; // index in the file
            List<string> cur_line = new List<string>(); // current line of data
            int cur_line_number = 0;
            StringBuilder cur_item = new StringBuilder("");
            bool inside_quotes = false; // managing quotes
            while (cur_file_index < file_length)
            {
                char c = csvString[cur_file_index++];

                switch (c)
                {
                    case '"':
                        if (!inside_quotes)
                        {
                            inside_quotes = true;
                        }
                        else
                        {
                            if (cur_file_index == file_length)
                            {
                                // end of file
                                inside_quotes = false;
                                goto case '\n';
                            }
                            else if (csvString[cur_file_index] == '"')
                            {
                                // double quote, save one
                                cur_item.Append("\"");
                                cur_file_index++;
                            }
                            else
                            {
                                // leaving quotes section
                                inside_quotes = false;
                            }
                        }
                        break;
                    case '\r':
                        // ignore it completely
                        break;
                    case ',':
                        goto case '\n';
                    case '\n':
                        if (inside_quotes)
                        {
                            // inside quotes, this characters must be included
                            cur_item.Append(c);
                        }
                        else
                        {
                            // end of current item
                            cur_line.Add(cur_item.ToString());
                            // Debug.Log("Adding item: " + cur_item);

                            cur_item.Length = 0;
                            if (c == '\n' || cur_file_index == file_length)
                            {
                                // also end of line, call line reader
                                
                                // Debug.Log("Row finished line: " + cur_line[0]);

                                lines.Add(new List<string>(cur_line));
                                
                                cur_line.Clear();
                            }
                        }
                        break;
                    default:
                        // other cases, add char
                        cur_item.Append(c);
                        break;
                }
            }
            
            
            //Store collected data in 2d array
            if (lines.Count <= 0) return null;
            int linesCount = lines.Count;
            int itemsCount = lines[0].Count;
            
            string[][] importedData = new string[linesCount][];

            for (int i = 0; i < lines.Count; i++)
            {
                importedData[i] = new string[itemsCount];
                
                for (int j = 0; j < itemsCount; j++)
                {
                    importedData[i][j] = lines[i][j];
                }
            }

            return importedData;
        }
        
    }
}

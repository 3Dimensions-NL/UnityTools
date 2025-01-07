using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.CSV
{
    public static class CsvReader
    {
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<Dictionary<string, object>> ReadFile(string file, string regex)
        {
            TextAsset data = Resources.Load(file) as TextAsset;
            return ReadTextAsset(data, regex);
        }
        
        public static List<Dictionary<string, object>> ReadTextAsset(TextAsset data, string regex)
        {
            return ReadCsvString(data.text, regex);
        }
        
        public static List<Dictionary<string, object>> ReadCsvString(string csvData, string regex)
        {
            var list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(csvData, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;
            var header = Regex.Split(lines[0], regex);

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], regex);
                
                if (values.Length == 0 || values.Length != header.Length) continue;
 
                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    object finalValue = value;
                    int n;
                    float f;
                    if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                    {
                        finalValue = f;
                    }
                    else if (int.TryParse(value, out n))
                    {
                        finalValue = n;
                    }
                    
                    entry[header[j]] = finalValue;
                }
                
                list.Add(entry);
            }
            return list;
        }
        
        public static List<Dictionary<string, object>> ReadCsvString(string csvData, string regex, string regexLineBrake)
        {
            var list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(csvData, regexLineBrake);

            if (lines.Length <= 1) return list;
            var header = Regex.Split(lines[0], regex);

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], regex);
                
                if (values.Length == 0 || values.Length != header.Length) continue;
 
                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    object finalValue = value;
                    int n;
                    float f;
                    if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                    {
                        finalValue = f;
                    }
                    else if (int.TryParse(value, out n))
                    {
                        finalValue = n;
                    }
                    
                    entry[header[j]] = finalValue;
                }
                
                list.Add(entry);
            }
            return list;
        }

        public static string[] GetRowsFromCsvString(string csvData, string regex, string regexLineBrake)
        {
            var list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(csvData, regexLineBrake);

            return lines;
        }
    }
}
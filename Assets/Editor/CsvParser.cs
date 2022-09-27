using System.IO;
using UnityEditor;
using UnityEngine;

public class CsvParser
{
    private static string csvPath = Application.dataPath + "/Editor/Csv/word_gender_sample(french).csv";
    private static string wordsPath ="Assets/Words/";
    private const char CSV_SEPERATOR = ',';

    [MenuItem("CsvParser/Generate Word Objects")]
    public static void GenerateEnemies()
    {
        string[] lines = File.ReadAllLines(csvPath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] wordAndGender = lines[i].Split(CSV_SEPERATOR);
            WordData wordData = ScriptableObject.CreateInstance<WordData>();
            wordData.word = wordAndGender[0];
            wordData.isMasculine = wordAndGender[1].Contains('m');
            AssetDatabase.CreateAsset(wordData,wordsPath+wordData.word+".asset");
        }
        AssetDatabase.SaveAssets();
        
    }
}

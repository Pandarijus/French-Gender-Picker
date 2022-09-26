using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wordText,scoreText;
    [SerializeField] Button feminineButton,masculineButton;
    [SerializeField] float time = 15f;
    [SerializeField] TextAsset csvWordsFile;
    private string[] wordsWithGender;
    private int wordIndex;
    private float timer;
    private int score;
    private bool currentWordIsMasculine;
    private const char CSV_SEPERATOR = ',';
 
    void Awake()
    {
        feminineButton.onClick.AddListener(delegate {ClickGender(false);});
        masculineButton.onClick.AddListener(delegate {ClickGender(true);});
        wordsWithGender = GetDataFromCSV();
        LoadNextWord();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            LoadNextWord();
            timer = 0;
        }
        
    }
    private string[] GetDataFromCSV()
    {
        var text = csvWordsFile.text;
        var lines = text.Split(Environment.NewLine);
        return lines;
    }
    private void LoadNextWord()
    {
        wordIndex = GetIncrementedIndexThatIsSaveForArray(wordIndex,wordsWithGender.Length);
        string[] wordsWithGenderSplit =  wordsWithGender[wordIndex].Split(CSV_SEPERATOR);
        string word = wordsWithGenderSplit[0];
        string gender = wordsWithGenderSplit[1];
        wordText.text = word;
        currentWordIsMasculine = gender.Equals("m");
        Debug.Log($"'{word}' is '{gender}'");
    }
    public static int GetIncrementedIndexThatIsSaveForArray(int value, int arrayLength)
    {
        value++;
        if (value >= arrayLength)
        {
            value = 0;
        }
        else if (value < 0)
        {
            value = arrayLength-1;
        }
        return value;
    }
    private void ClickGender(bool selectedMasculine)
    {
        if(selectedMasculine == currentWordIsMasculine)
        {
            score++;
            scoreText.text = "Score: " + score; 
            timer = 0;
        }
        LoadNextWord();
    }
}

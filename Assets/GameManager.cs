using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI wordText;
    [SerializeField] TextMeshProUGUI scoreText, timerText, highscoreText;
    [SerializeField] Button feminineButton, masculineButton;
    public WordData[] wordsData;
    
    [Header("Varibales")]
    [SerializeField] float maximumTime = 15f;
    [Range(0, 3)] [SerializeField] private int timerDecimalPlaces = 1; 
    [SerializeField] private bool quitAfterTimerEnds = true; // else will reload scene
    [SerializeField] private bool randomWordOrder = true;
    
    private WordData[] _wordsData;
    private string[] wordsWithGender;
    private int wordIndex;
    private float timer;
    private int score;
    private bool currentWordIsMasculine;

    void Awake()
    {
        highscoreText.text = "Highscore: " + GetHighscore();
        feminineButton.onClick.AddListener(delegate { ClickGender(false); });
        masculineButton.onClick.AddListener(delegate { ClickGender(true); });
        if (wordsData.Length == 0) // if didn't
        {
            Debug.LogError(
                "You fergot to pre-parse the csv so it will be loaded right now. (Go to the header in unity and click on CsvParser/Generate Word Objects. Then pull the Words from ) ");
            Quit();
            return;
        }
        else
        {
            if (randomWordOrder)
            {
                _wordsData = wordsData.OrderBy(c => Random.Range(0f, 1f)).ToArray();
            }
            else
            {
                _wordsData = wordsData;
            }
        }

        LoadNextWord();
        UpdateTimer();
    }

    private async void UpdateTimer()
    {
        int deltaTimeMilliSecounds = 10;
        while (timer < maximumTime)
        {
            timer += (deltaTimeMilliSecounds / 1000f);
            timerText.text = timer.ToString("F" + timerDecimalPlaces);
            await Task.Delay(deltaTimeMilliSecounds);
        }

        timer = 0;
        UpdateHighscoreIfNewScoreIsBetter();

        if (quitAfterTimerEnds)
        {
            Quit();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload this scene
        }
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void OnDisable()
    {
        timer = maximumTime;
    }

    private void UpdateHighscoreIfNewScoreIsBetter()
    {
        int oldHighscore = GetHighscore();
        if (score > oldHighscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    private int GetHighscore()
    {
        return PlayerPrefs.GetInt("Highscore", 0);
    }

    private void LoadNextWord()
    {
        wordIndex = GetIncrementedIndexThatIsSaveForArray(wordIndex, _wordsData.Length);
        WordData wordData = _wordsData[wordIndex];
        wordText.text = wordData.word;
        currentWordIsMasculine = wordData.isMasculine;
        Debug.Log(wordData);
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
            value = arrayLength - 1;
        }

        return value;
    }

    private void ClickGender(bool selectedMasculine)
    {
        if (selectedMasculine == currentWordIsMasculine)
        {
            score++;
            scoreText.text = "Score: " + score;
        }

        LoadNextWord();
    }
}
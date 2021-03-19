using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyPress : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] keys = new GameObject[26];
    
    private Button[] keysButtons = new Button[26];

    private WordsGenerator _wordsGenerator = new WordsGenerator();

    public TextMeshProUGUI currentWord;
    public TextMeshProUGUI nextWord;
    public TextMeshProUGUI nextNextWord;

    public TextMeshProUGUI score;
    public TextMeshProUGUI multiplier;

    public TextMeshProUGUI resume;

    public AudioSource errorSound;

    public GameObject pauseMenu;

    public Slider timeSlider;

    private Word _currentWord;
    private Word _nextWord;
    private Word _nextNextWord;

    private float _timeLeft;

    private bool _gamePaused;
    private bool _gameOver;

    private int _score;
    private int _multiplier = 1;

    private bool _mistake;

    void Start()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < keys.Length; i++)
        {
            keysButtons[i] = keys[i].GetComponent<Button>();
        }

        _currentWord = _wordsGenerator.GetRandomWord();
        _nextWord = _wordsGenerator.GetRandomWord();
        _nextNextWord = _wordsGenerator.GetRandomWord();

        currentWord.text = _currentWord.word;
        nextWord.text = _nextWord.word;
        nextNextWord.text = _nextNextWord.word;
        CompleteWordSettings();
    }

    private void Update()
    {
        if (_gameOver)
            return;
        if (Input.GetKeyDown("escape"))
        {
            if (_gamePaused)
                Resume();
            else
                Pause();
        }
    }

    void LateUpdate()
    {
        if (_timeLeft < Time.time)
        {
            _gameOver = true;
            Pause();
            return;
        }

        timeSlider.value = _timeLeft - Time.time;
        for (int x = 97; x <= 122; x++)
        {
            if (Input.GetKeyDown(((char) x).ToString()))
            {
                keysButtons[x-97].Select();
                if (_currentWord.getChar() == (char) x)
                {
                    _currentWord.removeChar();
                }
                else
                {
                    errorSound.Play();
                    _multiplier = 1;
                    multiplier.text = _multiplier.ToString();
                    _mistake = true;
                }
                if (_currentWord.wordLeft.Count < 1)
                {
                    UpdateWords();
                }
                UpdateCurrentWord();
            }
        }
    }

    private void UpdateWords()
    {
        _currentWord = _nextWord;
        _nextWord = _nextNextWord;
        _nextNextWord = _wordsGenerator.GetRandomWord();

        AddScore();
        
        currentWord.text = _currentWord.word;
        nextWord.text = _nextWord.word;
        nextNextWord.text = _nextNextWord.word;

        if (!_mistake)
        {
            _multiplier++;
            multiplier.text = _multiplier.ToString();
        }

        _mistake = false;
        CompleteWordSettings();
    }

    private void UpdateCurrentWord()
    {
        currentWord.text = _currentWord.word;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        _gamePaused = true;
        pauseMenu.SetActive(true);
        if(_gameOver) resume.text = "Restart";
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        if (_gameOver)
        {
            _gamePaused = false;
            SceneManager.LoadScene("GamePlay");
            return;
        }
        _gamePaused = false;
        pauseMenu.SetActive(false);
    }

    private int calculateScore()
    {
        Debug.Log((_timeLeft - Time.time));
        var x =  (int) ((_timeLeft - Time.time) * 100 * _multiplier);
        Debug.Log(x);
        return x;
    }

    private void CompleteWordSettings()
    {
        _timeLeft = Time.time + _currentWord.maxTime;
        timeSlider.maxValue = _currentWord.maxTime;
    }

    private void AddScore()
    {
        _score += calculateScore();
        score.text = _score.ToString();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

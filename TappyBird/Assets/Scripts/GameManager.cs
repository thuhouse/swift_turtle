using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate OnEatMedium;
    public static event GameDelegate OnEatHeavy;
    public static GameManager Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public TextMeshProUGUI scoreText;
    [SerializeField]
    private AudioSource backgroundAudio;
    [SerializeField]
    private AudioSource goldenAudio;

    public AudioSource currentAudio;

    enum PageState {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    public int Score { get {return score;}}

    int storedScore = 0;
    public int StoredScore {get {return storedScore;}}
    private bool _gameOver = true;

    public bool GameOver {get { return _gameOver;}}

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
        TapController.OnReset += OnReset;
        PooController.OnPoo += OnPoo;
        currentAudio = backgroundAudio;
        currentAudio.Play();
    }

    private void OnDisable() {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    private void Start() {
        ConfirmGameOver();
    }

    private void OnPlayerScored()
    {
        if (storedScore >= 10) return;
        storedScore++;        
        if (storedScore == 3) OnEatMedium();
        if (storedScore == 8) OnEatHeavy();
    }

    private void OnPoo(){

        if (storedScore < 8){
            if (currentAudio != backgroundAudio){
                currentAudio.Stop();
                currentAudio = backgroundAudio;
                currentAudio.Play();
            }
            
        } else {
            if (currentAudio != goldenAudio){
                currentAudio.Stop();
                currentAudio = goldenAudio;
                currentAudio.Play();
            }
        }


        score += CalculateScore(storedScore);
        scoreText.text = score.ToString();
        storedScore = 0;
    }

    private int CalculateScore(int storedScore){
        int total = 0;
        for (int i = 1; i <= storedScore; i++){
            total += i;
        }
        return total;
    }

    private void OnPlayerDied()
    {
        _gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore){
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }

    private void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        _gameOver = false;
    }

    

    void SetPageState(PageState state){
        switch(state){
            case PageState.None:
            startPage.SetActive(false);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(false);
            break;

            case PageState.Start:
            startPage.SetActive(true);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(false);
            currentAudio.Stop();
            currentAudio = backgroundAudio;
            currentAudio.Play();
            break;

            case PageState.GameOver:
            startPage.SetActive(false);
            gameOverPage.SetActive(true);
            countdownPage.SetActive(false);
            break;

            case PageState.Countdown:
            startPage.SetActive(false);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(true);
            break;
        }
    }

    public void ConfirmGameOver(){
        //activated when replay button is hit
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame(){
        //activated when play button
        SetPageState(PageState.Countdown);
    }

    private void OnReset(){
        storedScore = 0;
        score = 0;
    }
}

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
    public static GameManager Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public TextMeshProUGUI scoreText;

    enum PageState {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    public int Score { get {return score;}}
    private bool _gameOver = true;

    public bool GameOver {get { return _gameOver;}}

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
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
        score++;
        scoreText.text = score.ToString();
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
}

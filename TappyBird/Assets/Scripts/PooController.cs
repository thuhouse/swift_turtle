using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooController : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnPoo;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    GameObject poo;
    [SerializeField]
    GameObject goldPoo;
    [SerializeField]
    GameObject bird;

    private bool disabled = true;

    private void Start() {
        GameManager.OnGameStarted += OnGameStarted;
        TapController.OnPlayerDied += OnPlayerDied;
    }

    private void Update() {
        if (disabled) return;
        if (Input.GetMouseButtonDown(1)){
            if (gameManager.StoredScore < 8)
                Poo();
            else
                GoldPoo();
        }    
    }

    void Poo(){
        GameObject myPoo = GameObject.Instantiate(poo);
        myPoo.transform.position = bird.transform.position;
        OnPoo();
    }

    void GoldPoo(){
        GameObject myPoo = GameObject.Instantiate(goldPoo);
        myPoo.transform.position = bird.transform.position;
        OnPoo();
    }

    void OnGameStarted(){
        disabled = false;
    }

    void OnPlayerDied(){
        disabled = true;
    }
}

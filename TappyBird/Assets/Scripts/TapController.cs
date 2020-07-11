using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public static event PlayerDelegate OnReset;
    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;
    Rigidbody2D _rigidbody;
    Quaternion _downRotation;
    Quaternion _forwardRotation;

    SpriteRenderer _spriteRenderer;
    [SerializeField]
    Sprite[] birds;
    [SerializeField]
    ChineseController chineseController;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _downRotation = Quaternion.Euler(0, 0, -90);
        _forwardRotation = Quaternion.Euler(0, 0, 20);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResetPosition();
        //rigidbody.simulated = false;
    }

    private void OnEnable() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnEatMedium += OnEatMedium;
        GameManager.OnEatHeavy += OnEatHeavy;
        PooController.OnPoo += OnPoo;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    private void OnGameOverConfirmed()
    {
        ResetPosition();
    }

    private void OnGameStarted()
    {
       _rigidbody.velocity = Vector3.zero;
        _rigidbody.simulated = true;
    }

    private void Update() {
        if (GameManager.Instance.GameOver) return;

        if (Input.GetMouseButtonDown(0)){
            transform.rotation = _forwardRotation;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapAudio.Play();
        }

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        transform.rotation = Quaternion.Lerp(transform.rotation, _downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ScoreZone"){
            //register score
            OnPlayerScored();
            EatFood();
            chineseController.GetRandomAudioSource().Play();
            other.transform.parent.GetComponent<Pipe>().ClearSprite();
        }

        if (other.gameObject.tag == "DeadZone"){
            _rigidbody.simulated = false;
            //register dead event
            OnPlayerDied();
            dieAudio.Play();
        }
    }


    public void ResetPosition() {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
        _rigidbody.simulated = false;
        Reset();
    }

    private void Reset(){
        _spriteRenderer.sprite = birds[0];
        _rigidbody.mass = 1;
        transform.localScale = new Vector3(0.08f, 0.08f, 1);
        OnReset();
    }

    private void EatFood(){
        Vector3 newScale = new Vector3(transform.localScale.x + 0.005f, transform.localScale.y + 0.005f, transform.localScale.z);
        transform.localScale = newScale;
    }

    private void OnEatMedium(){
        _rigidbody.mass = 1f;
        _spriteRenderer.sprite = birds[1];
    }

    private void OnEatHeavy(){
        _rigidbody.mass = 3.5f;
        _spriteRenderer.sprite = birds[2];
    }

    private void OnPoo(){
        _spriteRenderer.sprite = birds[0];
        _rigidbody.mass = 1;
        transform.localScale = new Vector3(0.08f, 0.08f, 1);
    }

}

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
    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;
    Rigidbody2D _rigidbody;
    Quaternion _downRotation;
    Quaternion _forwardRotation;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _downRotation = Quaternion.Euler(0, 0, -90);
        _forwardRotation = Quaternion.Euler(0, 0, 20);
        ResetPosition();
        //rigidbody.simulated = false;
    }

    private void OnEnable() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    private void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
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

        transform.rotation = Quaternion.Lerp(transform.rotation, _downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ScoreZone"){
            //register score
            OnPlayerScored();
            scoreAudio.Play();
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
    }

}

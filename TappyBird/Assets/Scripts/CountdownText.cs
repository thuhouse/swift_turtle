using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CountdownText : MonoBehaviour
{
    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;
    private TextMeshProUGUI _countdownText;

    private void OnEnable() {
        _countdownText = GetComponent<TextMeshProUGUI>();
        _countdownText.text = "3";
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown() {
        int count = 3;
        for (int i = 0; i < count; i++){
            _countdownText.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }

        OnCountdownFinished();
    }
}

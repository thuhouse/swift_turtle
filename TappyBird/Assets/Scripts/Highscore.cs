using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Highscore : MonoBehaviour
{
   TextMeshProUGUI highscore;

   private void OnEnable() {
       highscore = GetComponent<TextMeshProUGUI>();
       highscore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
   }
}

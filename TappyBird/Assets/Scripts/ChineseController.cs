using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChineseController : MonoBehaviour
{
    [SerializeField]
    AudioSource[] chineseSounds;


    public AudioSource GetRandomAudioSource(){
        int index = Random.Range(0, chineseSounds.Length);
        return chineseSounds[index];
    }
}

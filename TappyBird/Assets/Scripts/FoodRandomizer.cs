using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodRandomizer : MonoBehaviour
{
    [SerializeField]
    public static Sprite[] foods;


    static Sprite FoodRandom() {
        System.Random randoGenerate = new System.Random();
         return foods[randoGenerate.Next(0, 20)];
    }
}
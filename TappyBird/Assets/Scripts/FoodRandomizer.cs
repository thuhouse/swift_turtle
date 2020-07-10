using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRandomizer : MonoBehaviour
{
    [SerializeField]
    public GameObject[] foods;



        public GameObject FoodRandom() {
        int index = Random.Range(0, foods.Length - 1);
        //System.Random randoGenerate = new System.Random();
        //int index = randoGenerate.Next(0, 19);
        Debug.Log(index);
         return foods[index];
    }
    

    private void OnEnable() {
        Debug.Log("hello");
        FoodRandom().SetActive(true);
    }

    //private void Update() {
        //FoodRandom();
    //}
}
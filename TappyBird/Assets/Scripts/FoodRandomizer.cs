using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRandomizer : MonoBehaviour
{
    [SerializeField]
    Sprite[] _sprites;

    public Sprite FoodRandom() {
        int index = Random.Range(0, _sprites.Length - 1);
        return _sprites[index];
    }

    private void PickFood(Pipe pipe){
        pipe.SetSprite(FoodRandom());
    }

    private void OnEnable() {
        Parallaxer.OnPipePlace += PickFood;
    }

    private void OnDisable() {
        Parallaxer.OnPipePlace -= PickFood;
    }
}
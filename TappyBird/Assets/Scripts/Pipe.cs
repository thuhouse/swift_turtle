using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite){
        spriteRenderer.sprite = sprite;
    }

    public void ClearSprite(){
        spriteRenderer.sprite = null;
    }
}

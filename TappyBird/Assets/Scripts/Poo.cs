using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poo : MonoBehaviour
{
    public float shiftSpeed;
    
    // Update is called once per frame
    void Update()
    {
        Shift();
    }

    void Shift() {
        gameObject.transform.position += (-Vector3.right * shiftSpeed + Vector3.down * 7f) * Time.deltaTime;
    }

    private void OnEnable() {
        StartCoroutine("Destroy");
    }

    IEnumerator Destroy(){
        yield return new WaitForSeconds(2);
        GameObject.Destroy(this.gameObject);
    }
}

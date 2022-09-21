using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    float velocity = 3;
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);//hace que el zombie camine
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "palotransparente"){
            //Debug.Log("entro");
            velocity *= -1;
            if(velocity>0)
                sr.flipX = false;
            else   
                sr.flipX = true;
        }
    }

}

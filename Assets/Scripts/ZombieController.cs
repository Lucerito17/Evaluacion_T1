using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    /*int vida=3;
    public int Vida(){
        return vida;
    }

    public void PerderVida(int temp){
        vida-=temp;
    }*/

    /*private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Bala")
            PerderVida(1);
        if(other.gameObject.tag=="BalaMed")
            PerderVida(2);
        if(other.gameObject.tag=="BalaGra")
            PerderVida(3);

        if(vida<=0)
            Destroy(this.gameObject);}*/
    

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

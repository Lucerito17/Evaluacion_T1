using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    int vida=2;
    

    float velocity = 3;
    Rigidbody2D rb;
    SpriteRenderer sr;
    public NinjaManagerController gameManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        rb.velocity = new Vector2(-velocity, rb.velocity.y);//hace que el zombie camine
        if(vida<=0){
            Destroy(this.gameObject);
            gameManager.GanarPuntos(1);
        }
            
        GirarAnimacion();
    }

    public int Vida(){
        return vida;
    }

    public void PerderVida(int temp){
        vida-=temp;
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
        
        if(other.gameObject.tag == "pain"){
            Destroy(this.gameObject);
            gameManager.GanarPuntos(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Bala")
            PerderVida(1);
    }

    private void GirarAnimacion()
    {
        if(rb.velocity.x < 0)
        {
            sr.flipX = true;
        }
        else if(rb.velocity.x > 0)
        {
            sr.flipX = false;
        }
    }

}

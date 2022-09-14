using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NinjaController : MonoBehaviour
{
    public float velocity = 3;
    public float jumpVelocity = 8;
    bool morir = false;
    public GameObject Balas;
    public GameManagerController gameManager;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    Vector3 lastCheckpointPosition;
    

    const int ANIMATION_CORRER = 3;
    const int ANIMATION_QUIETO = 0;
    const int ANIMATION_MUERTO = 1;

    void Start()
    {
        Debug.Log("Iniciando Juego");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(morir == false){
            Disparar();
            Saltar();
            Correr();
            GirarAnimacion();
            CheckGround();
        }
        else Morir(); 
    }

    private void Morir()
    {
        if(morir == true && gameManager.vida>0){
            ChangeAnimation(ANIMATION_MUERTO);
            gameManager.PerderVida();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Correr()
    {
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);

    }
    private void Saltar()
    {
        animator.SetFloat("jumpVelocity", rb.velocity.y);
        if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
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
    private void ChangeAnimation(int animation)
    {
        animator.SetInteger("Estado", animation);
    }

    private void CheckGround()
    {
        if(cl.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("isGround", true);
        }
        else
        {
            animator.SetBool("isGround", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Moriste");
            rb.velocity=new Vector2(0,rb.velocity.y);
            morir = true;
        }

        if(other.gameObject.name =="DarkHole")
        {
            if(lastCheckpointPosition != null)
            {
                transform.position = lastCheckpointPosition;
            }
        }  
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        lastCheckpointPosition = transform.position;
        
    }

    private void Disparar()
    {
        if(Input.GetKeyDown(KeyCode.X)&&gameManager.Balas()>0)
        {
            if(sr.flipX==true){//disparar hacia la izquierda
                var BalasPosition = transform.position + new Vector3(-3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetLeftDirection();
                gameManager.PerderBalas();
            }

            if(sr.flipX==false){//disparar hacia la derecha
                var BalasPosition = transform.position + new Vector3(3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetRightDirection();
                gameManager.PerderBalas();
            }
        }
    }
}
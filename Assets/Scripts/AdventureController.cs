using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureController : MonoBehaviour
{
    public float velocity = 10;
    public float VelocityJump = 25;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    Vector3 lastCheckpointPosition;
    Vector3 temp;

    const int ANIMATION_CORRER = 1;
    const int ANIMATION_QUIETO = 0;
    int cont;

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
        rb.velocity=new Vector2(0,rb.velocity.y);
        Correr();
        Saltar();
        GirarAnimacion();
        CheckGround();
    }

    private void Correr()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANIMATION_QUIETO);
        }
    }
    private void Saltar()
    {
        animator.SetFloat("VelocityJump", rb.velocity.y);
        //if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return;}
        if (Input.GetKeyDown(KeyCode.Space)&& cont!=1)
        {
            rb.velocity = new Vector2(rb.velocity.x, VelocityJump);
            cont++;
            Debug.Log(cont);
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
            cont = 0;
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
        }

        if(other.gameObject.name =="DarkHole")
        {
            if(lastCheckpointPosition != null)
            {
                transform.position = lastCheckpointPosition;
            }
            if(temp != null)
            {
                transform.position = temp;
            }
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        lastCheckpointPosition = transform.position;
        if(other.gameObject.name == "Sign")
        {
            temp = transform.position;
        }
        
    }
}
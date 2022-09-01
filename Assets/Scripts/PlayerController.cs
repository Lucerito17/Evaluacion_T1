using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float veloci = 6;
    public float velocity = 10;
    public float velocityJump = 8;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;

    const int ANIMATION_CORRER = 1;
    const int ANIMATION_QUIETO = 0;
    const int ANIMATION_CAMINAR = 2;
    const int ANIMATION_ATACAR = 3;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Iniciando Juego");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity=new Vector2(0,rb.velocity.y);
        Caminar();
        Correr();
        Saltar();
        Atacar();
        GirarAnimacion();
        CheckGround();
    }

    private void Caminar()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            rb.velocity = new Vector2(veloci, rb.velocity.y);
            ChangeAnimation(ANIMATION_CAMINAR);
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            rb.velocity = new Vector2(-veloci, rb.velocity.y);
            ChangeAnimation(ANIMATION_CAMINAR);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANIMATION_QUIETO);
        }
    }
    private void Correr()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.X))
        {
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.X))
        {
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
    }
    private void Saltar()
    {
        animator.SetFloat("jumpVelocity", rb.velocity.y);
        if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, velocityJump);
        }
    }
    private void Atacar()
    {
        if (Input.GetKey(KeyCode.Z)){
            ChangeAnimation(ANIMATION_ATACAR);
        }
        else if (Input.GetKeyUp(KeyCode.Z)){
            ChangeAnimation(ANIMATION_QUIETO);
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
    private void CheckGround()//para sentir el piso
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
}
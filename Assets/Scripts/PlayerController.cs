using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float veloci = 6;
    public float velocity = 10;
    public float velocityJump = 12;
    public GameObject Bullet;
    public GameManagerController gameManager;

    public AudioClip jumpClip;
    public AudioClip monedaClip;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity=new Vector2(0,rb.velocity.y);
        Correr();
        Saltar();
        //Atacar();
        Disparar();
        GirarAnimacion();
        CheckGround();
    }

    private void Correr()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            rb.velocity = new Vector2(veloci, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            rb.velocity = new Vector2(-veloci, rb.velocity.y);
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
        animator.SetFloat("jumpVelocity", rb.velocity.y);
        if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        if (Input.GetKey(KeyCode.Space))
        {
            audioSource.PlayOneShot(jumpClip);
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

    private void Disparar()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(sr.flipX==true){//disparar hacia la izquierda
                var BulletPosition = transform.position + new Vector3(-3,0,0);
                var gb = Instantiate(Bullet, BulletPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetLeftDirection();   
            }
            if(sr.flipX==false){//disparar hacia la derecha
                var BulletPosition = transform.position + new Vector3(3,0,0);
                var gb = Instantiate(Bullet, BulletPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetRightDirection();
            }
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

    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy"){//cuando colisiona con el enemigo y pierde una vida
            Debug.Log("Perdiste una vida");
            gameManager.PerderVida();
        }
        if (other.gameObject.tag == "CoinB"){//cuando colisiona con el hongo y crece
            Debug.Log("MonedaB");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
            gameManager.GanarMonedasB(10);
        }
        if (other.gameObject.tag == "CoinG"){//cuando colisiona con el hongo y crece
            Debug.Log("MonedaG");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
           // gameManager.GanarMonedasG(10);
        }
        if (other.gameObject.tag == "CoinS"){//cuando colisiona con el hongo y crece
            Debug.Log("MonedaS");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
           // gameManager.GanarMonedasS(10);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float veloci = 6;
    public float velocity = 10;
    public float velocityJump = 40;
    public GameObject Bullet;
    public GameManagerController gameManager;
    private bool guardado = false;

    public AudioClip jumpClip;
    public AudioClip monedaClip;
    Vector3 temp;
    Vector3 lastCheckpointPosition;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    AudioSource audioSource;

    const int ANIMATION_CORRER = 1;
    const int ANIMATION_QUIETO = 0;
    const int ANIMATION_CAMINAR = 2;
    const int ANIMATION_ATACAR = 3;
    const int ANIMATION_MORIR = 4;

    void Start()
    {
        Debug.Log("Iniciando Juego");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //para guardar la posicion luego de volver a ejecutar 46-50
        if(guardado == false && gameManager.tempx != 0 && gameManager.tempy != 0)//volver a descomentar
            {
                transform.position = new Vector2(gameManager.tempx, gameManager.tempy);
                guardado = true;
            }
            
        if(gameManager.vida>0)
        {
            rb.velocity=new Vector2(0,rb.velocity.y);
            Correr();
            Saltar();
            //Atacar();
            Disparar();
            GirarAnimacion();
            CheckGround();
        }
        else 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
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

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name =="checkpoint"){
            Debug.Log("Checkpoint");
            gameManager.SaveGame();
        }
        if (other.gameObject.tag == "CoinB"){//cuando colisiona con la moneda bronce
            Debug.Log("MonedaB");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
            gameManager.GanarMonedasB(10);
        }
        if (other.gameObject.tag == "CoinG"){//cuando colisiona con la moneda golden
            Debug.Log("MonedaG");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
            gameManager.GanarMonedasG(20);
        }
        if (other.gameObject.tag == "CoinS"){//cuando colisiona con la moneda silver
            Debug.Log("MonedaS");
            Destroy(other.gameObject);
            audioSource.PlayOneShot(monedaClip);
            gameManager.GanarMonedasS(30);
        }

        lastCheckpointPosition = transform.position;
        if(other.gameObject.name == "checkpoint")
        {
            var x = transform.position.x;
            var y = transform.position.y;
            temp = transform.position;
            gameManager.GuardarPartida(x,y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy"  && gameManager.vida >=0){//cuando colisiona con el enemigo y pierde una vida
            Debug.Log("Perdiste una vida");
            gameManager.PerderVida();
            if(gameManager.vida == 0) 
                ChangeAnimation(ANIMATION_MORIR);
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
}
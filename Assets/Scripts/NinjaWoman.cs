using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NinjaWoman : MonoBehaviour
{
    public float velocity = 5;
    private float vl = 0;
    public float jumpVelocity = 8;
    public float velocityClimb = 4;
    bool morir = false;
    public GameObject Balas;
    private bool atacar = true;
    public GameManagerController gameManager;
    private float gravedadInicial;
    private bool escalar;
    private Vector2 input;
    public SpriteRenderer srCharacter;
    public Sprite[] sprites;
    private int next = 1;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    BoxCollider2D bc;
    Vector3 lastCheckpointPosition;
    

    const int ANIMATION_CORRER = 3;
    const int ANIMATION_QUIETO = 0;
    const int ANIMATION_MUERTO = 1;
    const int ANIMATION_DISPARAR = 2;
    const int CARGAR = 4;
    const int ANIMATION_PLANEAR = 5;
    const int ANIMATION_ATACAR = 6;

    void Start()
    {
        Debug.Log("Iniciando Juego");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        gravedadInicial = rb.gravityScale;
        cl = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(morir == false){
            rb.velocity = new Vector2(vl, rb.velocity.y);
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            Disparar();
            Saltar();
            Correr();
            Climb();
            Planear();
            CambioArma();
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
        if (Input.GetKey(KeyCode.RightArrow)){
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANIMATION_QUIETO);
        }
        //Regresar (Q)
        if (Input.GetKey(KeyCode.LeftArrow)){
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            ChangeAnimation(ANIMATION_CORRER);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANIMATION_QUIETO);
        }

    }

    public void CambioArma()
    {
        srCharacter.sprite = sprites[next];
        next++;
        if(next == sprites.Length )
            next = 0;
        if(atacar==false)
            atacar=true;
        else
            atacar =false;
    }

    public void botonataque(){
        if(atacar == true)
        {
            Disparo();
        }else if(atacar == false)
        {
            Atacar();
        }
    }

    public void WalkToLeft()
    {
        vl = -velocity;
        ChangeAnimation(ANIMATION_CORRER);
    }

    public void WalkToRight()
    {
        vl = velocity;
        ChangeAnimation(ANIMATION_CORRER);
    }

    public void StopWalk()
    {
        vl = 0;
        ChangeAnimation(ANIMATION_QUIETO);
    }

    public void SaltoBoton()
    {
        animator.SetFloat("jumpVelocity", rb.velocity.y);
        if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }
    
    public void Saltar()
    {
        animator.SetFloat("jumpVelocity", rb.velocity.y);
        if(!cl.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    public void Planear()
    {
        if(rb.velocity.y<0 && Input.GetKey(KeyCode.Q))
        {
            ChangeAnimation(ANIMATION_PLANEAR);
            rb.velocity += Vector2.up*Physics2D.gravity.y*(-0.9f)*Time.deltaTime;
        }
    }

    public void Climb()
    {
        animator.SetBool("isClimb", escalar);//cambia la animación
        if(!bc.IsTouchingLayers(LayerMask.GetMask("Ground"))){escalar = false;}
        //si se ejecuta este if es porque es falso(esta en el piso) y saldra del metodo trepar (climb) 
        if((input.y !=0 || escalar) && (bc.IsTouchingLayers(LayerMask.GetMask("ladders")))){
            //velocidad escalar es true
            Vector2 velocidadSubida = new Vector2(rb.velocity.x, input.y * velocityClimb);
            rb.velocity = velocidadSubida;
            rb.gravityScale = 0;
            escalar = true;
        }
        else
        {
            rb.gravityScale = gravedadInicial;
            escalar = false;
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
        if(other.gameObject.tag=="Enemy"&&this.gameObject) 
        {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        lastCheckpointPosition = transform.position;
        
    }

    private void Disparar()
    {
        if(Input.GetKeyUp(KeyCode.X)&&gameManager.Balas()>0)
        {
            if(sr.flipX==true){//disparar hacia la izquierda   
                var BalasPosition = transform.position + new Vector3(-3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetLeftDirection();
                //gameManager.PerderBalas();
            }

            if(sr.flipX==false){//disparar hacia la derecha
                var BalasPosition = transform.position + new Vector3(3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetRightDirection();
                //gameManager.PerderBalas();
            }
        }        
    }

    private void Disparo()
    {
            if(sr.flipX==true){//disparar hacia la izquierda   
                var BalasPosition = transform.position + new Vector3(-3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetLeftDirection();
                //gameManager.PerderBalas();
            }

            if(sr.flipX==false){//disparar hacia la derecha
                var BalasPosition = transform.position + new Vector3(3,0,0);
                var gb = Instantiate(Balas, BalasPosition, Quaternion.identity) as GameObject;
                //llamar bala, posicion bala , direcion bala
                var controller = gb.GetComponent<Bullet>();
                controller.SetRightDirection();
                //gameManager.PerderBalas();
            }
     
    }
    private void Atacar()
    {
        if(Input.GetKey(KeyCode.X)){
            ChangeAnimation(ANIMATION_ATACAR);
        }
    }
}
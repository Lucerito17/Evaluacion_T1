using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Bullet : MonoBehaviour
{
    public float velocity = 20;
    Rigidbody2D rb;
    float realVelocity;
    public int cont = 0;
    private GameManagerController gameManager;

    public void SetRightDirection()
    {
        realVelocity = velocity;
    }

    public void SetLeftDirection()
    {
        realVelocity = -velocity;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManagerController>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 3);//eliminacion del objeto
    }


    void Update()
    {
        rb.velocity = new Vector2(realVelocity, 0); 
    }

    public void OnCollisionEnter2D(Collision2D other)//para chocar y eliminar
    {   
        Destroy(this.gameObject);//se topa con el objeto 
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);//destruye al objeto topado
            //gameManager.GanarPuntos(10);
        }  
    }
}
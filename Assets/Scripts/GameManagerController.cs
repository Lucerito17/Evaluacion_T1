using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManagerController : MonoBehaviour
{
    public TMP_Text puntosTexto;
    public TMP_Text balasTexto;
    public TMP_Text vidas;

    private int score;
    private int balas;
    public int vida;

    void Start()
    {
        score = 0;
        balas = 5;
        vida = 1;
        PrintScoreInScreen();
        PrintBulletInScreen();
        PrintLifeInScreen();
    }

    public int Puntos()
    {
        return score;
    }

    public int Balas()
    {
        return balas;
    }

    public int Vidas()
    {
        return vida;
    }

    public void GanarPuntos(int punto)
    {
        score += punto;
        PrintScoreInScreen();
    }

    public void PerderBalas()
    {
        balas -= 1;
        PrintBulletInScreen();
    }

    public void PerderVida()
    {
        vida -= 1;
        PrintLifeInScreen();
    }
    
    private void PrintScoreInScreen()
    {
        puntosTexto.text = "Puntaje: " + score;
    }
    
    private void PrintBulletInScreen()
    {
        balasTexto.text = "Balas: " + balas;
    }

    private void PrintLifeInScreen()
    {
        vidas.text = "Vida: " + vida;
    }
}

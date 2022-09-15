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
    public TMP_Text monedaB;
    public TMP_Text monedaG;
    public TMP_Text monedaS;

    private int score;
    private int balas;
    public int vida;
    public int coinB;
    public int coinG;
    public int coinS;

    void Start()
    {
        score = 0;
        balas = 5;
        vida = 1;
        coinB = 0;
        coinG = 0;
        coinS = 0;
        PrintScoreInScreen();
        PrintBulletInScreen();
        PrintLifeInScreen();
        PrintCoinB();
       //PrintCoinG();
        //PrintCoinS();
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

    public int MonedaB()
    {
        return coinB;
    }

    public int MonedaG()
    {
        return coinG;
    }

    public int MonedaS()
    {
        return coinS;
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

    public void GanarMonedasB(int punto)
    {
        coinB += punto;
        PrintCoinB();
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

    private void PrintCoinB()
    {
        monedaB.text = "MonedaB: " + coinB;
    }
}

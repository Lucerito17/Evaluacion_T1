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

    private int score;
    private int balas;

    void Start()
    {
        score = 0;
        balas = 5;
        PrintScoreInScreen();
        PrintBulletInScreen();
    }

    public int Puntos()
    {
        return score;
    }

    public int Balas()
    {
        return balas;
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
    
    private void PrintScoreInScreen()
    {
        puntosTexto.text = "Puntaje: " + score;
    }
    
    private void PrintBulletInScreen()
    {
        balasTexto.text = "Balas: " + balas;
    }
}

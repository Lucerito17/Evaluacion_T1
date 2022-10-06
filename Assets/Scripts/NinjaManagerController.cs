using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class NinjaManagerController : MonoBehaviour
{
    public TMP_Text puntosTexto;
    //public TMP_Text balasTexto;
    public TMP_Text vidas;
    public TMP_Text monedaB;

    private int score;
    private int balas;
    public int vida;
    public int coinB;
    public float tempx;
    public float tempy;

    void Start()
    {
        score = 0;
        balas = 5;
        vida = 1;
        coinB = 0;
        tempx = 0;
        tempy = 0;
        PrintScoreInScreen();
        //PrintBulletInScreen();
        PrintLifeInScreen();
        PrintCoinB();
        //LoadGame();
    }

    public void SaveGame()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        if(File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = score;
        data.CoinsB = coinB;
        data.ax = tempx;
        data.ay = tempy;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        if(File.Exists(filePath))
            file = File.OpenRead(filePath);
        else{
            Debug.LogError("No see encontro archivo");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        //usar datos guardados
        score = data.Score;
        coinB = data.CoinsB;
        tempx = data.ax;
        tempy = data.ay;

        PrintScoreInScreen();
        PrintCoinB();
    }

    public float Tempx()
    {
        return tempx;
    }

    public float Tempy()
    {
        return tempy;
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

    public void GuardarPartida(float x, float y)
    {
        tempx = x;
        tempy = y;
    }

    public void GanarPuntos(int punto)
    {
        score += punto;
        PrintScoreInScreen();
    }

    public void PerderBalas()
    {
        balas -= 1;
        //PrintBulletInScreen();
    }

    public void Balas3(int bala)
    {
        balas += bala;
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
    
    /*private void PrintBulletInScreen()
    {
        balasTexto.text = "Balas: " + balas;
    }*/

    private void PrintLifeInScreen()
    {
        vidas.text = "Vida: " + vida;
    }

    private void PrintCoinB()
    {
        monedaB.text = "Monedas: " + coinB;
    }
}
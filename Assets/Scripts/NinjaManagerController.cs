using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

public class NinjaManagerController : MonoBehaviour
{
    public TMP_Text puntosTexto;
    //public TMP_Text balasTexto;
    public TMP_Text vidas;
    public TMP_Text monedaB;
    public GameObject Enemy;
    public TMP_Text zombiesCantidad;

    private int zombie;
    private int score;
    private int balas;
    public int vida;
    public int coinB;
    public float tempx;
    public float tempy;
    private bool temp1 = false;
    private bool temp2 = false;
    Stopwatch stopwatch = new Stopwatch();

    void Start()
    {
        stopwatch.Start();
        score = 0;
        zombie = 0;
        balas = 5;
        vida = 1;
        coinB = 0;
        tempx = 0;
        tempy = 0;
        PrintScoreInScreen();
        //PrintBulletInScreen();
        PrintZombieInScreen();
        PrintLifeInScreen();
        PrintCoinB();
        LoadGame();
    }

    void Update()
    {
        TimeSpan timestop = stopwatch.Elapsed;
        if(timestop.TotalSeconds >=1 && timestop.TotalSeconds < 1.3 && temp1 == false )
        {
            aleatorio();
            temp1 = true;
        }
        if(timestop.TotalSeconds >=2 && timestop.TotalSeconds < 2.3 && temp2 == false )
        {
            aleatorio();
            temp2 = true;
        }
        if(timestop.TotalSeconds >= 3)
        {
            aleatorio();
            stopwatch.Reset();
            stopwatch.Start();
            temp1 = false;
            temp2 = false;
        }
    }

    public void aleatorio()
    {
        UnityEngine.Debug.Log("funciona");
        System.Random r = new System.Random();
        int value = r.Next(0, 101);
        if(value < 26)
        {
            var gb = Instantiate(Enemy, new Vector2(-17, -8), Quaternion.identity) as GameObject;
            var gba = Instantiate(Enemy, new Vector2(0, -2), Quaternion.identity) as GameObject;
            var gbb = Instantiate(Enemy, new Vector2(44, -7), Quaternion.identity) as GameObject;
            stopwatch.Reset();
            stopwatch.Start();
        }
    }

    public void SaveGame()
    {
        var filePath = Application.persistentDataPath + "/guardarnuevo1.dat";
        FileStream file;

        if(File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = score;
        data.CoinsB = coinB;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        var filePath = Application.persistentDataPath + "/guardarnuevo1.dat";
        FileStream file;

        if(File.Exists(filePath))
            file = File.OpenRead(filePath);
        else{
            UnityEngine.Debug.LogError("No se encontro archivo");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        //usar datos guardados
        score = data.Score;
        coinB = data.CoinsB;

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

    public int ZombieCant()
    {
        return zombie;
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

    public void CantidadZombies(int zombiess)
    {
        UnityEngine.Debug.Log("LLega aqui");
        zombie += zombiess;
        PrintZombieInScreen();
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

    private void PrintZombieInScreen()
    {
        zombiesCantidad.text = "Zombies Muertos: " + zombie;
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
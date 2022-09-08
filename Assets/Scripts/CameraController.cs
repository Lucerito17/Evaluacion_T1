using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;//se necesita la posicion del jugador y esto me indica
    public Vector3 offset;//sirve para sumar los vectores de la posicion del player

    [Range(1,10)]
    public float smootherFactor;

    void Update()
    {
        var targetPosition = target.position + offset;
        //para que la camara se mueva unos milisegundos despues del player
        var smootherPosition = Vector3.Lerp(transform.position, targetPosition, smootherFactor * Time.fixedDeltaTime);
        transform.position = smootherPosition;
    }
}
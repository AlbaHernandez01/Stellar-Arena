using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    void Update()
    {
        //Le he aplicado un LookAt al canvas del enemigo (en el que se ve su barra de salud) y le he indicado que mire a la cámara mientras el enemigo se mueve.
        transform.LookAt(Camera.main.transform.position);
    }
}

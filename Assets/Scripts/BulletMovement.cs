using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [Header("Movement")]
    //He creado una variable de tipo float para indicarle la velocidad del movimiento.
    [SerializeField]
    private float speed = 100f;
    private void Awake()
    {
        //Le he indicado en el Awake que, automáticamente después de que aparezca la bala desde el empty object que le he asignado, espere 5 segundos y se destruya.
        Destroy(gameObject, 5.0f);
    }
    private void Update()
    {
        //Se llama a la función "Movement()" en el Update para que se ejecute correctamente cuando la inicialicemos.
        Movement();
    }

    //FUNCIÓN DE MOVIMIENTO DE LAS BALAS
    private void Movement()
    {
        //He creado una función para el movimiento de la bala. En ella, le indico que cuando aparezca se dirija hacia delante a la velocidad de movimiento asignada
        //y le he aplicado un Time.deltaTime para que tenga fluidez el movimiento.
        transform.Translate(Vector3.forward * speed * Time.deltaTime); 
    }

}

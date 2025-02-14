using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //He convertido este script en singleton para poder acceder a él mediante otros scripts.
    public static SpawnManager Instance { get; private set; }

    //He creado una variable de tipo GameObject en la que se almacena el personaje enemigo.
    [SerializeField]
    private GameObject enemy;

    //He creado una variable de tipo Transform que, a su vez, es un array que almacena diferentes posiciones en las que irán spawneando los enemigos.
    [SerializeField]
    private Transform[] posRotEnemy;

    //He creado, además, una varible tipo float que almacena el tiempo que habrá entre el spawn de un enemigo y otro.
    [SerializeField]
    private float timeBetweenEnemies = 5;
    private void Start()
    {
        //Le indico a "Instance" que cuando empiece a ejecutarse el script, este script sea accesible desde otros scripts.
        Instance = this;

        //Creo una función de tipo "InvokeRepeating" que irá repitiendo (tras 1 segundo después de haber iniciado el juego) la función "SpawnEnemies",
        //y que se irá repitiendo transcurrido el tiempo almacenado en la variable "timeBetweenEnemies".
        InvokeRepeating("SpawnEnemies", 1.0f, timeBetweenEnemies);
    }

    //FUNCIÓN DE CREACIÓN DE ENEMIGOS
    private void SpawnEnemies()
    {
        //He creado una variable de tipo int que almacena un número que será aleatorio entre el número 0 y el último número del array "posRotEnemy". En cada número se almacena un empty diferente.
        int n = Random.Range(0, posRotEnemy.Length);

        //Aquí he indicado que lo que se instancie sea el enemigo en la posición y rotación del empty seleccionado de forma aleatoria.
        Instantiate(enemy, posRotEnemy[n].position, posRotEnemy[n].rotation);
    }
}

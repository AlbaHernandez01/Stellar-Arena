using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //He convertido este script en singleton para poder acceder a �l mediante otros scripts.
    public static SpawnManager Instance { get; private set; }

    //He creado una variable de tipo GameObject en la que se almacena el personaje enemigo.
    [SerializeField]
    private GameObject enemy;

    //He creado una variable de tipo Transform que, a su vez, es un array que almacena diferentes posiciones en las que ir�n spawneando los enemigos.
    [SerializeField]
    private Transform[] posRotEnemy;

    //He creado, adem�s, una varible tipo float que almacena el tiempo que habr� entre el spawn de un enemigo y otro.
    [SerializeField]
    private float timeBetweenEnemies = 5;
    private void Start()
    {
        //Le indico a "Instance" que cuando empiece a ejecutarse el script, este script sea accesible desde otros scripts.
        Instance = this;

        //Creo una funci�n de tipo "InvokeRepeating" que ir� repitiendo (tras 1 segundo despu�s de haber iniciado el juego) la funci�n "SpawnEnemies",
        //y que se ir� repitiendo transcurrido el tiempo almacenado en la variable "timeBetweenEnemies".
        InvokeRepeating("SpawnEnemies", 1.0f, timeBetweenEnemies);
    }

    //FUNCI�N DE CREACI�N DE ENEMIGOS
    private void SpawnEnemies()
    {
        //He creado una variable de tipo int que almacena un n�mero que ser� aleatorio entre el n�mero 0 y el �ltimo n�mero del array "posRotEnemy". En cada n�mero se almacena un empty diferente.
        int n = Random.Range(0, posRotEnemy.Length);

        //Aqu� he indicado que lo que se instancie sea el enemigo en la posici�n y rotaci�n del empty seleccionado de forma aleatoria.
        Instantiate(enemy, posRotEnemy[n].position, posRotEnemy[n].rotation);
    }
}

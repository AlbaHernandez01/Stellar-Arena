using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //He convertido este script en singleton para poder acceder a él mediante otros scripts.
    public static GameManager Instance { get; private set; }

    //He creado dos variables de tipo GameObjec: una que almacena el panel de "Game Over" y otra que almacena el panel de la puntuación durante la partida.
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private GameObject panelUIScore;

    //He creado dos variables de tipo AudioSource: una que almacena el sonido de cuando se pierde y termina la partida y otra que almacena la música que va a sonar durante el juego.
    [SerializeField]
    private AudioSource endGame;
    [SerializeField]
    private AudioSource musicGame;
    private void Awake()
    {
        //Le indico a "Instance" que cuando empiece a ejecutarse el script, este script sea accesible desde otros scripts.
        Instance = this;

        //Le indico a la variable de audio "musicGame" que se reproduzca justo al cargar la escena "Level01".
        musicGame.Play();

        //Aplico el método "Cursor.lockState" y le digo que aplique el modo "Lock" en el cual el cursor se bloquea para no hacerse visible y poder tener más visibilidad dentro del juego
        //mientras el personaje se desplaza y dispara nada más cargar la escena.
        Cursor.lockState = CursorLockMode.Locked;
    }

    //FUNCIÓN DE FINALIZAR LA PARTIDA
    public void GameOver()
    {
        //Le indico a la variable de audio "musicGame" que deje de reproducirse.
        musicGame.Stop();

        //Acto seguido, activo la variable de audio "endGame" para que suene el sonido de Game Over e indicar que se ha finalizado la partida.
        endGame.Play();

        //Le he indicado que la variable que almacena el panel de Game Over se active.
        panelGameOver.SetActive(true);

        //Y al mismo tiempo, le digo a la variable que almacena la interfaz dentro del juego que se desactive para que no aparezca cuando termine la partida.
        panelUIScore.SetActive(false);

        //Y accedo al script que se encarga del spawn de los enemigos y le digo que desactive el bucle que reproduce constantemente la función de creación de enemigos,
        //para que no aparezcan una vez finalizado el juego.
        SpawnManager.Instance.CancelInvoke();

        //Y para finalizar reactivamos el cursor con el método anterior aplicando esta vez el modo "Confined" para que se haga visible de nuevo y poder interactuar con los botones de la interfaz.
        Cursor.lockState = CursorLockMode.Confined;
    }

    //FUNCIÓN DE CARGA DE ESCENA
    public void _LoadSceneLevel()
    {
        //He creado una función para el botón "PLAY" del panel Game Over. Esta función se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Level01",
        //indicada entre los paréntesis. 
        SceneManager.LoadScene("Level01");
    }

    //FUNCIÓN DE CARGA DEL MENÚ PRINCIPAL
    public void _LoadMenu()
    {
        //He creado una función para el botón "MENU" del panel Game Over. Esta función se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Menu",
        //indicada entre los paréntesis. 
        SceneManager.LoadScene("Menu");
    }

    //FUNCIÓN DE SALIDA DEL JUEGO DESDE UNITY
    public void _ExitGame()
    {
        //He creado otra función para el botón "EXIT" del panel Game Over. Esta función se encarga de salir del PlayMode de Unity y, así, detener el juego. 
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}

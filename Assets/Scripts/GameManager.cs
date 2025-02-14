using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //He convertido este script en singleton para poder acceder a �l mediante otros scripts.
    public static GameManager Instance { get; private set; }

    //He creado dos variables de tipo GameObjec: una que almacena el panel de "Game Over" y otra que almacena el panel de la puntuaci�n durante la partida.
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private GameObject panelUIScore;

    //He creado dos variables de tipo AudioSource: una que almacena el sonido de cuando se pierde y termina la partida y otra que almacena la m�sica que va a sonar durante el juego.
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

        //Aplico el m�todo "Cursor.lockState" y le digo que aplique el modo "Lock" en el cual el cursor se bloquea para no hacerse visible y poder tener m�s visibilidad dentro del juego
        //mientras el personaje se desplaza y dispara nada m�s cargar la escena.
        Cursor.lockState = CursorLockMode.Locked;
    }

    //FUNCI�N DE FINALIZAR LA PARTIDA
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

        //Y accedo al script que se encarga del spawn de los enemigos y le digo que desactive el bucle que reproduce constantemente la funci�n de creaci�n de enemigos,
        //para que no aparezcan una vez finalizado el juego.
        SpawnManager.Instance.CancelInvoke();

        //Y para finalizar reactivamos el cursor con el m�todo anterior aplicando esta vez el modo "Confined" para que se haga visible de nuevo y poder interactuar con los botones de la interfaz.
        Cursor.lockState = CursorLockMode.Confined;
    }

    //FUNCI�N DE CARGA DE ESCENA
    public void _LoadSceneLevel()
    {
        //He creado una funci�n para el bot�n "PLAY" del panel Game Over. Esta funci�n se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Level01",
        //indicada entre los par�ntesis. 
        SceneManager.LoadScene("Level01");
    }

    //FUNCI�N DE CARGA DEL MEN� PRINCIPAL
    public void _LoadMenu()
    {
        //He creado una funci�n para el bot�n "MENU" del panel Game Over. Esta funci�n se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Menu",
        //indicada entre los par�ntesis. 
        SceneManager.LoadScene("Menu");
    }

    //FUNCI�N DE SALIDA DEL JUEGO DESDE UNITY
    public void _ExitGame()
    {
        //He creado otra funci�n para el bot�n "EXIT" del panel Game Over. Esta funci�n se encarga de salir del PlayMode de Unity y, as�, detener el juego. 
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}

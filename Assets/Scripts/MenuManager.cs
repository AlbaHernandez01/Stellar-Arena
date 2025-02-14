using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //FUNCI�N DE CARGA DE LA ESCENA DEL JUEGO
    public void _LoadGame()
    {
        //He creado una funci�n para el bot�n "PLAY" del men�. Esta funci�n se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Level01", indicada entre los par�ntesis. 
        SceneManager.LoadScene("Level01");
    }

    //FUNCI�N DE SALIDA DEL JUEGO DESDE UNITY
    public void _ExitGame()
    {
        //He creado otra funci�n para el bot�n "EXIT" del men�. Esta funci�n se encarga de salir del PlayMode de Unity y, as�, detener el juego. 
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //FUNCIÓN DE CARGA DE LA ESCENA DEL JUEGO
    public void _LoadGame()
    {
        //He creado una función para el botón "PLAY" del menú. Esta función se encarga de entrar dentro del SceneManager de Unity y cargar la escena "Level01", indicada entre los paréntesis. 
        SceneManager.LoadScene("Level01");
    }

    //FUNCIÓN DE SALIDA DEL JUEGO DESDE UNITY
    public void _ExitGame()
    {
        //He creado otra función para el botón "EXIT" del menú. Esta función se encarga de salir del PlayMode de Unity y, así, detener el juego. 
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}

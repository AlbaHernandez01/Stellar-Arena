using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    //He convertido este script en singleton para poder acceder a �l mediante otros scripts.
    public static Highscore Instance { get; private set; }

    //He creado una funci�n de tipo int para los puntos que ir� ganando el jugador, le he dado el valor 0 para que empiece a contar desde ese n�mero.
    [SerializeField]
    private int points = 0;

    //He creado dos funciones de tipo TMP_Text: una almacena un texto de tipo TMPro que mostrar� los puntos que va ganando el jugador
    //y la otra almacena otro texto de tipo TMPro pero en el que se mostrar� el high score.
    [SerializeField]
    private TMP_Text pointsText;
    [SerializeField]
    private TMP_Text maxPointsText;
    private void Awake()
    {
        //Le indico a "Instance" que cuando empiece a ejecutarse el script, este script sea accesible desde otros scripts.
        Instance = this;
    }

    //FUNCI�N DE SUMA DE PUNTOS
    public void IncreasePoints()
    {
        //Sumar� puntos cada vez que se realice esta funci�n.
        points++;

        //Los puntos que vaya sumando se ir�n actualizando y mostrando en el texto almacenado en la variable "pointsText".
        pointsText.text = points.ToString();

        //A su vez, tambi�n va actualizando la funci�n "UpdateMaxPoints()".
        UpdateMaxPoints();
    }

    //FUNCI�N DE ALMACENAJE DEL HIGH SCORE
    public void UpdateMaxPoints()
    {
        //He creado una variable de tipo int a la cual se le almacena una clase de Unity que guarda y recupera un valor. Dentro del par�ntesis,
        //le he indicado que el valor que empezar� a almacenar ser� el m�ximo de puntos y le he indicado que empiece desde 0.
        int maxPoints = PlayerPrefs.GetInt("Max", 0);

        //Dentro de un if, indico que solo y cuando el puntaje de la partida tenga un valor mayor que el �ltimo elemento guardado en maxPoints,
        //que se actualice y lo ponga como nuevo high score.
        if (points >= maxPoints) 
        {
            //Aqu� le indico que el valor de maxPoints tiene que cambiar al nuevo valor superior del puntaje de la partida.
            maxPoints = points;

            //Aqu� entre los par�ntesis le indico que se guarde el valor m�s alto (para que ahora no sea el valor 0 lo que se muestre, sino el high score),
            //y que cuando vuelva a ocurrir lo del if, se actualice "Max".
            PlayerPrefs.SetInt("Max", maxPoints); 
        }
        //En esta sentencia, le digo que el texto almacenado en "maxPointsText" tiene que irse actualizando en cada partida, mostrando en pantalla high score.
        maxPointsText.text = "HIGH SCORE: " + maxPoints.ToString();
    }
}

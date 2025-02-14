using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement")]
    //He creado dos variables de tipo float: una para almacenar la velocidad de movimiento y otra para almacenar el giro que tendr� el jugador.
    [SerializeField]
    private float speed = 15;
    [SerializeField]
    private float turnSpeed = 90;

    [Header("Attack")]
    //He creado una variable de tipo GameObject que almacena el prefab de la bala del jugador.
    [SerializeField]
    private GameObject bulletPlayer;

    //He creado una variable de tipo Transform que almacena la posici�n y rotaci�n del empty de donde saldr� la bala del jugador cuando dispare.
    [SerializeField]
    private Transform shootPlayer;

    [Header("Animations")]
    //He creado una variable de tipo "Animator" que almacenar� el componente Animator del jugador para poder aplicarle las animaciones.
    [SerializeField]
    private Animator animator;

    [Header("Damage")]
    //He creado tres variables de tipo float: una que almacena la vida m�xima del jugador, otra que almacena su vida actual (tiene el valor m�ximo porque empieza el juego teniendo
    //la vida al m�ximo) y una que almacena la cantidad de vida que se resta a la vida actual cuando la bala del enemigo choca conntra el jugador.
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth = 100;
    [SerializeField]
    private float damageBullet = 20;

    //He creado una variable de tipo Image en la cual se almacena la barra de vida del jugador.
    [SerializeField]
    private Image lifeBar;

    //He creado una variable de tipo ParticleSystem que almacena el efecto de sangre que se reproducir� cada vez que una bala colisione con el jugador.
    [SerializeField]
    private ParticleSystem bloody;

    //He creado dos variables de tipo AudioSource: una que guarda el sonido del disparo que se reproducir� cada vez que dispare el jugador y otra que guarda un sonido de quejido
    //que se reproducir� cuando el jugador reciba da�o de la bala del enemigo.
    [SerializeField]
    private AudioSource shootAudio;
    [SerializeField]
    private AudioSource hurtAudio;
    private void Awake()
    {   
        //Le he asignado a la variable "animator" el componente Animator del jugador.
        animator = GetComponent<Animator>();

        //He indicado que, al inicio de la partida, la vida actual empiece siendo igual que la vida m�xima.
        currentHealth = maxHealth;

        //Aqu� le he indicado que la barra de vida est� completa (con el valor 1) cuando inicia la partida.
        lifeBar.fillAmount = 1;

        //Con este m�todo me aseguro que la variable "bloody" que almacena la animaci�n de part�culas no se reproduzca justo al empezar el juego.
        bloody.Stop();
    }
    private void Update()
    {
        //Se llaman a las funciones en el Update para que se ejecuten correctamente cuando las inicialicemos.
        Shoot();
        Movement();
        Turning();
    }

    //FUNCI�N DE MOVIMIENTO
    private void Movement()
    {
        //Indico con un if que si se cumple que pulsamos las flechas o WASD, se ejecute el movimiento programado.
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            //Para ir hacia delante, con el m�todo Translate le indicamos que vaya hacia delante con la velocidad de movimiento asignada cada vez que se pulse cualquiera de las flechas verticales
            //o WS, y para darle fluidez al movimiento lo multiplico por un Time.deltaTime.
            transform.Translate(Vector3.forward * speed * Input.GetAxis("Vertical") * Time.deltaTime);

            //Para ir hacia los lados, con el m�todo Translate le indicamos que vaya hacia los lados con la velocidad de movimiento asignada cada vez que se pulse cualquiera de las flechas horizontales
            //o AD, y para darle fluidez al movimiento lo multiplico por un Time.deltaTime.
            transform.Translate(Vector3.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime);
            
            //Finalmente, le decimos que cada que ocurra el movimiento se reproduzca la animaci�n de correr.
            animator.Play("CharacterArmature|Run_Gun");
        }
        else
        {
            //Con un else le indicamos que, en caso de que no se mueva, siga con una animaci�n donde se vea quieto y respirando.
            animator.Play("CharacterArmature|Idle");
        }
    }

    //FUNCI�N DE GIRO
    private void Turning()
    {
        //Con un if indico que, si el rat�n se mueve de izquierda a derecha (sobre el eje X), tambi�n lo haga el personaje a la vez, para as� poder rotar mietras se juega.
        if (Input.GetAxis("Mouse X") != 0)
        {
            //Aqu� indico con el m�todo Rotate que rote el personaje usando el eje Y global a la velocidad de giro asignada y con un Time.deltaTime para que rote de forma fluida.
            transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
        }
    }

    //FUNCI�N DE DISPARO
    private void Shoot()
    {
        //Con un if indico que, cuando pulse el bot�n izquierdo del rat�n, dispare.
        if (Input.GetMouseButtonDown(0))
        {
            //Aqu� le digo a la variable "shootAudio" que se reproduzca.
            shootAudio.Play();

            //Aqu� instancio las balas, que saldr�n desde la posici�n y rotaci�n del empty que funciona como disparador.
            Instantiate(bulletPlayer, shootPlayer.position, shootPlayer.rotation);
        }
    }

    //COLISIONES
    private void OnTriggerEnter(Collider col)
    {
        //Con un if indico que, si colisiona con la bala del enemigo (la bala que tiene el tag "Enemy_Bullet"), reste vida al jugador.
        if (col.CompareTag("Enemy_Bullet"))
        {
            //Aqu� le indico que le reste a la vida actual el valor del da�o que produce una bala enemiga.
            currentHealth -= damageBullet;

            //Despu�s, divido el valor de la vida actual por la vida m�xima para calcular el porcentaje y cantidad de barra de vida que se va a visualizar.
            lifeBar.fillAmount = currentHealth / maxHealth;

            //Indico que la bala del enemigo se destruya si ha chocado contra el jugador.
            Destroy(col.gameObject);

            //Le digo que se reproduzca la animaci�n de part�culas almacenada en la variable "bloody".
            bloody.Play();

            //Le digo que se reproduzca el audio almacenado en la variable "hurtAudio".
            hurtAudio.Play();

            //Con un if indico que, en el caso de que la salud actual sea mayor o igual a 0, se inicialice la funci�n "Death()".
            if(currentHealth <= 0)
            {
                Death();
            }
        }
    }

    //FUNCI�N DE MUERTE
    private void Death()
    {   
        //Desenparento la c�mara del player para que lo deje de seguir, sea independiente y no se destruya con �l cuando muera.
        Camera.main.transform.SetParent(null);

        //Accedo al script "GameManager" para usar su funci�n "GameOver()".
        GameManager.Instance.GameOver();

        //Destruyo al jugador.
        Destroy(gameObject);
    }
}

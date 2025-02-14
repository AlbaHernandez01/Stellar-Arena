using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement")]
    //He creado dos variables de tipo float: una para almacenar la velocidad de movimiento y otra para almacenar el giro que tendrá el jugador.
    [SerializeField]
    private float speed = 15;
    [SerializeField]
    private float turnSpeed = 90;

    [Header("Attack")]
    //He creado una variable de tipo GameObject que almacena el prefab de la bala del jugador.
    [SerializeField]
    private GameObject bulletPlayer;

    //He creado una variable de tipo Transform que almacena la posición y rotación del empty de donde saldrá la bala del jugador cuando dispare.
    [SerializeField]
    private Transform shootPlayer;

    [Header("Animations")]
    //He creado una variable de tipo "Animator" que almacenará el componente Animator del jugador para poder aplicarle las animaciones.
    [SerializeField]
    private Animator animator;

    [Header("Damage")]
    //He creado tres variables de tipo float: una que almacena la vida máxima del jugador, otra que almacena su vida actual (tiene el valor máximo porque empieza el juego teniendo
    //la vida al máximo) y una que almacena la cantidad de vida que se resta a la vida actual cuando la bala del enemigo choca conntra el jugador.
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth = 100;
    [SerializeField]
    private float damageBullet = 20;

    //He creado una variable de tipo Image en la cual se almacena la barra de vida del jugador.
    [SerializeField]
    private Image lifeBar;

    //He creado una variable de tipo ParticleSystem que almacena el efecto de sangre que se reproducirá cada vez que una bala colisione con el jugador.
    [SerializeField]
    private ParticleSystem bloody;

    //He creado dos variables de tipo AudioSource: una que guarda el sonido del disparo que se reproducirá cada vez que dispare el jugador y otra que guarda un sonido de quejido
    //que se reproducirá cuando el jugador reciba daño de la bala del enemigo.
    [SerializeField]
    private AudioSource shootAudio;
    [SerializeField]
    private AudioSource hurtAudio;
    private void Awake()
    {   
        //Le he asignado a la variable "animator" el componente Animator del jugador.
        animator = GetComponent<Animator>();

        //He indicado que, al inicio de la partida, la vida actual empiece siendo igual que la vida máxima.
        currentHealth = maxHealth;

        //Aquí le he indicado que la barra de vida está completa (con el valor 1) cuando inicia la partida.
        lifeBar.fillAmount = 1;

        //Con este método me aseguro que la variable "bloody" que almacena la animación de partículas no se reproduzca justo al empezar el juego.
        bloody.Stop();
    }
    private void Update()
    {
        //Se llaman a las funciones en el Update para que se ejecuten correctamente cuando las inicialicemos.
        Shoot();
        Movement();
        Turning();
    }

    //FUNCIÓN DE MOVIMIENTO
    private void Movement()
    {
        //Indico con un if que si se cumple que pulsamos las flechas o WASD, se ejecute el movimiento programado.
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            //Para ir hacia delante, con el método Translate le indicamos que vaya hacia delante con la velocidad de movimiento asignada cada vez que se pulse cualquiera de las flechas verticales
            //o WS, y para darle fluidez al movimiento lo multiplico por un Time.deltaTime.
            transform.Translate(Vector3.forward * speed * Input.GetAxis("Vertical") * Time.deltaTime);

            //Para ir hacia los lados, con el método Translate le indicamos que vaya hacia los lados con la velocidad de movimiento asignada cada vez que se pulse cualquiera de las flechas horizontales
            //o AD, y para darle fluidez al movimiento lo multiplico por un Time.deltaTime.
            transform.Translate(Vector3.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime);
            
            //Finalmente, le decimos que cada que ocurra el movimiento se reproduzca la animación de correr.
            animator.Play("CharacterArmature|Run_Gun");
        }
        else
        {
            //Con un else le indicamos que, en caso de que no se mueva, siga con una animación donde se vea quieto y respirando.
            animator.Play("CharacterArmature|Idle");
        }
    }

    //FUNCIÓN DE GIRO
    private void Turning()
    {
        //Con un if indico que, si el ratón se mueve de izquierda a derecha (sobre el eje X), también lo haga el personaje a la vez, para así poder rotar mietras se juega.
        if (Input.GetAxis("Mouse X") != 0)
        {
            //Aquí indico con el método Rotate que rote el personaje usando el eje Y global a la velocidad de giro asignada y con un Time.deltaTime para que rote de forma fluida.
            transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
        }
    }

    //FUNCIÓN DE DISPARO
    private void Shoot()
    {
        //Con un if indico que, cuando pulse el botón izquierdo del ratón, dispare.
        if (Input.GetMouseButtonDown(0))
        {
            //Aquí le digo a la variable "shootAudio" que se reproduzca.
            shootAudio.Play();

            //Aquí instancio las balas, que saldrán desde la posición y rotación del empty que funciona como disparador.
            Instantiate(bulletPlayer, shootPlayer.position, shootPlayer.rotation);
        }
    }

    //COLISIONES
    private void OnTriggerEnter(Collider col)
    {
        //Con un if indico que, si colisiona con la bala del enemigo (la bala que tiene el tag "Enemy_Bullet"), reste vida al jugador.
        if (col.CompareTag("Enemy_Bullet"))
        {
            //Aquí le indico que le reste a la vida actual el valor del daño que produce una bala enemiga.
            currentHealth -= damageBullet;

            //Después, divido el valor de la vida actual por la vida máxima para calcular el porcentaje y cantidad de barra de vida que se va a visualizar.
            lifeBar.fillAmount = currentHealth / maxHealth;

            //Indico que la bala del enemigo se destruya si ha chocado contra el jugador.
            Destroy(col.gameObject);

            //Le digo que se reproduzca la animación de partículas almacenada en la variable "bloody".
            bloody.Play();

            //Le digo que se reproduzca el audio almacenado en la variable "hurtAudio".
            hurtAudio.Play();

            //Con un if indico que, en el caso de que la salud actual sea mayor o igual a 0, se inicialice la función "Death()".
            if(currentHealth <= 0)
            {
                Death();
            }
        }
    }

    //FUNCIÓN DE MUERTE
    private void Death()
    {   
        //Desenparento la cámara del player para que lo deje de seguir, sea independiente y no se destruya con él cuando muera.
        Camera.main.transform.SetParent(null);

        //Accedo al script "GameManager" para usar su función "GameOver()".
        GameManager.Instance.GameOver();

        //Destruyo al jugador.
        Destroy(gameObject);
    }
}

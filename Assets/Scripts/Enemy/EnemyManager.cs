using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager : MonoBehaviour
{
    [Header("Movement")]
    //He creado dos variables de tipo float: una que determina la velocidad del enemigo y otra para determinar la distancia que tendr� respecto al jugador.
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float distanceToPlayer = 5f;

    //He creado otra variable de tipo GameObject que almacena el prefab del jugador. 
    private GameObject player;

    [Header("Animation")]
    [SerializeField]
    //He creado una variable de tipo "Animator" que almacenar� el componente Animator del jugador para poder aplicarle las animaciones.
    private Animator animator;

    [Header("Attack")]
    //He creado una variable de tipo GameObject que almacena el prefab de la bala del enemigo.
    [SerializeField]
    private GameObject bulletEnemy;

    //He creado otra variable de tipo Transform que guarda la posici�n y rotaci�n del empty de donde saldr�n las balas del enemigo.
    [SerializeField]
    private Transform shootEnemy;

    //He creado otra variable de tipo float en la que se determina el tiempo que habr� entre bala y bala del enemigo.
    [SerializeField]
    private float timeBetweenBullets = 2;

    //He creado una variable de tipo AudioSource la cual almacena el sonido de disparo enemigo.
    private AudioSource shootAudio;

    [Header("Damage")]
    //He creado tres variables de tipo float: una que almacena la vida m�xima del enemigo, otra que almacena la vida actual (tiene el valor m�ximo porque cada que aparece
    //en el juego empieza teniendo la vida al m�ximo) y una que almacena la cantidad de vida que se resta a la vida actual cuando la bala del jugador choca contra el enemigo.
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth = 100;
    [SerializeField]
    private float damageBullet = 20;

    //He creado una variable de tipo Image en la cual se almacena la barra de vida del enemigo.
    [SerializeField]
    private Image lifeBar;

    //He creado una variable de tipo ParticleSystem que almacena el efecto de sangre que se reproducir� cada vez que una bala colisione con el enemigo.
    [SerializeField]
    private ParticleSystem bloody;

    //He creado, adem�s, una booleana que indica cuando el enemigo ha muerto o no (tiene el valor "false" porque est� desactivada al inicio de la partida).
    [SerializeField]
    private bool imDead = false;

    private void Awake()
    {
        //Creo una funci�n de tipo "InvokeRepeating" que ir� repitiendo (tras 1 segundo despu�s de haber iniciado el juego) la funci�n "Attak",
        //y que se ir� repitiendo transcurrido el tiempo almacenado en la variable "timeBetweenBullets".
        InvokeRepeating("Attack", 1, timeBetweenBullets);

        //Le he indicado a la variable "player" que almacene al prefab que contenga el tag "Player".
        player = GameObject.FindWithTag("Player");

        //Le he asignado a la variable de audio "shootAudio" el componente AudioSource del enemigo (que contendr� el sonido del disparo).
        shootAudio = GetComponent<AudioSource>();

        //He indicado que, cuando spawnee el enemigo, la vida actual empiece siendo igual que la vida m�xima.
        currentHealth = maxHealth;

        //Aqu� le he indicado que la barra de vida est� completa (con el valor 1) cuando spawnea el enemigo.
        lifeBar.fillAmount = 1;

        //Con este m�todo me aseguro que la variable "bloody" que almacena la animaci�n de part�culas no se reproduzca justo al empezar el juego.
        bloody.Stop();
    }
    private void Update()
    {
        //Con un if indico que, si el jugador no existe o si ya ha muerto (la booleana est� activada), que no devuelva ninguna funci�n y, por lo tanto, que no haga nada.
        if (player == null || imDead == true)
            return;

        //En caso de s� existir un player, con un "LookAt" le indico que mire constantemente la posici�n del jugador.
        transform.LookAt(player.transform.position);

        //Despu�s, que realice la funci�n de persecuci�n del jugador.
        FollowPlayer();
    }

    //FUNCI�N DE PERSECUCI�N
    private void FollowPlayer()
    {
        //He creado una variable tipo float que calcula la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(transform.position, player.transform.position);

        //Con un if indico que, si la distancia calculada es mayor que el valor de la variable "distanceToPlayer" (que es la distancia m�nima que tiene que tener respecto al jugador),
        //se mueva hacia el jugador.
        if (distance > distanceToPlayer)
        {
            //Aqu� le digo que use el m�todo "Translate" para moverse hacia delante a la velocidad asignada y con un Time.deltaTime para que sea un movimiento fluido.
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Le digo tambi�n que se reproduzca la animaci�n de correr.
            animator.Play("CharacterArmature|Run_Gun");
        }
        //Con un else le indicamos que, en caso de que no se mueva, siga con una animaci�n donde se vea quieto y respirando.
        else
            animator.Play("CharacterArmature|Idle");
    }

    //FUNCI�N DE ATAQUE
    private void Attack()
    {
        //Con un if indico que, si el jugador no existe o si ya ha muerto (la booleana est� activada), que no devuelva ninguna funci�n y, por lo tanto, que no haga nada.
        if (player == null)
            return;

        //En caso de s� existir, le digo a la variable "shootAudio" que se reproduzca.
        shootAudio.Play();
        
        //Y tambi�n que se instancie el prefab de la bala enemiga desde la posici�n y rotaci�n del empty almacenado en la variable "shootEnemy";
        Instantiate(bulletEnemy, shootEnemy.position, shootEnemy.rotation);
    }

    //COLISIONES
    private void OnTriggerEnter(Collider col)
    {
        //Con un if indico que, si colisiona con la bala del jugador (la bala que tiene el tag "Player_Bullet"), reste vida al enemigo.
        if (col.CompareTag("Player_Bullet"))
        {
            //Aqu� le indico que le reste a la vida actual el valor del da�o que produce una bala del player.
            currentHealth -= damageBullet;

            //Despu�s, divido el valor de la vida actual por la vida m�xima para calcular el porcentaje y cantidad de barra de vida que se va a visualizar.
            lifeBar.fillAmount = currentHealth / maxHealth;

            //Indico que la bala del jugador se destruya si ha chocado contra el enemigo.
            Destroy(col.gameObject);

            //Le digo que se reproduzca la animaci�n de part�culas almacenada en la variable "bloody".
            bloody.Play();

            //Accedo al script "HighScore" para poder usar la funci�n "IncreasePoints()" y que suma puntos al jugador cada que le de a un enemigo.
            Highscore.Instance.IncreasePoints();

            //Con un if indico que, en el caso de que la salud actual sea mayor o igual a 0, se inicialice la funci�n "Death()".
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    //FUNCI�N DE MUERTE
    private void Death()
    {
        //Cuando muera el jugador, le digo a la booleana "imDead" que se active. 
        imDead = true;

        //Le digo tambi�n que se reproduzca la animaci�n de muerte.
        animator.Play("CharacterArmature|Death");

        //Y que se destruya a los 2 segundos de morir.
        Destroy(gameObject, 2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager : MonoBehaviour
{
    [Header("Movement")]
    //He creado dos variables de tipo float: una que determina la velocidad del enemigo y otra para determinar la distancia que tendrá respecto al jugador.
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float distanceToPlayer = 5f;

    //He creado otra variable de tipo GameObject que almacena el prefab del jugador. 
    private GameObject player;

    [Header("Animation")]
    [SerializeField]
    //He creado una variable de tipo "Animator" que almacenará el componente Animator del jugador para poder aplicarle las animaciones.
    private Animator animator;

    [Header("Attack")]
    //He creado una variable de tipo GameObject que almacena el prefab de la bala del enemigo.
    [SerializeField]
    private GameObject bulletEnemy;

    //He creado otra variable de tipo Transform que guarda la posición y rotación del empty de donde saldrán las balas del enemigo.
    [SerializeField]
    private Transform shootEnemy;

    //He creado otra variable de tipo float en la que se determina el tiempo que habrá entre bala y bala del enemigo.
    [SerializeField]
    private float timeBetweenBullets = 2;

    //He creado una variable de tipo AudioSource la cual almacena el sonido de disparo enemigo.
    private AudioSource shootAudio;

    [Header("Damage")]
    //He creado tres variables de tipo float: una que almacena la vida máxima del enemigo, otra que almacena la vida actual (tiene el valor máximo porque cada que aparece
    //en el juego empieza teniendo la vida al máximo) y una que almacena la cantidad de vida que se resta a la vida actual cuando la bala del jugador choca contra el enemigo.
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth = 100;
    [SerializeField]
    private float damageBullet = 20;

    //He creado una variable de tipo Image en la cual se almacena la barra de vida del enemigo.
    [SerializeField]
    private Image lifeBar;

    //He creado una variable de tipo ParticleSystem que almacena el efecto de sangre que se reproducirá cada vez que una bala colisione con el enemigo.
    [SerializeField]
    private ParticleSystem bloody;

    //He creado, además, una booleana que indica cuando el enemigo ha muerto o no (tiene el valor "false" porque está desactivada al inicio de la partida).
    [SerializeField]
    private bool imDead = false;

    private void Awake()
    {
        //Creo una función de tipo "InvokeRepeating" que irá repitiendo (tras 1 segundo después de haber iniciado el juego) la función "Attak",
        //y que se irá repitiendo transcurrido el tiempo almacenado en la variable "timeBetweenBullets".
        InvokeRepeating("Attack", 1, timeBetweenBullets);

        //Le he indicado a la variable "player" que almacene al prefab que contenga el tag "Player".
        player = GameObject.FindWithTag("Player");

        //Le he asignado a la variable de audio "shootAudio" el componente AudioSource del enemigo (que contendrá el sonido del disparo).
        shootAudio = GetComponent<AudioSource>();

        //He indicado que, cuando spawnee el enemigo, la vida actual empiece siendo igual que la vida máxima.
        currentHealth = maxHealth;

        //Aquí le he indicado que la barra de vida está completa (con el valor 1) cuando spawnea el enemigo.
        lifeBar.fillAmount = 1;

        //Con este método me aseguro que la variable "bloody" que almacena la animación de partículas no se reproduzca justo al empezar el juego.
        bloody.Stop();
    }
    private void Update()
    {
        //Con un if indico que, si el jugador no existe o si ya ha muerto (la booleana está activada), que no devuelva ninguna función y, por lo tanto, que no haga nada.
        if (player == null || imDead == true)
            return;

        //En caso de sí existir un player, con un "LookAt" le indico que mire constantemente la posición del jugador.
        transform.LookAt(player.transform.position);

        //Después, que realice la función de persecución del jugador.
        FollowPlayer();
    }

    //FUNCIÓN DE PERSECUCIÓN
    private void FollowPlayer()
    {
        //He creado una variable tipo float que calcula la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(transform.position, player.transform.position);

        //Con un if indico que, si la distancia calculada es mayor que el valor de la variable "distanceToPlayer" (que es la distancia mínima que tiene que tener respecto al jugador),
        //se mueva hacia el jugador.
        if (distance > distanceToPlayer)
        {
            //Aquí le digo que use el método "Translate" para moverse hacia delante a la velocidad asignada y con un Time.deltaTime para que sea un movimiento fluido.
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Le digo también que se reproduzca la animación de correr.
            animator.Play("CharacterArmature|Run_Gun");
        }
        //Con un else le indicamos que, en caso de que no se mueva, siga con una animación donde se vea quieto y respirando.
        else
            animator.Play("CharacterArmature|Idle");
    }

    //FUNCIÓN DE ATAQUE
    private void Attack()
    {
        //Con un if indico que, si el jugador no existe o si ya ha muerto (la booleana está activada), que no devuelva ninguna función y, por lo tanto, que no haga nada.
        if (player == null)
            return;

        //En caso de sí existir, le digo a la variable "shootAudio" que se reproduzca.
        shootAudio.Play();
        
        //Y también que se instancie el prefab de la bala enemiga desde la posición y rotación del empty almacenado en la variable "shootEnemy";
        Instantiate(bulletEnemy, shootEnemy.position, shootEnemy.rotation);
    }

    //COLISIONES
    private void OnTriggerEnter(Collider col)
    {
        //Con un if indico que, si colisiona con la bala del jugador (la bala que tiene el tag "Player_Bullet"), reste vida al enemigo.
        if (col.CompareTag("Player_Bullet"))
        {
            //Aquí le indico que le reste a la vida actual el valor del daño que produce una bala del player.
            currentHealth -= damageBullet;

            //Después, divido el valor de la vida actual por la vida máxima para calcular el porcentaje y cantidad de barra de vida que se va a visualizar.
            lifeBar.fillAmount = currentHealth / maxHealth;

            //Indico que la bala del jugador se destruya si ha chocado contra el enemigo.
            Destroy(col.gameObject);

            //Le digo que se reproduzca la animación de partículas almacenada en la variable "bloody".
            bloody.Play();

            //Accedo al script "HighScore" para poder usar la función "IncreasePoints()" y que suma puntos al jugador cada que le de a un enemigo.
            Highscore.Instance.IncreasePoints();

            //Con un if indico que, en el caso de que la salud actual sea mayor o igual a 0, se inicialice la función "Death()".
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    //FUNCIÓN DE MUERTE
    private void Death()
    {
        //Cuando muera el jugador, le digo a la booleana "imDead" que se active. 
        imDead = true;

        //Le digo también que se reproduzca la animación de muerte.
        animator.Play("CharacterArmature|Death");

        //Y que se destruya a los 2 segundos de morir.
        Destroy(gameObject, 2f);
    }
}

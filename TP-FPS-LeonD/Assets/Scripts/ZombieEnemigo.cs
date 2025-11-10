using TMPro;
using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ZombieEnemigo : MonoBehaviour
{
    [SerializeField] private int Salud = 100;
    [SerializeField] private TextMeshProUGUI SaludTexto;
    [SerializeField] private Image BarraVida;
    private Animator zombieAnimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoDaño;
    [SerializeField] AudioClip SonidoMuerte;
    [SerializeField] AudioClip SonidoAtaque;
    private NavMeshAgent zombieNavMeshAgent;
    [SerializeField] Transform Player;
    [SerializeField] float chaseInterval = 0.5f;
    private ReglasDeJuego reglasdejuego;
    private VidaPlayer vidaplayer;
    public int PuntosPorZombie = 1;
    public int DañoZombie = 20;

    private void Start()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieNavMeshAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Objetivo").transform;
        reglasdejuego = FindFirstObjectByType<ReglasDeJuego>();
        vidaplayer = FindFirstObjectByType<VidaPlayer>();
        //UI
        UpdateUI();
        //Sonido
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        InvokeRepeating(nameof(SetDestination), 3f, chaseInterval);
        float randomSpeed = Random.Range(0.8f, 1.5f);
        zombieNavMeshAgent.speed = randomSpeed;
        zombieAnimator.speed = randomSpeed;
    }
    private void Update()
    {
        if (zombieAnimator != null)
        {
            zombieAnimator.SetFloat("MoveSpeed", zombieNavMeshAgent.velocity.magnitude);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            //Hace el daño al enemigo y actualiza el texto
            int DañoBala = collision.gameObject.GetComponent<DañoBala>().CantidadDaño;
            Salud -= DañoBala;
            UpdateUI();
            //Sonido de daño, muerte y aplicar muerte
            if (Salud == 0)
            {
                RuidoMuerte();
                zombieAnimator.SetTrigger("Dead");
                //Destruye objeto
                if (reglasdejuego != null)
                {
                    reglasdejuego.SumarPunto(PuntosPorZombie);
                }
                Muerte();
            }
            else
            {
                RuidoDaño();
            }

            //Destruye bala
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zombieAnimator.SetTrigger("Attack");
            RuidoAtaque();
            vidaplayer.AtaqueZombie(DañoZombie);
        }
    }

    public void SetDestination()
    {
        zombieNavMeshAgent.SetDestination(Player.position);
    }
    private void UpdateUI()
    {
        SaludTexto.text = "Salud " + Salud;
        BarraVida.fillAmount = Salud / 100f;
    }
    private void Muerte()
    {
        zombieNavMeshAgent.isStopped = true;
        Destroy(gameObject, 1.5f);
    }
    void RuidoAtaque()
    {
        if (SonidoMuerte != null && audioSource != null && !audioSource.isPlaying)
        { audioSource.PlayOneShot(SonidoAtaque); }
    }
    void RuidoMuerte()
    {
        if (SonidoMuerte != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SonidoMuerte);
        }
    }
    void RuidoDaño()
    {
        if (SonidoDaño != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SonidoDaño);
        }
    }
}

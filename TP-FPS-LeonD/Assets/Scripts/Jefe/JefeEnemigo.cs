using System.Collections;
using TMPro;
using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class JefeEnemigo : MonoBehaviour
{
    [SerializeField] private float SaludTotal = 500f;
    private float Salud;
    [SerializeField] private TextMeshProUGUI SaludTexto;
    [SerializeField] private Image BarraVida;
    private Animator zombieAnimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoDaño;
    [SerializeField] AudioClip SonidoMuerte;
    [SerializeField] AudioClip SonidoAtaque;
    private NavMeshAgent zombieNavMeshAgent;
    [SerializeField] Transform Player;
    [SerializeField] float chaseInterval = 1f;
    private ReglasDeJuego reglasdejuego;
    private VidaPlayer vidaplayer;
    public int PuntosPorZombie = 1;
    public int DañoZombie = 50;

    [SerializeField] GameObject Corona;

    public GameObject SFXAtaque;
    private Coroutine RutinAtaque;

    public bool puedePerseguir = false;

    void Awake()
    {
       Salud = SaludTotal;
    }

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

        zombieNavMeshAgent.speed = 3f;
        zombieAnimator.speed = 3f;
        if (SFXAtaque) SFXAtaque.SetActive(false);
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
            float DañoBala = collision.gameObject.GetComponent<DañoBala>().CantidadDaño;
            Salud -= DañoBala;
            UpdateUI();
            //Sonido de daño, muerte y aplicar muerte
            if (Salud == 0)
            {
                RuidoMuerte();
                zombieAnimator.SetTrigger("Dead");
                //Destruye objeto
                //if (reglasdejuego != null)
                //{
                //    reglasdejuego.SumarPunto(PuntosPorZombie);
                //}
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
            EfectoAtaque();
            vidaplayer.AtaqueZombie(DañoZombie);
        }
    }

    public void SetDestination()
    {
        if (!puedePerseguir) return;

        zombieNavMeshAgent.SetDestination(Player.position);
    }
    public void ActivarPersecucion()
    {
        if (puedePerseguir) return;

        puedePerseguir = true;
        InvokeRepeating(nameof(SetDestination), 1f, chaseInterval);
    }
    private void UpdateUI()
    {
        SaludTexto.text = "Salud " + Salud;
        BarraVida.fillAmount = Salud / SaludTotal;
    }
    private void Muerte()
    {
        zombieNavMeshAgent.isStopped = true;
        Vector3 offset = new Vector3(0, 2f, 0);
        Instantiate(Corona, transform.position + offset, Quaternion.identity);
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
    public void EfectoAtaque()
    {
        if (RutinAtaque != null)
            StopCoroutine(RutinAtaque);
        RutinAtaque = StartCoroutine(TiempoDaño());
    }
    IEnumerator TiempoDaño()
    {
        if (SFXAtaque)
        {
            SFXAtaque.SetActive(true);
        }

        yield return new WaitForSeconds(0.3f);

        if (SFXAtaque)
        {
            SFXAtaque.SetActive(false);
        }
    }
}

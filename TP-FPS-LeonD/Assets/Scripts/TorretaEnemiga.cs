using System.Collections;
using TMPro;
using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TorretaEnemiga : MonoBehaviour
{
    [SerializeField] private float VidaTorreta = 140f;
    [SerializeField] private float SaludTorreta;
    [SerializeField] private TextMeshProUGUI SaludTexto;
    [SerializeField] private Image BarraVida;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoDisparo;
    [SerializeField] AudioClip SonidoExplocion;

    [SerializeField] Transform Player;
    private VidaPlayer vidaplayer;

    [SerializeField] Transform SpawnDisparo;
    public GameObject SFXDisparo;
    public LineRenderer Laser;
    [SerializeField] private float TiempoDisparo = 1.5f;
    [SerializeField] private float velocidadRotacion = 5f;
    [SerializeField] private float distanciaLaser = 30f;

    private bool jugadorDentro = false;


    private void Start()
    {
        vidaplayer = FindFirstObjectByType<VidaPlayer>();

        UpdateUI();

        SaludTorreta = VidaTorreta;

        Laser.enabled = false;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (jugadorDentro && Player != null)
        {
            // Apuntar hacia el jugador
            Vector3 direccion = Player.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * velocidadRotacion);
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.transform;
            jugadorDentro = true;
            InvokeRepeating("DispararLaser", 0f, TiempoDisparo);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            Player = null;
            CancelInvoke("DispararLaser");
        }
    }

    void DispararLaser()
    {
        if (!jugadorDentro || Player == null) return;

        Vector3 dir = Player.position - SpawnDisparo.position;

        // Raycast
        if (Physics.Raycast(SpawnDisparo.position, dir, out RaycastHit hit, distanciaLaser))
        {
            Laser.enabled = true;
            Laser.SetPosition(0, SpawnDisparo.position);
            Laser.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<VidaPlayer>().AtaqueZombie(20);
            }

            Invoke("ApagarLaser", 0.1f);
        }
    }

    public void ApagarLaser()
    {
        Laser.enabled = false;
    }

    public void EfectoDisparo()
    {
        if (SonidoDisparo != null && audioSource != null && !audioSource.isPlaying)
        { audioSource.PlayOneShot(SonidoDisparo); }

        GameObject newVFX = Instantiate(SFXDisparo, SpawnDisparo.position, SpawnDisparo.rotation);
    }

    void UpdateUI()
    {
        SaludTexto.text = "Salud " + SaludTorreta;
        BarraVida.fillAmount = SaludTorreta / VidaTorreta;
    }
}

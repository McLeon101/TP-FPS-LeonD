using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReglasDeJuego : MonoBehaviour
{
    public static ReglasDeJuego instance;

    public int PuntosZombie = 0; //Puntos zombies
    public int totalpuntoszombies = 0; //Total zombies
    public TextMeshProUGUI TextPZombie; //Texto para UI
    public string EnemigoTag = "Enemigo"; //Etiqueta de Puntos
    public int PuntosPorZombie = 1; //Puntos que da por zombie

    public int PuntosMoneda = 0; //Puntos moneda
    public int TotalMonedas = 0; //Total monedas
    public TextMeshProUGUI TextPMonedas;
    public string MonedaTag = "Moneda";
    public int PuntosPorMoneda = 1;

    public GameObject PerderPanel;
    public GameObject GanarPanel;

    public GameObject Player;
    public GameObject PlayerCamera;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoVictoria;
    [SerializeField] AudioClip SonidoDerrota;
    [SerializeField] AudioClip MusicaAmbiente;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        BloquearMouse();
        //Cuenta cantidad de etiquetas "Zombie"
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag(EnemigoTag);
        totalpuntoszombies = collectibles.Length * PuntosPorZombie;
        //Cuenta cantidad de etiquetas "Monedas"
        GameObject[] collectibles1 = GameObject.FindGameObjectsWithTag(MonedaTag);
        TotalMonedas = collectibles1.Length * PuntosPorMoneda;
        //Actualiza "Puntaje"
        UpdateScoreUI();
        //Desactiva las UI
        if (PerderPanel) PerderPanel.SetActive(false);
        if (GanarPanel) GanarPanel.SetActive(false);
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        MusicaJuego();
        audioSource.volume = 0.2f;
    }
    public void SumarPunto(int amount)
    {
        PuntosZombie += amount;
        UpdateScoreUI();
        RevisarVictoria();
    }

    public void SumarMoneda(int amount)
    {
        PuntosMoneda += 1;
        UpdateScoreUI();
        RevisarVictoria();
    }

    public void RevisarVictoria()
    {
        if (PuntosMoneda >= TotalMonedas && PuntosZombie >= totalpuntoszombies)
        {
            Win();
        }
    }

    void Win()
    {
        if (GanarPanel)
        {
            GanarPanel.SetActive(true);
        }
        MusicaVictoria();
        DesbloquearMouse();
        Time.timeScale = 0f; // Pausar el juego
    }
    public void Perder()
    {
        if (PerderPanel)
        {
            PerderPanel.SetActive(true);
        }
        MusicaDerrota();
        DesbloquearMouse();
        Time.timeScale = 0f; // Pausar el juego
    }
    void UpdateScoreUI()
    {
        if (TextPZombie != null)
        {
            TextPZombie.text = "Zombies: " + PuntosZombie + "/" + totalpuntoszombies;
        }
        if (TextPMonedas != null)
        {
            TextPMonedas.text = "Monedas: " + PuntosMoneda + "/" + TotalMonedas;
        }
    }

    internal void SumarPunto()
    {
        throw new NotImplementedException();
    }
    public void RestartLevel()
    {
        BloquearMouse();
        Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void DesbloquearMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void BloquearMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void MusicaVictoria()
    {
        audioSource.Stop();
        if (SonidoVictoria != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SonidoVictoria);
        }
        audioSource.volume = 0.4f;
    }
    void MusicaDerrota()
    {
        audioSource.Stop();
        if (SonidoDerrota != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SonidoDerrota);
        }
        audioSource.volume = 0.4f;
    }
    void MusicaJuego()
    {
        if (MusicaAmbiente != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MusicaAmbiente);
        }
    }
}

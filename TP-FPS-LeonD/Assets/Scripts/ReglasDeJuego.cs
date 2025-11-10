using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReglasDeJuego : MonoBehaviour
{
    public int score = 0; //Puntos que juntas
    public int totalScore = 0; //Total de puntos
    public TextMeshProUGUI Puntaje; //Texto para UI
    public string EnemigoTag = "Enemigo"; //Etiqueta de Puntos
    public int PuntosPorZombie = 1; //Puntos que da por zombie
    public GameObject PerderPanel;
    public GameObject GanarPanel;
    public GameObject Player;
    public GameObject PlayerCamera;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoVictoria;
    [SerializeField] AudioClip SonidoDerrota;
    [SerializeField] AudioClip MusicaAmbiente;
    void Start()
    {
        //Cuenta cantidad de etiquetas con "Puntos"
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag(EnemigoTag);
        totalScore = collectibles.Length * PuntosPorZombie;
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
        score += amount;
        UpdateScoreUI();
        if (score >= totalScore)
            Win();
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
        if (Puntaje != null)
        {
            Puntaje.text = "Zombies: " + score + "/" + totalScore;
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

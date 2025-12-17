using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReglasDeJuego : MonoBehaviour
{
    public static ReglasDeJuego instance;

    [HideInInspector] public int PuntosZombie = 0; //Puntos zombies
    [HideInInspector] public int totalpuntoszombies = 0; //Total zombies
    public TextMeshProUGUI TextPZombie; //Texto para UI
    public string EnemigoTag = "Enemigo"; //Etiqueta de Puntos
    [HideInInspector] public int PuntosPorZombie = 1; //Puntos que da por zombie

    [HideInInspector] public int PuntosMoneda = 0; //Puntos moneda
    [HideInInspector] public int TotalMonedas = 0; //Total monedas
  //public TextMeshProUGUI TextPMonedas;
  //public string MonedaTag = "Moneda";
    [HideInInspector] public int PuntosPorMoneda = 1;

    [HideInInspector] public bool TengoCorona = false;

    public GameObject PerderPanel;
    public GameObject GanarPanel;
    public TextMeshProUGUI CoronaText;

    public GameObject Player;
    public GameObject PlayerCamera;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoVictoria;
    [SerializeField] AudioClip SonidoDerrota;
    [SerializeField] AudioClip MusicaAmbiente;

    [SerializeField] TextMeshProUGUI TiempoText;
    private float Tiempo;
    [HideInInspector] public int SegundosJugados = 0;
    [HideInInspector] public int MinutosJugados = 0;
    const string CLAVE_RECORD = "MejorTiempo";
    [SerializeField] TextMeshProUGUI RecordText;

    public GameObject LlaveImage;

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
        //GameObject[] collectibles1 = GameObject.FindGameObjectsWithTag(MonedaTag);
        //TotalMonedas = collectibles1.Length * PuntosPorMoneda;
        //Actualiza "Puntaje"
        UpdateScoreUI();
        //Desactiva las UI
        if (PerderPanel) PerderPanel.SetActive(false);
        if (GanarPanel) GanarPanel.SetActive(false);
        if (LlaveImage) LlaveImage.SetActive(false);
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        MusicaJuego();
        audioSource.volume = 0.2f;
        MostrarRecord();
        CoronaText.text = "Busca la corona";
    }

    private void Update()
    {
        Tiempo += Time.deltaTime;
        MinutosJugados = Mathf.FloorToInt(Tiempo / 60);
        SegundosJugados = Mathf.FloorToInt(Tiempo % 60);
        UpdateScoreUI();
    }


    //public void SumarPunto(int amount)
    //{
    //    PuntosZombie += amount;
    //    UpdateScoreUI();
    //    RevisarVictoria();
    //}
    public void RevisarVictoria()
    {
        if (PuntosMoneda >= totalpuntoszombies && TengoCorona == true)
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
        GuardarRecord();
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
            TextPZombie.text = "Moneda: " + PuntosMoneda + "/" + totalpuntoszombies;
        }
        //if (TextPMonedas != null)
        //{
        //    TextPMonedas.text = "Monedas: " + PuntosMoneda + "/" + TotalMonedas;
        //}

        if (TiempoText != null)
        {
            TiempoText.text = MinutosJugados + ":" + SegundosJugados.ToString("00");
        }
    }
    public void SumarMoneda(int amount)
    {
        PuntosMoneda += 1;
        UpdateScoreUI();
        RevisarVictoria();
    }
    public void SumarCorona()
    {
        TengoCorona = true;
        CoronaText.text = "Tienes la corona";
        RevisarVictoria();
    }
    public void SumarPunto()
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
    public void TomarLLave()
    {
        LlaveImage.SetActive(true);
    }
    void GuardarRecord()
    {
        float recordGuardado = PlayerPrefs.GetFloat(CLAVE_RECORD, 0);

        if (recordGuardado == 0 || Tiempo < recordGuardado)
        {
            PlayerPrefs.SetFloat(CLAVE_RECORD, Tiempo);
            PlayerPrefs.Save();
        }
    }
    void MostrarRecord()
    {
        if (RecordText == null) return;

        float record = PlayerPrefs.GetFloat(CLAVE_RECORD, 0);

        if (record > 0)
        {
            int min = Mathf.FloorToInt(record / 60);
            int seg = Mathf.FloorToInt(record % 60);
            RecordText.text = "Récord: " + min + ":" + seg.ToString("00");
        }
        else
        {
            RecordText.text = "Récord: --:--";
        }
    }
}

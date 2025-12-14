using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class MiArma : MonoBehaviour
{
    [SerializeField] GameObject BalaPrefab;
    [SerializeField] Transform SpawnBala;

    [SerializeField] float VelocidadBala = 200f;
    [SerializeField] float VelocidadDisparo = 0.1f;
    public int RafagaCount = 3;
    public float RafagaDelay = 0.1f;
    [SerializeField] int cargador = 9;
    [SerializeField] bool Municion = true;
    private float siguienteDisparo = 0f;

    private bool LuzON = false;
    [SerializeField] GameObject LuzLinterna;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoDisparo;
    [SerializeField] AudioClip SonidoSinBala;
    [SerializeField] AudioClip SonidoRecarga;

    [SerializeField] TextMeshProUGUI ModoArmaText;
    [SerializeField] TextMeshProUGUI CantidadBalasTexto;

    public enum ModoDeFuego { SemiAuto, RafagaAuto, FullAuto }
    public ModoDeFuego mododefuego = ModoDeFuego.SemiAuto;


    public Animator SpawnRifle;
    [SerializeField] GameObject VFXdisparo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
        CantidadBalasTexto.text = "Balas: " + cargador.ToString() + "/9";
        SpawnRifle = GetComponent<Animator>();
        LuzLinterna.SetActive(LuzON);
    }
    void Update()
    {
        Disparando();
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            CambiarModoDeFuego();
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnRifle.SetTrigger("Reload");
            Municion = true;
            cargador = 9;
            CantidadBalasTexto.text = "Balas: " + cargador.ToString() + "/9";
            if (SonidoRecarga != null && audioSource != null)
            { audioSource.PlayOneShot(SonidoRecarga); }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (LuzON == false)
            {
                LuzON = true;
                LuzLinterna.SetActive(LuzON);
            }
            else if (LuzON == true)
            {
                LuzON = false;
                LuzLinterna.SetActive(LuzON);
            }
        }
    }

    void Disparando()
    {
        //Disparo semi automatico
        if (mododefuego == ModoDeFuego.SemiAuto)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= siguienteDisparo)
            {
                if (Municion == true)
                {
                    Disparar();
                }
                else
                {
                    SinMunicion();
                }
            }
        }
        //Disparo en rafagas
        else if (mododefuego == ModoDeFuego.RafagaAuto)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= siguienteDisparo)
            {
                siguienteDisparo = Time.time + VelocidadDisparo;
                StartCoroutine(FuegoEnRafaga());
            }
        }
        //Disparo en Automatico
        else if (mododefuego == ModoDeFuego.FullAuto)
        {
            if (Input.GetButton("Fire1") && Time.time >= siguienteDisparo)
            {
                if (Municion == true)
                {
                    Disparar();
                }
                else
                {
                    SinMunicion();
                }
            }
        }
    }
    public void CambiarModoDeFuego()
    {
        mododefuego = (ModoDeFuego)(((int)mododefuego + 1) % System.Enum.GetValues(typeof(ModoDeFuego)).Length);
    }
    void Disparar()
    {
        siguienteDisparo = Time.time + VelocidadDisparo;
        GameObject newBala = Instantiate(BalaPrefab, SpawnBala.position, SpawnBala.rotation);
        Rigidbody BalaRigidbody = newBala.GetComponent<Rigidbody>();
        BalaRigidbody.AddForce(SpawnBala.forward * VelocidadBala);
        GameObject newVFX = Instantiate(VFXdisparo, SpawnBala.position, SpawnBala.rotation);
        //Sonido de la bala
        if (SonidoDisparo != null && audioSource != null)
        { audioSource.PlayOneShot(SonidoDisparo); }
        //Recarga
        cargador -= 1;
        if (cargador <= 0)
        {
            Municion = false;
        }
        CantidadBalasTexto.text = "Balas: " + cargador.ToString() + "/9";
        //Destruye la bala
        Destroy(newBala, 2f);
        Destroy(newVFX, 1f);
    }
    IEnumerator FuegoEnRafaga()
    {
        for (int i = 0; i < RafagaCount; i++)
        {
            if (Municion == true)
            {
                Disparar();
            }
            else
            {
                SinMunicion();
            }
            yield return new WaitForSeconds(RafagaDelay);
        }
    }
    void SinMunicion()
    {
        if (SonidoSinBala != null && audioSource != null)
        { audioSource.PlayOneShot(SonidoSinBala); }
    }    
    private void UpdateUI()
    {
        ModoArmaText.text = "Modo de arma: \n" + mododefuego.ToString();
    }
}

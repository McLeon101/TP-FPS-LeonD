using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class MiArma : MonoBehaviour
{
    [SerializeField] GameObject BalaPrefab;
    [SerializeField] Transform SpawnBala;
    [SerializeField] float VelocidadBala = 1000f;
    [SerializeField] float VelocidadDisparo = 0.1f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject VFXdisparo;
    public enum ModoDeFuego { SemiAuto, RafagaAuto, FullAuto }
    public ModoDeFuego mododefuego = ModoDeFuego.SemiAuto;
    private float siguienteDisparo = 0f;
    [SerializeField] TextMeshProUGUI ModoArmaText;
    public int RafagaCount = 3;
    public float RafagaDelay = 0.1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
    }
    void Update()
    {
        Disparando();
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            CambiarModoDeFuego();
            UpdateUI();
        }
    }

    void Disparando()
    {
        //Disparo semi automatico
        if (mododefuego == ModoDeFuego.SemiAuto)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= siguienteDisparo)
            {
                Disparar();
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
                Disparar();
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
        if (audioSource != null)
        {
            audioSource.Play();
        }
        //Destruye la bala
        Destroy(newBala, 2f);
        Destroy(newVFX, 1f);
    }
    IEnumerator FuegoEnRafaga()
    {
        for (int i = 0; i < RafagaCount; i++)
        {
            Disparar();
            yield return new WaitForSeconds(RafagaDelay);
        }
    }
    private void UpdateUI()
    {
        ModoArmaText.text = "Modo de arma: " + mododefuego.ToString();
    }
}

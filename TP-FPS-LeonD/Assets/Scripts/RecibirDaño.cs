using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class RecibirDaño : MonoBehaviour
{
    [SerializeField] private int Salud = 100;
    [SerializeField] private TextMeshProUGUI SaludTexto;
    [SerializeField] private Image BarraVida;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SonidoDaño;
    [SerializeField] AudioClip SonidoMuerte;


    private void Start()
    {
        UpdateUI();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
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
            if (Salud <= 0)
            {
                if (SonidoMuerte != null && audioSource != null && !audioSource.isPlaying)
                { audioSource.PlayOneShot(SonidoMuerte); }

                //Destruye objeto
                Muerte();
            }
            else
            {
                if (SonidoDaño != null && audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(SonidoDaño);
                }
            }

            //Destruye bala
            Destroy(collision.gameObject);
        }
    }
    private void UpdateUI()
    {
        SaludTexto.text = "Salud " + Salud;
        BarraVida.fillAmount = Salud / 100f;
    }
    private void Muerte()
    {
        Destroy(gameObject, 1f);
    }
}

using UnityEngine;

public class MonedaColeccionable : MonoBehaviour
{
    public string MonedasTag = "Moneda";
    public int puntosPorMoneda = 1;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip Moneda;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MonedasTag))
        {
            // Sumar puntos
            ReglasDeJuego.instance.SumarMoneda(puntosPorMoneda);

            if (audioSource != null)
            {
                audioSource.PlayOneShot(Moneda);
            }

            // Desaparecer el cubo
            Destroy(other.gameObject);
        }
    }
}
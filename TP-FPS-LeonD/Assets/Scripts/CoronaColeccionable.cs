using UnityEngine;

public class CoronaColeccionable : MonoBehaviour
{
    public string CoronaTag = "Corona";
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip Moneda;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CoronaTag))
        {
            // Sumar puntos
            ReglasDeJuego.instance.SumarCorona();

            if (audioSource != null)
            {
                audioSource.PlayOneShot(Moneda);
            }

            // Desaparecer el cubo
            Destroy(other.gameObject);
        }
    }
}

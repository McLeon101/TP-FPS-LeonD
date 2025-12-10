using StarterAssets;
using UnityEngine;

public class LLave : MonoBehaviour
{
    public string LlaveTag = "Llave";
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip llave;
    public ReglasDeJuego tomarllave;
    public PortonGigante abrirporton;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(LlaveTag))
        {
            tomarllave.TomarLLave();
            abrirporton.AbrirPorton();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(llave);
            }
            // Desaparecer el cubo
            Destroy(other.gameObject, 1f);
        }
    }
}

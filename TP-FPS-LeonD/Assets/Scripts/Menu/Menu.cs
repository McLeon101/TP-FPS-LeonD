using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip Menuu;

    public void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (Menuu != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(Menuu);
        }
    }
    public void IniciarJuego()
    {
        SceneManager.LoadScene("Juego");
    }
}

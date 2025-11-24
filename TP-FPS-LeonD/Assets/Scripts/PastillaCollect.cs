using StarterAssets;
using UnityEngine;

public class PastillaCollect : MonoBehaviour
{
    public string PastillaTag = "Pastilla";
    public FirstPersonController powerjump;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip Pastilla;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PastillaTag))
        {
            powerjump.PowerJump();

            if (audioSource != null)
            {
                audioSource.PlayOneShot(Pastilla);
            }

            // Desaparecer el cubo
            Destroy(other.gameObject, 2f);
        }
    }
}

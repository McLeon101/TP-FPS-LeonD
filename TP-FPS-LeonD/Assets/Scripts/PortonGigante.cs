using UnityEngine;

public class PortonGigante : MonoBehaviour
{
    public Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void AbrirPuerta()
    {
        _animator.SetBool("AbrirP", true);
        _animator.SetBool("CerrarP", false);
    }

    private void CerrarPuerta()
    {
        _animator.SetBool("CerrarP", true);
        _animator.SetBool("AbrirP", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbrirPuerta();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CerrarPuerta();
        }
    }

    public void AbrirPorton()
    {
        _animator.SetBool("Llave", true);
    }
}

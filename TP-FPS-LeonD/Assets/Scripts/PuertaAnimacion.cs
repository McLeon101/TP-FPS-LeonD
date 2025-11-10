using UnityEngine;

public class PuertaAnimacion : MonoBehaviour
{
    public Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void AbrirPuerta()
    {
        _animator.SetBool("Abrir", true);
        _animator.SetBool("Cerrar", false);
    }

    private void CerrarPuerta()
    {
        _animator.SetBool("Cerrar", true);
        _animator.SetBool("Abrir", false);
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
}

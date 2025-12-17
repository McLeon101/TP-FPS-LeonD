using UnityEngine;

public class AreaJefe : MonoBehaviour
{
    public JefeEnemigo enemigo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemigo.ActivarPersecucion();
        }
    }
}

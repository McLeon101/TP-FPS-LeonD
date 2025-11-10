using TMPro;
using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VidaPlayer : MonoBehaviour
{
    [SerializeField] private int Salud = 100;
    [SerializeField] private TextMeshProUGUI SaludTexto;
    [SerializeField] private Image BarraVida;
    private ReglasDeJuego reglasdejuego;

    private void Start()
    {
        UpdateUI();
        reglasdejuego = FindFirstObjectByType<ReglasDeJuego>();
    }

    public void AtaqueZombie(int amount)
    {
        Salud -= amount;
        UpdateUI();
        if (Salud <= 0)
        {
            reglasdejuego.Perder();
        }
    }

    private void UpdateUI()
    {
        SaludTexto.text = "Salud " + Salud;
        BarraVida.fillAmount = Salud / 100f;
    }
}


using UnityEngine;
using TMPro;

public class Fps_controller : MonoBehaviour
{
    [SerializeField] private TMP_Text textoFPS;

    private float tiempoAcumulado;
    private int cantidadFotogramas;

    private void Update()
    {
        cantidadFotogramas++;
        tiempoAcumulado += Time.unscaledDeltaTime;

        if (tiempoAcumulado >= 0.5f)
        {
            float fps = cantidadFotogramas / tiempoAcumulado;

            if (textoFPS != null)
                textoFPS.text = "FPS: " + Mathf.RoundToInt(fps);

            cantidadFotogramas = 0;
            tiempoAcumulado = 0f;
        }
    }
}


using UnityEngine;
using UnityEngine.UI;
public class Inventario_hud : MonoBehaviour
{
    [Header("Contornos de las ranuras")]
    [SerializeField] private Outline[] contornosRanuras;

    [Header("Seleccion")]
    [SerializeField] private int indiceSeleccionado = 0;

    private void Start()
    {
        ActualizarResaltado();
    }

    private void Update()
    {
        float desplazamiento = Input.mouseScrollDelta.y;

        if (desplazamiento > 0f)
        {
            CambiarSeleccion(1);
        }
        else if (desplazamiento < 0f)
        {
            CambiarSeleccion(-1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SeleccionarRanura(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SeleccionarRanura(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SeleccionarRanura(2);
        }
    }

    private void CambiarSeleccion(int direccion)
    {
        if (contornosRanuras.Length == 0) return;

        indiceSeleccionado += direccion;

        if (indiceSeleccionado >= contornosRanuras.Length)
        {
            indiceSeleccionado = 0;
        }

        if (indiceSeleccionado < 0)
        {
            indiceSeleccionado = contornosRanuras.Length - 1;
        }

        ActualizarResaltado();
    }

    private void SeleccionarRanura(int indice)
    {
        if (indice < 0 || indice >= contornosRanuras.Length)
            return;

        indiceSeleccionado = indice;

        ActualizarResaltado();
    }

    private void ActualizarResaltado()
    {
        for (int i = 0; i < contornosRanuras.Length; i++)
        {
            if (contornosRanuras[i] == null) continue;

            contornosRanuras[i].enabled = (i == indiceSeleccionado);
        }
    }
}

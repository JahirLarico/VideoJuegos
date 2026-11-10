
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Inventario_hud : MonoBehaviour
{
    [Header("Contornos de las ranuras")]
    [SerializeField] private Outline[] contornosRanuras;

    [Header("Nombres de los objetos")]
    [SerializeField]
    private string[] nombresObjetos =
    {
        "Llave",
        "Telefono",
        "Ayuda"
    };

    [Header("Texto del HUD")]
    [SerializeField] private TMP_Text textoNombreObjeto;
    [SerializeField] private float duracionMostrarNombre = 2f;

    [Header("Seleccion")]
    [SerializeField] private int indiceSeleccionado = 0;

    [Header("Uso de objetos")]
    [SerializeField] private KeyCode teclaUsarObjeto = KeyCode.Q;

    [SerializeField]
    private UnityEvent[] accionesPorRanura = new UnityEvent[3];

    private float tiempoRestante;

    private void Start()
    {
        ActualizarResaltado();
        MostrarNombreObjeto();
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

        // Ejecutar la accion del objeto seleccionado.
        if (Input.GetKeyDown(teclaUsarObjeto))
        {
            UsarObjetoSeleccionado();
        }

        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0f && textoNombreObjeto != null)
            {
                textoNombreObjeto.gameObject.SetActive(false);
            }
        }
    }

    private void UsarObjetoSeleccionado()
    {
        if (accionesPorRanura == null)
            return;

        if (indiceSeleccionado < 0 ||
            indiceSeleccionado >= accionesPorRanura.Length)
            return;

        if (accionesPorRanura[indiceSeleccionado] == null)
            return;

        accionesPorRanura[indiceSeleccionado].Invoke();
    }

    private void CambiarSeleccion(int direccion)
    {
        if (contornosRanuras == null || contornosRanuras.Length == 0)
            return;

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
        MostrarNombreObjeto();
    }

    private void SeleccionarRanura(int indice)
    {
        if (contornosRanuras == null)
            return;

        if (indice < 0 || indice >= contornosRanuras.Length)
            return;

        indiceSeleccionado = indice;

        ActualizarResaltado();
        MostrarNombreObjeto();
    }

    private void ActualizarResaltado()
    {
        if (contornosRanuras == null)
            return;

        for (int i = 0; i < contornosRanuras.Length; i++)
        {
            if (contornosRanuras[i] == null)
                continue;

            contornosRanuras[i].enabled = (i == indiceSeleccionado);
        }
    }

    private void MostrarNombreObjeto()
    {
        if (textoNombreObjeto == null)
            return;

        if (indiceSeleccionado >= nombresObjetos.Length)
            return;

        textoNombreObjeto.text = nombresObjetos[indiceSeleccionado];

        textoNombreObjeto.gameObject.SetActive(true);

        tiempoRestante = duracionMostrarNombre;
    }
}

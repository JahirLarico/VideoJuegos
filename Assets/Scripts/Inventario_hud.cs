
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Inventario_hud : MonoBehaviour
{
    [Header("Contornos de las ranuras")]
    [SerializeField] private Outline[] contornosRanuras;

    [Header("Iconos de los objetos")]
    [SerializeField] private Image[] iconosObjetos;

    [Header("Ranura de la pildora")]
    [SerializeField] private int indiceRanuraPildora = 2;

    [Header("Nombres de los objetos")]
    [SerializeField]
    private string[] nombresObjetos =
    {
        "Llave",
        "Telefono",
        "Pildora"
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

    private bool[] ranurasDesbloqueadas;
    private float tiempoRestante;

    private void Awake()
    {
        int cantidadRanuras = contornosRanuras != null
            ? contornosRanuras.Length
            : 3;

        ranurasDesbloqueadas = new bool[cantidadRanuras];

        for (int i = 0; i < ranurasDesbloqueadas.Length; i++)
        {
            ranurasDesbloqueadas[i] = true;
        }

        BloquearRanuraPildora();
    }

    private void Start()
    {
        ActualizarResaltado();
        MostrarNombreObjeto();
    }

    private void Update()
    {
        if (Dialogo_controller.ControlesBloqueados)
            return;

        float desplazamiento = Input.mouseScrollDelta.y;

        if (desplazamiento > 0f)
            CambiarSeleccion(1);
        else if (desplazamiento < 0f)
            CambiarSeleccion(-1);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SeleccionarRanura(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SeleccionarRanura(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SeleccionarRanura(2);

        if (Input.GetKeyDown(teclaUsarObjeto))
            UsarObjetoSeleccionado();

        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0f && textoNombreObjeto != null)
                textoNombreObjeto.gameObject.SetActive(false);
        }
    }

    private void BloquearRanuraPildora()
    {
        if (indiceRanuraPildora < 0 ||
            indiceRanuraPildora >= ranurasDesbloqueadas.Length)
            return;

        ranurasDesbloqueadas[indiceRanuraPildora] = false;

        ActualizarIconoPildora(false);
    }

    public void DesbloquearPildora()
    {
        if (indiceRanuraPildora < 0 ||
            indiceRanuraPildora >= ranurasDesbloqueadas.Length)
            return;

        if (ranurasDesbloqueadas[indiceRanuraPildora])
            return;

        ranurasDesbloqueadas[indiceRanuraPildora] = true;

        ActualizarIconoPildora(true);

        if (indiceSeleccionado == indiceRanuraPildora)
            MostrarNombreObjeto();
    }

    private void ActualizarIconoPildora(bool mostrar)
    {
        if (iconosObjetos == null ||
            indiceRanuraPildora >= iconosObjetos.Length ||
            iconosObjetos[indiceRanuraPildora] == null)
        {
            Debug.LogWarning("No se ha asignado el icono de la pildora en Inventario_hud.");
            return;
        }

        iconosObjetos[indiceRanuraPildora].gameObject.SetActive(mostrar);
    }

    private void UsarObjetoSeleccionado()
    {
        if (ranurasDesbloqueadas == null)
            return;

        if (indiceSeleccionado < 0 ||
            indiceSeleccionado >= ranurasDesbloqueadas.Length)
            return;

        if (!ranurasDesbloqueadas[indiceSeleccionado])
            return;

        if (accionesPorRanura == null ||
            indiceSeleccionado >= accionesPorRanura.Length)
            return;

        if (accionesPorRanura[indiceSeleccionado] == null)
            return;

        accionesPorRanura[indiceSeleccionado].Invoke();
    }

    private void CambiarSeleccion(int direccion)
    {
        if (contornosRanuras == null ||
            contornosRanuras.Length == 0)
            return;

        indiceSeleccionado += direccion;

        if (indiceSeleccionado >= contornosRanuras.Length)
            indiceSeleccionado = 0;

        if (indiceSeleccionado < 0)
            indiceSeleccionado = contornosRanuras.Length - 1;

        ActualizarResaltado();
        MostrarNombreObjeto();
    }

    private void SeleccionarRanura(int indice)
    {
        if (contornosRanuras == null)
            return;

        if (indice < 0 ||
            indice >= contornosRanuras.Length)
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
        if (textoNombreObjeto == null ||
            ranurasDesbloqueadas == null)
            return;

        if (indiceSeleccionado < 0 ||
            indiceSeleccionado >= ranurasDesbloqueadas.Length)
            return;

        if (!ranurasDesbloqueadas[indiceSeleccionado])
        {
            textoNombreObjeto.gameObject.SetActive(false);
            return;
        }

        if (nombresObjetos == null ||
            indiceSeleccionado >= nombresObjetos.Length)
            return;

        textoNombreObjeto.text = nombresObjetos[indiceSeleccionado];

        textoNombreObjeto.gameObject.SetActive(true);

        tiempoRestante = duracionMostrarNombre;
    }
}

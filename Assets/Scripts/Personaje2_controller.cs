
using System.Collections;
using UnityEngine;
using TMPro;
public class Personaje2_controller : MonoBehaviour
{
    [Header("Deteccion del jugador")]
    [SerializeField] private string tagJugador = "Player";

    [Header("Interaccion")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;
    [SerializeField] private TMP_Text textoInteraccion;

    [Header("Sistema de dialogos")]
    [SerializeField] private Dialogo_controller sistemaDialogos;

    [Header("Dialogo del personaje")]
    [SerializeField] private string nombrePersonaje = "Desconocido";

    [TextArea(2, 5)]
    [SerializeField] private string[] mensajes;

    [Header("Desbloqueo de la pildora")]
    [SerializeField] private Inventario_hud inventario;

    private bool jugadorCerca;
    private bool dialogoIniciado;
    private bool conversacionFinalizada;

    private void Start()
    {
        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!jugadorCerca)
            return;

        if (dialogoIniciado || conversacionFinalizada)
            return;

        if (Dialogo_controller.ControlesBloqueados)
            return;

        if (Input.GetKeyDown(teclaInteraccion))
        {
            IniciarConversacion();
        }
    }

    private void IniciarConversacion()
    {
        if (sistemaDialogos == null)
        {
            Debug.LogError("No hay Dialogo_controller asignado.");
            return;
        }

        if (mensajes == null || mensajes.Length == 0)
            return;

        LineaDialogo[] nuevosDialogos =
            new LineaDialogo[mensajes.Length];

        for (int i = 0; i < mensajes.Length; i++)
        {
            nuevosDialogos[i] = new LineaDialogo
            {
                posicion = 2,
                nombre = nombrePersonaje,
                texto = mensajes[i]
            };
        }

        bool iniciado =
            sistemaDialogos.IniciarDialogo(nuevosDialogos);

        if (!iniciado)
            return;

        dialogoIniciado = true;

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);

        StartCoroutine(EsperarFinalDialogo());
    }

    private IEnumerator EsperarFinalDialogo()
    {
        while (sistemaDialogos.DialogoActivo)
        {
            yield return null;
        }

        dialogoIniciado = false;
        conversacionFinalizada = true;

        if (inventario != null)
        {
            inventario.DesbloquearPildora();
        }
        else
        {
            Debug.LogWarning("No hay Inventario_hud asignado.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador))
            return;

        jugadorCerca = true;

        if (!dialogoIniciado && !conversacionFinalizada &&
            textoInteraccion != null)
        {
            textoInteraccion.text = "(E) Hablar";
            textoInteraccion.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tagJugador))
            return;

        jugadorCerca = false;

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }
}

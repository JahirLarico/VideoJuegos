
using System;
using System.Collections;
using UnityEngine;
using TMPro;

[Serializable]
public class ConversacionTelefono
{
    [Header("Identificacion")]
    public string nombreConversacion;

    [Header("Estado")]
    public bool desbloqueada;

    [Header("Dialogos")]
    public LineaDialogo[] dialogos;
}

public class Telefono_controller : MonoBehaviour
{
    [Header("Sistema de dialogos")]
    [SerializeField] private Dialogo_controller sistemaDialogos;

    [Header("Conversaciones")]
    [SerializeField] private ConversacionTelefono[] conversaciones;

    [Header("Mensaje de bloqueo")]
    [SerializeField] private TMP_Text textoMensaje;

    [SerializeField] private float duracionMensaje = 2.5f;

    [TextArea]
    [SerializeField]
    private string mensajeBloqueado =
        "Aun no deberia usar el telefono.";

    [SerializeField]
    private string mensajeSinConversaciones =
        "No hay nada nuevo en el telefono.";

    private int indiceConversacion;

    private Coroutine corrutinaMensaje;

    private void Start()
    {
        if (textoMensaje != null)
            textoMensaje.gameObject.SetActive(false);
    }

    public void UsarTelefono()
    {

        if (sistemaDialogos == null)
            return;

        if (sistemaDialogos.DialogoActivo)
            return;

        if (conversaciones == null ||
            conversaciones.Length == 0)
            return;

        if (indiceConversacion >= conversaciones.Length)
        {
            MostrarMensaje(mensajeSinConversaciones);
            return;
        }

        ConversacionTelefono conversacion =
            conversaciones[indiceConversacion];

        if (!conversacion.desbloqueada)
        {
            MostrarMensaje(mensajeBloqueado);
            return;
        }

        if (conversacion.dialogos == null ||
            conversacion.dialogos.Length == 0)
            return;

        bool iniciado = sistemaDialogos.IniciarDialogo(
            conversacion.dialogos
        );

        if (iniciado)
            indiceConversacion++;
    }

    public void DesbloquearConversacion(int indice)
    {
        if (indice < 0 || indice >= conversaciones.Length)
            return;

        conversaciones[indice].desbloqueada = true;
    }

    public void BloquearConversacion(int indice)
    {
        if (indice < 0 || indice >= conversaciones.Length)
            return;

        conversaciones[indice].desbloqueada = false;
    }

    private void MostrarMensaje(string mensaje)
    {
        if (textoMensaje == null)
            return;

        if (corrutinaMensaje != null)
            StopCoroutine(corrutinaMensaje);

        corrutinaMensaje = StartCoroutine(
            MostrarMensajeTemporal(mensaje)
        );
    }

    private IEnumerator MostrarMensajeTemporal(string mensaje)
    {
        textoMensaje.text = mensaje;
        textoMensaje.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(duracionMensaje);

        textoMensaje.gameObject.SetActive(false);
        corrutinaMensaje = null;
    }
}

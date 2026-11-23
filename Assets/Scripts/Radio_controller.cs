
using UnityEngine;
using TMPro;

public class Radio_controller : MonoBehaviour
{
    [Header("Deteccion de la radio")]
    [SerializeField] private Camera camara;
    [SerializeField] private float distanciaMaxima = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource reproductorAudio;

    [Header("Interfaz")]
    [SerializeField] private TMP_Text textoInteraccion;

    [Header("Sistema de dialogos")]
    [SerializeField] private Dialogo_controller sistemaDialogos;

    [Header("Mensaje de la grabacion")]
    [SerializeField] private float segundoMensaje = 5f;

    [TextArea(2, 5)]
    [SerializeField]
    private string mensaje =
        "Si estas escuchando esto, significa que algo salio mal...";

    private bool mirandoRadio;
    private bool reproduccionIniciada;
    private bool audioPausado;
    private bool mensajeMostrado;
    private bool grabacionFinalizada;

    private void Awake()
    {
        if (reproductorAudio != null)
            reproductorAudio.playOnAwake = false;
    }

    private void Start()
    {
        if (camara == null)
            camara = Camera.main;

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }

    private void Update()
    {
        DetectarRadio();
        ActualizarGrabacion();

        if (mirandoRadio && Input.GetKeyDown(KeyCode.E))
            Interactuar();
    }

    private void DetectarRadio()
    {
        mirandoRadio = false;

        if (camara == null)
            return;

        Ray rayo = camara.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            rayo,
            out RaycastHit impacto,
            distanciaMaxima))
        {
            Radio_controller radioDetectada =
                impacto.collider.GetComponentInParent<Radio_controller>();

            if (radioDetectada == this)
                mirandoRadio = true;
        }

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoInteraccion == null)
            return;

        bool mostrar = mirandoRadio && !grabacionFinalizada;

        textoInteraccion.gameObject.SetActive(mostrar);

        if (!mostrar)
            return;

        if (!reproduccionIniciada)
            textoInteraccion.text = "(E) Reproducir";
        else if (audioPausado)
            textoInteraccion.text = "(E) Continuar";
        else
            textoInteraccion.text = "(E) Detener";
    }

    private void Interactuar()
    {
        if (grabacionFinalizada)
            return;

        if (reproductorAudio == null ||
            reproductorAudio.clip == null)
        {
            Debug.LogError("La radio no tiene un AudioClip asignado.");
            return;
        }

        if (!reproduccionIniciada)
        {
            reproduccionIniciada = true;
            audioPausado = false;

            reproductorAudio.time = 0f;
            reproductorAudio.Play();
        }
        else if (!audioPausado)
        {
            reproductorAudio.Pause();
            audioPausado = true;
        }
        else
        {
            reproductorAudio.UnPause();
            audioPausado = false;
        }

        ActualizarTexto();
    }

    private void ActualizarGrabacion()
    {
        if (!reproduccionIniciada || grabacionFinalizada)
            return;

        if (reproductorAudio == null ||
            reproductorAudio.clip == null)
            return;

        if (!mensajeMostrado &&
            reproductorAudio.isPlaying &&
            reproductorAudio.time >= segundoMensaje)
        {
            MostrarMensaje();
        }

        if (!reproductorAudio.isPlaying && !audioPausado)
        {
            FinalizarGrabacion();
        }
    }

    private void MostrarMensaje()
    {
        if (sistemaDialogos == null)
        {
            Debug.LogError("No hay Dialogo_controller asignado.");
            return;
        }

        if (sistemaDialogos.DialogoActivo)
            return;

        LineaDialogo[] linea = new LineaDialogo[1];

        linea[0] = new LineaDialogo
        {
            posicion = 1,
            nombre = "Yo",
            texto = mensaje
        };

        bool iniciado = sistemaDialogos.IniciarDialogo(linea);

        if (iniciado)
            mensajeMostrado = true;
    }

    private void FinalizarGrabacion()
    {
        grabacionFinalizada = true;
        reproductorAudio.Stop();

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }
}

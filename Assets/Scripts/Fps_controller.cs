
using UnityEngine;
using TMPro;
using Unity.Profiling;

public class Fps_controller : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] private TMP_Text textoFPS;
    [SerializeField] private TMP_Text textoCPU;
    [SerializeField] private TMP_Text textoGPU;

    [Header("FPS")]
    [SerializeField] private float intervaloActualizacion = 0.5f;

    [Header("CPU")]
    [SerializeField] private float intervaloCPU = 1.5f;

    [Header("GPU")]
    [SerializeField] private float intervaloGPU = 1.5f;

    private float tiempoAcumulado;
    private int cantidadFotogramas;

    private float tiempoCPU;
    private float tiempoGPU;

    private float tiempoCPUms;
    private float tiempoGPUms;

    private ProfilerRecorder recorderCPU;
    private ProfilerRecorder recorderGPU;

    private void Start()
    {
        recorderCPU = ProfilerRecorder.StartNew(
            ProfilerCategory.Internal,
            "Main Thread",
            15
        );

        recorderGPU = ProfilerRecorder.StartNew(
            ProfilerCategory.Render,
            "GPU Frame Time",
            15
        );
    }

    private void Update()
    {
        ActualizarFPS();
        ActualizarCPU();
        ActualizarGPU();
    }

    private void ActualizarFPS()
    {
        cantidadFotogramas++;
        tiempoAcumulado += Time.unscaledDeltaTime;

        if (tiempoAcumulado >= intervaloActualizacion)
        {
            float fpsReales = cantidadFotogramas / tiempoAcumulado;

            if (textoFPS != null)
                textoFPS.text = "FPS: " + Mathf.RoundToInt(fpsReales);

            cantidadFotogramas = 0;
            tiempoAcumulado = 0f;
        }
    }

    private void ActualizarCPU()
    {
        if (textoCPU == null)
            return;

        tiempoCPU += Time.unscaledDeltaTime;

        if (tiempoCPU >= intervaloCPU)
        {
            if (recorderCPU.Valid && recorderCPU.LastValue > 0)
            {
                tiempoCPUms = recorderCPU.LastValue * 0.000001f;
            }

            tiempoCPU = 0f;
        }

        textoCPU.text = recorderCPU.Valid && tiempoCPUms > 0f
            ? "CPU: " + tiempoCPUms.ToString("F2") + " ms"
            : "CPU: No disponible";
    }

    private void ActualizarGPU()
    {
        if (textoGPU == null)
            return;

        tiempoGPU += Time.unscaledDeltaTime;

        if (tiempoGPU >= intervaloGPU)
        {
            if (recorderGPU.Valid && recorderGPU.LastValue > 0)
            {
                tiempoGPUms = recorderGPU.LastValue * 0.000001f;
            }

            tiempoGPU = 0f;
        }

        textoGPU.text = recorderGPU.Valid && tiempoGPUms > 0f
            ? "GPU: " + tiempoGPUms.ToString("F2") + " ms"
            : "GPU: No disponible";
    }

    private void OnValidate()
    {
        intervaloCPU = Mathf.Max(0.05f, intervaloCPU);
        intervaloGPU = Mathf.Max(0.05f, intervaloGPU);
        intervaloActualizacion = Mathf.Max(0.05f, intervaloActualizacion);
    }

    private void OnDestroy()
    {
        if (recorderCPU.Valid)
            recorderCPU.Dispose();

        if (recorderGPU.Valid)
            recorderGPU.Dispose();
    }
}

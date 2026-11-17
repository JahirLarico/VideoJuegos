
using UnityEngine;

public class Pildoras_controller : MonoBehaviour
{
    [Header("Configuracion del jugador")]
    [SerializeField] private string tagJugador = "Player";

    [Header("Configuracion de las nubes")]
    [SerializeField] private Renderer renderizadorNubes;

    [SerializeField, Range(0f, 1f)]
    private float intensidadArcoirisObjetivo = 1f;

    [Header("Configuracion de la luz principal")]
    [SerializeField] private Light luzEscena;

    [SerializeField] private Color colorLuzObjetivo = Color.white;
    [SerializeField] private float intensidadLuzObjetivo = 0.2f;

    [SerializeField] private float rangoLuzObjetivo = 10f;

    [SerializeField, Range(1f, 179f)]
    private float anguloSpotObjetivo = 60f;

    [Header("Iluminacion ambiental")]
    [SerializeField]
    private Color colorAmbientalObjetivo =
        new Color(0.06f, 0.08f, 0.15f);

    [Header("Skybox")]
    [SerializeField] private bool modificarExposicionSkybox = false;
    [SerializeField] private float exposicionSkyboxObjetivo = 0.3f;

    [Header("Transicion de los efectos")]
    [SerializeField] private float duracionTransicion = 4f;

    [Header("Recogida de la pildora")]
    [SerializeField] private bool destruirAlRecoger = true;

    private Material materialNubes;

    private float intensidadArcoirisInicial;
    private float intensidadArcoirisActual;

    private Color colorLuzInicial;
    private float intensidadLuzInicial;
    private float rangoLuzInicial;
    private float anguloSpotInicial;

    private Color colorAmbientalInicial;

    private Material skyboxMaterial;
    private float exposicionSkyboxInicial;

    private bool efectoActivado = false;

    private float tiempoInicio;

    private void Start()
    {
        if (renderizadorNubes != null)
        {
            materialNubes = renderizadorNubes.material;

            intensidadArcoirisActual =
                materialNubes.GetFloat("_Brillo_arcoiris");

            intensidadArcoirisInicial =
                intensidadArcoirisActual;
        }

        if (luzEscena != null)
        {
            colorLuzInicial = luzEscena.color;
            intensidadLuzInicial = luzEscena.intensity;
            rangoLuzInicial = luzEscena.range;
            anguloSpotInicial = luzEscena.spotAngle;
        }

        colorAmbientalInicial = RenderSettings.ambientLight;

        if (RenderSettings.skybox != null)
        {
            skyboxMaterial = new Material(RenderSettings.skybox);

            if (skyboxMaterial.HasProperty("_Exposure"))
            {
                exposicionSkyboxInicial =
                    skyboxMaterial.GetFloat("_Exposure");
            }
        }
    }

    private void Update()
    {
        if (!efectoActivado)
            return;

        float progreso = ObtenerProgreso();

        if (materialNubes != null)
        {
            intensidadArcoirisActual = Mathf.Lerp(
                intensidadArcoirisInicial,
                intensidadArcoirisObjetivo,
                progreso
            );

            materialNubes.SetFloat(
                "_Brillo_arcoiris",
                intensidadArcoirisActual
            );
        }

        if (luzEscena != null)
        {
            luzEscena.color = Color.Lerp(
                colorLuzInicial,
                colorLuzObjetivo,
                progreso
            );

            luzEscena.intensity = Mathf.Lerp(
                intensidadLuzInicial,
                intensidadLuzObjetivo,
                progreso
            );

            if (luzEscena.type == LightType.Point ||
                luzEscena.type == LightType.Spot)
            {
                luzEscena.range = Mathf.Lerp(
                    rangoLuzInicial,
                    rangoLuzObjetivo,
                    progreso
                );
            }

            if (luzEscena.type == LightType.Spot)
            {
                luzEscena.spotAngle = Mathf.Lerp(
                    anguloSpotInicial,
                    anguloSpotObjetivo,
                    progreso
                );
            }
        }

        RenderSettings.ambientLight = Color.Lerp(
            colorAmbientalInicial,
            colorAmbientalObjetivo,
            progreso
        );

        if (modificarExposicionSkybox &&
            skyboxMaterial != null &&
            RenderSettings.skybox.HasProperty("_Exposure"))
        {
            RenderSettings.skybox.SetFloat(
                "_Exposure",
                Mathf.Lerp(
                    exposicionSkyboxInicial,
                    exposicionSkyboxObjetivo,
                    progreso
                )
            );
        }

        DynamicGI.UpdateEnvironment();
    }

    private float ObtenerProgreso()
    {
        if (duracionTransicion <= 0f)
            return 1f;

        return Mathf.Clamp01(
            (Time.time - tiempoInicio) / duracionTransicion
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (efectoActivado)
            return;

        if (!other.CompareTag(tagJugador))
            return;

        ActivarEfecto();
    }

    public void ActivarEfecto()
    {
        if (efectoActivado)
            return;

        efectoActivado = true;

        tiempoInicio = Time.time;

        if (destruirAlRecoger)
        {
            Destroy(gameObject);
        }
    }
}

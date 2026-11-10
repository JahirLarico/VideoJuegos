using UnityEngine;

public class Pildoras_controller : MonoBehaviour
{
    [Header("Configuracion del jugador")]
    [SerializeField] private string tagJugador = "Player";

    [Header("Configuracion de las nubes")]
    [SerializeField] private Renderer renderizadorNubes;

    [SerializeField, Range(0f, 1f)]
    private float intensidadArcoirisObjetivo = 1f;

    [Header("Configuracion de la luz")]
    [SerializeField] private Light luzEscena;

    [SerializeField] private Color colorLuzObjetivo = Color.white;

    [SerializeField] private float intensidadLuzObjetivo = 2f;

    [SerializeField] private float rangoLuzObjetivo = 10f;

    [SerializeField, Range(1f, 179f)]
    private float anguloSpotObjetivo = 60f;

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

    private bool efectoActivado = false;

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
    }

    private void Update()
    {
        if (!efectoActivado)
            return;

        float velocidad = Time.deltaTime /
            Mathf.Max(0.01f, duracionTransicion);

        if (materialNubes != null)
        {
            intensidadArcoirisActual = Mathf.MoveTowards(
                intensidadArcoirisActual,
                intensidadArcoirisObjetivo,
                velocidad
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
                ObtenerProgreso()
            );

            luzEscena.intensity = Mathf.Lerp(
                intensidadLuzInicial,
                intensidadLuzObjetivo,
                ObtenerProgreso()
            );

            if (luzEscena.type == LightType.Point ||
                luzEscena.type == LightType.Spot)
            {
                luzEscena.range = Mathf.Lerp(
                    rangoLuzInicial,
                    rangoLuzObjetivo,
                    ObtenerProgreso()
                );
            }

            if (luzEscena.type == LightType.Spot)
            {
                luzEscena.spotAngle = Mathf.Lerp(
                    anguloSpotInicial,
                    anguloSpotObjetivo,
                    ObtenerProgreso()
                );
            }
        }
    }

    private float ObtenerProgreso()
    {
        if (duracionTransicion <= 0f)
            return 1f;

        return Mathf.Clamp01(
            (Time.time - tiempoInicio) / duracionTransicion
        );
    }

    private float tiempoInicio;

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
        efectoActivado = true;

        tiempoInicio = Time.time;

        if (destruirAlRecoger)
        {
            Destroy(gameObject);
        }
    }
}

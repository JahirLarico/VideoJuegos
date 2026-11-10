
using UnityEngine;

public class Interuptor_controller : MonoBehaviour
{
    [Header("Configuracion del interruptor")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;
    [SerializeField] private string tagJugador = "Player";
    [SerializeField] private float rangoInteraccion = 3f;

    [Header("Objeto que queremos controlar")]
    [SerializeField] private GameObject objetoControlado;

    [Header("Estado inicial")]
    [SerializeField] private bool empiezaEncendido = false;

    [Header("Configuracion de colores de las nubes")]
    [SerializeField] private Renderer renderizadorNubes;
    [SerializeField] private float duracionCambioColor = 4f;
    [SerializeField, Range(0f, 1f)] private float intensidadArcoiris = 0f;

    private bool jugadorCerca = false;
    private bool estaEncendido = false;

    private float intensidadObjetivo = 0f;
    private Material materialNubes;

    private void Start()
    {
        estaEncendido = empiezaEncendido;

        if (objetoControlado != null)
        {
            objetoControlado.SetActive(estaEncendido);
        }

        if (renderizadorNubes != null)
        {
            materialNubes = renderizadorNubes.material;
            ActualizarColorNubes();
        }
    }

    private void Update()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag(tagJugador);

        if (jugador == null)
            return;

        Vector3 direccionJugador = jugador.transform.position - transform.position;

        bool estaDelante =
            Vector3.Dot(transform.forward, direccionJugador.normalized) > 0;

        float distancia = direccionJugador.magnitude;

        jugadorCerca = estaDelante && distancia <= rangoInteraccion;

        if (jugadorCerca && Input.GetKeyDown(teclaInteraccion))
        {
            CambiarEstado();
        }

        intensidadArcoiris = Mathf.MoveTowards(
            intensidadArcoiris,
            intensidadObjetivo,
            Time.deltaTime / Mathf.Max(0.01f, duracionCambioColor)
        );

        ActualizarColorNubes();
    }

    private void CambiarEstado()
    {
        if (objetoControlado != null)
        {
            estaEncendido = !estaEncendido;
            objetoControlado.SetActive(estaEncendido);
        }

        intensidadObjetivo = intensidadObjetivo == 0f ? 1f : 0f;

    }

    private void ActualizarColorNubes()
    {
        if (materialNubes == null)
            return;

        materialNubes.SetFloat(
            "_Brillo_arcoiris",
            intensidadArcoiris
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}

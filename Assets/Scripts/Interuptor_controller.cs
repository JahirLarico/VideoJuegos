using UnityEngine;

public class Interuptor_controller : MonoBehaviour
{

    // Pre_lab12 Backup

    [Header("Configuracion del interruptor")]

    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;

    [SerializeField] private string tagJugador = "Player";

    [SerializeField] private float rangoInteraccion = 3f;


    [Header("Objeto que queremos controlar")]

    [SerializeField] private GameObject objetoControlado;


    [Header("Estado inicial")]

    [SerializeField] private bool empiezaEncendido = false;


    private bool jugadorCerca = false;

    private bool estaEncendido = false;


    private void Start()
    {
        estaEncendido = empiezaEncendido;

        if (objetoControlado != null)
        {
            objetoControlado.SetActive(estaEncendido);
        }
        else
        {
            Debug.LogWarning("No has asignado el objeto que quieres controlar.");
        }
    }


    private void Update()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag(tagJugador);

        if (jugador == null)
            return;

        Vector3 direccionJugador = jugador.transform.position - transform.position;

        bool estaDelante = Vector3.Dot(transform.forward, direccionJugador.normalized) > 0;

        float distancia = direccionJugador.magnitude;

        jugadorCerca = estaDelante && distancia <= rangoInteraccion;

        if (jugadorCerca && Input.GetKeyDown(teclaInteraccion))
        {
            CambiarEstado();
        }
    }


    private void CambiarEstado()
    {
        if (objetoControlado == null)
            return;

        estaEncendido = !estaEncendido;

        objetoControlado.SetActive(estaEncendido);

        Debug.Log("Interruptor: " + (estaEncendido ? "Encendido" : "Apagado"));
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

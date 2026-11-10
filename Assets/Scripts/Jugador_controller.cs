using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Jugador_controller : MonoBehaviour
{
    public float velocidad = 5f;
    public float gravedad = -9.81f;
    public float fuerzaSalto = 2f;

    private CharacterController controlador;
    private Vector3 velocidadVertical;
    private Transform camara;

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            camara = Camera.main.transform;
        }
    }

    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 adelante = camara != null
            ? camara.forward
            : transform.forward;

        Vector3 derecha = camara != null
            ? camara.right
            : transform.right;

        adelante.y = 0f;
        derecha.y = 0f;

        adelante.Normalize();
        derecha.Normalize();

        Vector3 movimiento =
            derecha * horizontal + adelante * vertical;

        movimiento = Vector3.ClampMagnitude(movimiento, 1f);

        if (controlador.isGrounded && velocidadVertical.y < 0f)
        {
            velocidadVertical.y = -2f;
        }

        if (controlador.isGrounded &&
            Input.GetKeyDown(KeyCode.Space))
        {
            velocidadVertical.y =
                Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        velocidadVertical.y += gravedad * Time.deltaTime;

        Vector3 movimientoFinal =
            movimiento * velocidad + velocidadVertical;

        controlador.Move(movimientoFinal * Time.deltaTime);
    }
}
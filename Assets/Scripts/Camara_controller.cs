using UnityEngine;

public class Camara_controller : MonoBehaviour
{
    public float sensibilidad = 300f;
    public float limiteArriba = 90f;
    public float limiteAbajo = -90f;

    private float rotacionVertical = 0f;
    private float rotacionHorizontal = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotacionVertical = transform.localEulerAngles.x;

        if (rotacionVertical > 180f)
            rotacionVertical -= 360f;

        rotacionHorizontal = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensibilidad * Time.deltaTime;

        rotacionHorizontal += mouseX;
        rotacionVertical -= mouseY;

        rotacionVertical = Mathf.Clamp(
            rotacionVertical,
            limiteAbajo,
            limiteArriba
        );

        transform.localRotation = Quaternion.Euler(
            rotacionVertical,
            rotacionHorizontal,
            0f
        );
    }
}

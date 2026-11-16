using UnityEngine;

public class Luz_calle_controller : MonoBehaviour
{
    [Header("Luz principal del escenario")]
    [SerializeField] private Light luzPrincipal;

    [Header("Configuracion dia y noche")]
    [SerializeField] private float intensidadUmbralNoche = 0.8f;

    [Header("Luz del farol")]
    [SerializeField] private Light luzFarol;

    private void Start()
    {
        if (luzFarol == null)
            luzFarol = GetComponentInChildren<Light>();

        ActualizarFarol();
    }

    private void Update()
    {
        ActualizarFarol();
    }

    private void ActualizarFarol()
    {
        if (luzPrincipal == null || luzFarol == null)
            return;

        bool esDeNoche =
            luzPrincipal.intensity <= intensidadUmbralNoche;

        luzFarol.enabled = esDeNoche;
    }
}

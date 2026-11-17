
using System;
using System.Collections;
using UnityEngine;
using TMPro;

[Serializable]
public class LineaDialogo
{
    [Header("Informacion del dialogo")]
    [Range(1, 2)]
    public int posicion = 1;

    public string nombre;

    [TextArea(2, 5)]
    public string texto;
}

public class Dialogo_controller : MonoBehaviour
{
    [Header("Panel del dialogo")]
    [SerializeField] private GameObject panelDialogo;

    [Header("Nombres de los personajes")]
    [SerializeField] private TMP_Text nombreIzquierda;
    [SerializeField] private TMP_Text nombreDerecha;

    [Header("Texto del dialogo")]
    [SerializeField] private TMP_Text textoDialogo;

    [Header("Configuracion")]
    [SerializeField] private KeyCode teclaContinuar = KeyCode.Y;

    [SerializeField] private float velocidadTexto = 0.04f;

    private LineaDialogo[] dialogos;
    private int indiceActual;

    private bool dialogoActivo;
    private bool escribiendo;

    private Coroutine corrutinaTexto;

    public bool DialogoActivo => dialogoActivo;

    private void Start()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }

    private void Update()
    {
        if (!dialogoActivo)
            return;

        if (Input.GetKeyDown(teclaContinuar))
        {
            if (escribiendo)
            {
                CompletarTexto();
            }
            else
            {
                SiguienteDialogo();
            }
        }
    }

    public bool IniciarDialogo(LineaDialogo[] nuevosDialogos)
    {
        if (nuevosDialogos == null || nuevosDialogos.Length == 0)
            return false;

        if (dialogoActivo)
            return false;

        dialogos = nuevosDialogos;

        indiceActual = 0;
        dialogoActivo = true;

        panelDialogo.SetActive(true);

        MostrarDialogoActual();

        return true;
    }

    private void MostrarDialogoActual()
    {
        if (indiceActual >= dialogos.Length)
        {
            FinalizarDialogo();
            return;
        }

        LineaDialogo linea = dialogos[indiceActual];

        if (linea.posicion == 1)
        {
            nombreIzquierda.text = linea.nombre;

            nombreIzquierda.gameObject.SetActive(true);
            nombreDerecha.gameObject.SetActive(false);
        }
        else
        {
            nombreDerecha.text = linea.nombre;

            nombreDerecha.gameObject.SetActive(true);
            nombreIzquierda.gameObject.SetActive(false);
        }

        textoDialogo.text = linea.texto;

        textoDialogo.maxVisibleCharacters = 0;

        corrutinaTexto = StartCoroutine(EscribirTexto());
    }

    private IEnumerator EscribirTexto()
    {
        escribiendo = true;

        textoDialogo.ForceMeshUpdate();

        int cantidadLetras = textoDialogo.textInfo.characterCount;

        for (int i = 0; i < cantidadLetras; i++)
        {
            textoDialogo.maxVisibleCharacters = i + 1;

            yield return new WaitForSecondsRealtime(velocidadTexto);
        }

        textoDialogo.maxVisibleCharacters = cantidadLetras;

        escribiendo = false;
        corrutinaTexto = null;
    }

    private void CompletarTexto()
    {
        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            corrutinaTexto = null;
        }

        textoDialogo.maxVisibleCharacters =
            textoDialogo.textInfo.characterCount;

        escribiendo = false;
    }

    private void SiguienteDialogo()
    {
        indiceActual++;

        if (indiceActual >= dialogos.Length)
        {
            FinalizarDialogo();
            return;
        }

        MostrarDialogoActual();
    }

    public void FinalizarDialogo()
    {
        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            corrutinaTexto = null;
        }

        escribiendo = false;
        dialogoActivo = false;

        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }
}

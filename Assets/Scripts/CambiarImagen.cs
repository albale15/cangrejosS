using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

public class CambiarImagen : MonoBehaviour
{
    // Referencia al componente Image
    public Image imagen;

    // Imágenes para cambiar
    public Sprite imagen1;
    public Sprite imagen2;

    // Bandera para controlar la imagen actual
    private bool esImagen1 = true;

    // Función que cambia la imagen
    public void Cambiar()
    {
        if (esImagen1)
        {
            imagen.sprite = imagen2; // Cambiar a imagen2
        }
        else
        {
            imagen.sprite = imagen1; // Cambiar a imagen1
        }

        esImagen1 = !esImagen1; // Alternar entre las imágenes
    }
}

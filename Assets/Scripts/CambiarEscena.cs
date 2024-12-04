using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para trabajar con escenas

public class CambiarEscena : MonoBehaviour
{
    // M�todo espec�fico para cargar la escena "escenario de lucha"
    public void CargarEscenarioDeLucha()
    {
        string nombreDeLaEscena = "escenario de lucha";
        Debug.Log("Intentando cargar la escena: " + nombreDeLaEscena);

        // Verifica que el nombre de la escena sea v�lido y est� configurado
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            SceneManager.LoadScene(nombreDeLaEscena);
        }
        else
        {
            Debug.LogError("El nombre de la escena no puede estar vac�o.");
        }
    }
}

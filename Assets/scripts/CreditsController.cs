using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
         
            SceneManager.LoadScene("MainMenu");
        }
    }
}
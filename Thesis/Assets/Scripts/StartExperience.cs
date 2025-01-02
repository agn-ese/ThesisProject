using UnityEngine;
using UnityEngine.SceneManagement;

public class StartExperience : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("FireTraining");
    }
}

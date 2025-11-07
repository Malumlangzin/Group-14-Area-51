using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Home_Screen : MonoBehaviour
{

    public void Startgame()
    {
        print("Start pressed");
        SceneManager.LoadSceneAsync(1);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // use scene manager

// I added this script to a GameManager object
// for a button, go to OnClick and drag GameManager object there, then give value of scene (0--> menu, 1 --> first level)
// add scenes into Build Settings, that assigns the indexes

public class ChangeScene : MonoBehaviour {

    //public int levelToLoad;
    public void ChangeToScene(int sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);

    }
    public void Exit()
    {
        Application.Quit();
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenController : MonoBehaviour
{
	void Update ()
    {
	    if (Input.GetButtonDown("Joystick_Action"))
        {
            SceneManager.LoadSceneAsync("Demo");
        }
	}
}

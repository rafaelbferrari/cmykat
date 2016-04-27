using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

    private int _count = 0;

    public int m_delayBetweenScreenshot = 3;

	// Use this for initialization
	void Start () {
        StartCoroutine(Screenshot());
    }
	
    IEnumerator Screenshot()
    {
        Application.CaptureScreenshot(Application.persistentDataPath + "/"+"CMYKat_" + _count+".png");
        Debug.Log("Saved screenshot to " + Application.persistentDataPath + "/");
        _count++;
        yield return new WaitForSeconds(m_delayBetweenScreenshot);
        StartCoroutine(Screenshot());
    }
}

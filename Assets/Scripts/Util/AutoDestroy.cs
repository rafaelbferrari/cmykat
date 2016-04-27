using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

    public float m_autoDestroyTime;
	// Use this for initialization
	void Start () {
        Invoke("Destroy", m_autoDestroyTime);
	}
	
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

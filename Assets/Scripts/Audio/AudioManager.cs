using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public GameObject m_mainMenu;
    public GameObject m_gamePlay;
    
    public void PlayMainMenu()
    {
        m_mainMenu.SetActive(true);
        m_gamePlay.SetActive(false);
    }

    public void PlayGamePlay()
    {
        m_mainMenu.SetActive(false);
        m_gamePlay.SetActive(true);
    }

    public void FadeMainMenu()
    {
        StartCoroutine(SetVolumeDown(m_mainMenu.GetComponent<AudioSource>()));
    }

    IEnumerator SetVolumeDown(AudioSource audio)
    {
        yield return new WaitForSeconds(.1f);
        if(audio.volume > 0)
        {
            audio.volume -= .05f;
            StartCoroutine(SetVolumeDown(audio));
        }
        else
        {
            audio.volume = 1;
            audio.gameObject.SetActive(false);
        }

    }

}

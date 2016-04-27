using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonBase : MonoBehaviour
{

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        GameObject pause = UIView.Instance.m_popup.GetPopup(PopupsView.PopupType.PauseGame).gameObject;
        pause.GetComponent<EndGameView>().ResumeGame();
    }
}

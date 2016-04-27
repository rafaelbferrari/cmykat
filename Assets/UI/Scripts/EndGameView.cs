using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameView : PopupBase
{
    public RectTransform m_avatarOne;
    public RectTransform m_avatarTwo;

    public Text m_scoreOne;
    public Text m_scoreTwo;

    private float scoreOne;
    private float scoreTwo;
    private float totalBoardSlots;

    public GameObject[] m_buttons;

    public EndGamePlayer[] m_PlayersList;
    
    void OnEnable()
    {
        if (m_Type != PopupsView.PopupType.PauseGame || m_Type == PopupsView.PopupType.EndGame)
        {
            GameManager.Instance.m_audioManager.PlayMainMenu();
        }
        
        UpdateScore();

        if (m_Type == PopupsView.PopupType.PauseGame || m_Type == PopupsView.PopupType.EndGame)
        {
            Time.timeScale = 0;
        }

    }

    private void UpdateScore()
    {
        if (m_Type != PopupsView.PopupType.PauseGame)
        {
            List<Player> players = GameManager.Instance.m_playerManager.GetPlayerList();

            Vector2 boardSize = GameManager.Instance.GetNewBoard().GetBoardSize();
            totalBoardSlots = (boardSize.x * boardSize.y);
            

            for (int i = 0; i < players.Count; i++)
            {
                Player p = players[i];
                SetPlayerScore(i, p.GetScore(), 237 + (413 * (p.GetScore() / totalBoardSlots)));
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        foreach(GameObject b in m_buttons)
        {
            b.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    
    public void RestartGame()
    {
        gameObject.SetActive(false);
        GameManager.Instance.m_audioManager.PlayGamePlay();
        GameManager.Instance.Reset();
    }

    public void QuitGame()
    {

    }

    public void SetPlayerScore(int p_player, int p_score, float p_playerHeight)
    {
        m_PlayersList[p_player].m_Score = p_score;
        m_PlayersList[p_player].m_AvatarHeight = p_playerHeight;
    }

}

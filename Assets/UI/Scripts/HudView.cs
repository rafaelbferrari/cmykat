using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudView : MonoBehaviour 
{
    //public HudChargesView[] m_PlayerCharges;

    public Text[] m_PlayerScore;

    public Text m_TimeText;

    /*
	public void ActivatePlayerCharges(int p_playerIndex, int p_chargeIndex)
	{
        if(p_playerIndex < m_PlayerCharges.Length)
		    m_PlayerCharges[p_playerIndex].ActivateCharge(p_chargeIndex);
	}

    public void ClearPlayerCharge(int p_playerIndex)
    {
        if (p_playerIndex < m_PlayerCharges.Length)
            m_PlayerCharges[p_playerIndex].ClearAllCharges();
    }
    */

    public void SetPlayerScore(int p_player, int p_score)
    {
        m_PlayerScore[p_player].text = p_score.ToString();
    }
    
    public void SetTime(int p_time)
	{
		if(p_time >= 0)
		{
			m_TimeText.text = p_time.ToString();
		}

		else 
		{
			m_TimeText.text = "TIME OUT";
		}
	}

	public void ShowResults()
	{

	}
}

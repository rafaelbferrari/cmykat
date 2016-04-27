using UnityEngine;
using System.Collections;

public class PopupsView : MonoBehaviour
{
	public enum PopupType
	{
		None,
		CharSelection,
        EndGame,
        PauseGame
    }
    
    public PopupBase[] m_PopupList;

	public void ShowPopup(PopupType p_type)
	{
		foreach(PopupBase popup in m_PopupList)
		{
			if(popup.m_Type == p_type) popup.gameObject.SetActive(true);
		}
	}

    public PopupBase GetPopup(PopupType p_type)
    {
        foreach (PopupBase popup in m_PopupList)
        {
            if (popup.m_Type == p_type) return popup;
        }
        return null;
    }
}

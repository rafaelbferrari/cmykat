using UnityEngine;
using System.Collections;

public class UIView : MonoBehaviour 
{
	public HudView m_Hud;
    public PopupsView m_popup;

    private static UIView _instance;
    public static UIView Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIView>();
            }
            return _instance;
        }
    }

}

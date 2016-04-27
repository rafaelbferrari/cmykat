using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGamePlayer : MonoBehaviour {

    public Text m_ScoreText;

    public RectTransform m_AvatarTransform;

    public int m_Score
    {
        set
        {
            m_ScoreText.text = value.ToString();
        }
    }

    public float m_AvatarHeight
    {
        set
        {
            m_AvatarTransform.sizeDelta = new Vector2(m_AvatarTransform.sizeDelta.x, value);
        }
    }
}

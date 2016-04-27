using UnityEngine;
using System.Collections;

[System.Serializable]
public class TeamPlayer
{
	public enum TeamPlayerID
	{
		NONE,
		ONE,
		TWO,
		THREE,
		FOUR,
		COUNT
	}

	public TeamPlayerID m_teamId;
	public Color m_teamColor;
	public Texture2D m_catTexture;
	public Texture2D m_foodTexture;
	public InputManager.Joystick m_joystick;

	[Header("Box")]
	public Texture2D m_boxTexture;

}


using UnityEngine;
using System.Collections;

public class CharSelectView : PopupBase
{
	public int m_MinPlayers;

	public int SelectedPlayers { get; private set; }
    private const int MAX_PLAYERS = 4;
    private bool m_PlayersCanJoin;

    private Animator _Animator;
    private bool[] _PlayerSectionState = { false, false, false, false };

    public GameObject m_pressStart;

    private static CharSelectView _instance;
    public static CharSelectView Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CharSelectView>();
            }

            return _instance;
        }
    }

    void Start()
    {
        m_PlayersCanJoin = true;
        _Animator = GetComponent<Animator>();
        if (_Animator == null)
        {
            Debug.LogError("CharSelectView requires an Animator component to work. Assign one.");
        }
    }

    public void Update()
    {
        if (_Animator != null && m_PlayersCanJoin)
        {
            for (int i = 0; i < MAX_PLAYERS; ++i)
            {
                string buttonName = "Joystick" + (i + 1) + "_Action";
                if (Input.GetButtonDown(buttonName) && !_PlayerSectionState[i])
                {
                    Debug.Log(buttonName);
                    _PlayerSectionState[i] = true;
                    _Animator.SetTrigger("CharSelected_" + i);
                }
            }
        }
    }

	public void AddPlayer()
	{
        SelectedPlayers++;
        if(SelectedPlayers>=2)
        {
            m_pressStart.SetActive(true);
        }
	}

    public void StartCountdown()
    {
        _Animator.SetTrigger("Countdown");
        m_PlayersCanJoin = false;
    }

	public void StartGame()
	{
		_Animator.SetTrigger ("PlayOutro");
	}

    public void DisablePopup()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }
}

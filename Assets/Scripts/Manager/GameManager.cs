using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public NewBoardManager m_newBoardManager;
    public PlayerManager m_playerManager;
    public AudioManager m_audioManager;
    public float m_timeToSelectSlot = 1f;
    public int m_totalGameTime = 60;
    public float m_shootRecoilStrength = 200.0f;
    public float m_maxPlayerSpeed = 50.0f;

    public float m_spawnInvincibilityTime = 2.5f;
    public float SpawnInvincibilityTime
    {
        get
        {
            return m_spawnInvincibilityTime;
        }

        private set
        {
            m_spawnInvincibilityTime = value;
        }
    }

    public LevelDescriptor m_level;

	public TeamPlayer[] m_teams;

    private int _gameTime;

    private bool _gameStarted = false;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    void Start()
    {
        SpawnInvincibilityTime = m_spawnInvincibilityTime;
        m_audioManager.PlayMainMenu();
    }


    public void StartGame()
    {
		m_audioManager.PlayGamePlay ();
		_gameTime = m_totalGameTime;
		UIView.Instance.m_Hud.gameObject.SetActive (true);
		UpdateTimeHUD ();
		Initialize ();
        _canPause = false;
        StartCoroutine (CountDown ());
    }

    private bool _canPause = false;

    void Update()
    {
        if (Input.GetButtonDown("Joystick_Start") || Input.GetKeyDown(KeyCode.Space))
        {
            CharSelectView charView = CharSelectView.Instance;
            if (!_gameStarted && charView.SelectedPlayers >= 2)
			{
				_gameStarted = true;
	            m_audioManager.FadeMainMenu();
                charView.StartCountdown();
            }
            else if(_canPause)
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    public void Reset()
    {
        ResetBoards();
        _gameTime = m_totalGameTime;
        _canPause = false;
        UIView.Instance.m_Hud.gameObject.SetActive (true);
        UpdateTimeHUD();
        StartCoroutine(CountDown());
    }

    private void Initialize()
    {
        int selectedPlayers = CharSelectView.Instance.SelectedPlayers;
        TeamPlayer[] teams = new TeamPlayer[selectedPlayers];
        for (int i = 0; i < selectedPlayers; ++i)
        {
            teams[i] = m_teams[i];
        }

        m_newBoardManager.Initialize(m_level);
        m_playerManager.Initialize(m_level, teams);
    }

    private void ResetBoards()
    {
        GetNewBoard().ResetBoard();
        GetPlayerManager().Reset();
    }
    
    public NewBoardManager GetNewBoard()
    {
        return m_newBoardManager;
    }

    public PlayerManager GetPlayerManager()
    {
        return m_playerManager;
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        if(!_canPause) _canPause = true;
        if (_gameTime > 0)
        {
            _gameTime--;
            UpdateTimeHUD();
            StartCoroutine(CountDown());
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateTimeHUD()
    {
        UIView.Instance.m_Hud.SetTime(_gameTime);
    }

    private void GameOver()
    {
		UIView.Instance.m_Hud.gameObject.SetActive (false);
        UIView.Instance.m_popup.ShowPopup(PopupsView.PopupType.EndGame);
    }

    private void Pause()
    {
        UIView.Instance.m_popup.ShowPopup(PopupsView.PopupType.PauseGame);
    }
}

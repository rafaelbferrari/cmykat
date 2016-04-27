using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBoardManager : MonoBehaviour
{
    public GameObject m_boardSlotPrefab;
    public float m_space;
    
    private BoardSlot[,] _board;
    private GameObject _container;

    private LevelDescriptor _currentLevel;

    #region BOARD CREATION
    public void Initialize(LevelDescriptor level)
    {
        _currentLevel = level;
        _board = CreateBoard();
        _container = new GameObject("Container");
        
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.GetLength(1); k++)
            {
                GameObject g = Instantiate(_currentLevel.m_boardSlotPrefab);
                BoardSlot slot = g.GetComponent<BoardSlot>();
                slot.Init();
                slot.SetupCallbacks(OnSlotEnter, OnSlotExit);
                slot.SetPosition(i, k, m_space);
                slot.SetParent(_container.transform);
                slot.m_position = new Vector2(i, k);
                _board[i, k] = slot;
            }
        }
        CreateDictionary();
    }

    private BoardSlot[,] CreateBoard()
    {
        return new BoardSlot[(int)_currentLevel.m_boardSize.x, (int)_currentLevel.m_boardSize.y];
    }
    #endregion

    #region SLOT CALLBACKS
    private void OnSlotEnter(BoardSlot slot, Player player)
    {
        if(_currentLevel.m_automaticPieceSelection)
        {
            if(_currentLevel.m_keyRule)
            {
                if(player.HasKey)
                {
                    slot.SelectSlot(player);
                }
            }
            else
            {
                slot.SelectSlot(player);
            }
        }
    }

    private void OnSlotExit()
    {

    }
    #endregion

    private BoardSlot GetSlot(int x, int y)
    {
        return _board[x, y];
    }

    public BoardSlot GetRandomSlot()
    {
        return _board[(int)Random.Range(0, _board.GetLength(0)), (int)Random.Range(0, _board.GetLength(1))];
    }

	public Vector2 GetBoardSize()
	{
		return new Vector2((int)_currentLevel.m_boardSize.x, (int)_currentLevel.m_boardSize.y);
	}

	public int GetScoreByTeam(TeamPlayer team)
	{
		int score = 0;
		foreach(BoardSlot slot in _board)
		{
			if(slot.m_slotTeam == team)
			{
				score++;
			}
		}
		return score;
	}
	
	public void ResetBoard()
	{
		foreach(BoardSlot slot in _board)
		{
			slot.Reset();
            Destroy(slot.gameObject);
		}
        Initialize(_currentLevel);
	}

    private Dictionary<TeamPlayer.TeamPlayerID, int> _scoreByPlayer;

    private void CreateDictionary()
    {
        _scoreByPlayer = new Dictionary<TeamPlayer.TeamPlayerID, int>();
        _scoreByPlayer.Add(TeamPlayer.TeamPlayerID.ONE, 0);
        _scoreByPlayer.Add(TeamPlayer.TeamPlayerID.TWO, 0);
        _scoreByPlayer.Add(TeamPlayer.TeamPlayerID.THREE, 0);
        _scoreByPlayer.Add(TeamPlayer.TeamPlayerID.FOUR, 0);
    }

    public void AddScore(TeamPlayer.TeamPlayerID team)
    {
        _scoreByPlayer[team] += 1;
        UpdateUI(team);
    }

    public void RemoveScore(TeamPlayer.TeamPlayerID team)
    {
        _scoreByPlayer[team] -= 1;
        UpdateUI(team);
    }

    private void UpdateUI(TeamPlayer.TeamPlayerID team)
    {
        UIView.Instance.m_Hud.SetPlayerScore((int)team - 1, _scoreByPlayer[team]);
    }

    public Dictionary<TeamPlayer.TeamPlayerID, int> GetScoreList()
    {
        return _scoreByPlayer;
    }
}

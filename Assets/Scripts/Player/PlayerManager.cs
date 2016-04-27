using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

    public GameObject m_playerPrefab;
    public float m_diferenceToGround;
    public GameObject m_explosionAudio;
    public Transform[] m_SpawnPoints;

    private List<Player> _players;
    private LevelDescriptor _level;


	public void Initialize(LevelDescriptor level, TeamPlayer[] players)
    {
        _players = new List<Player>();

		for (int i = 0; i < players.Length; ++i)
		{
            TeamPlayer team = players[i];
			GameObject g = Instantiate(m_playerPrefab);
			Player player = g.GetComponent<Player>();
			player.ChangeTeamSlot(team, _players.Count+1);
            player.SetPostion(m_SpawnPoints[i].position);
			player.Init(GameManager.Instance.SpawnInvincibilityTime);
			_players.Add(player);
		}

        if(level.m_keyRule)
        {
            int rand = Random.Range(0, _players.Count);
			AddRoundKey(_players[rand]);
        }
    }

    public void Reset()
    {
        foreach (Player player in _players)
        {
            player.Reset();
            RandomizePosition(player);
        }
    }

    public int GetPlayerNumber()
    {
        return _players.Count;
    }

    public void RandomizePosition(Player player)
    {
        player.SetPostion(GameManager.Instance.GetNewBoard().GetRandomSlot().transform.position + new Vector3(0, m_diferenceToGround, 0));
    }
    #region KEY GAMEPLAY
    private Player _currentkeyPlayer;
    public void AddRoundKey(Player player)
    {
        if(_currentkeyPlayer != player)
        {
            if(_currentkeyPlayer != null)
            {
                _currentkeyPlayer.RemoveKey();
            }
            _currentkeyPlayer = player;
            _currentkeyPlayer.AddKey();
        }
    }

    #endregion

    public void KillPlayer(Player killed, Player killer)
    {
        AddRoundKey(killer);
        killed.Kill();
        ReBorn(killed);
        CreateExplosionAudio();
    }

    public void ReBorn(Player player)
    {
        StartCoroutine(Born(player, 2f));
    }

    IEnumerator Born(Player p, float delay)
    {
        yield return new WaitForSeconds(delay);
        RandomizePosition(p);
        p.Reset();
    }

    public void CreateExplosionAudio()
    {
        Instantiate(m_explosionAudio);
    }

    public List<Player> GetPlayerList()
    {
        return _players;
    }
}

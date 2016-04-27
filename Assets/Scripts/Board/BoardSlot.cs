using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class BoardSlot : MonoBehaviour
{
    public Action<BoardSlot, Player> _onEnterCallback;
    public Action _onExitCallback;

    public enum BoardSlotState
    {
        IDLE,
        ONE,
        TWO
    }

	public GameObject m_slot;

    public Animator m_animator;
    public Vector2 m_position;
    public ParticleSystem m_selectingParticle;

    private Player _slotLastPlayer;
	public TeamPlayer m_slotTeam;
    public MeshRenderer[] m_meshRenderers;

	public void UpdateMeshRenderersTexture()
	{
		if(m_slotTeam == null)
		{
			return;
		}
		foreach(MeshRenderer m in m_meshRenderers)
		{
			m.gameObject.SetActive(true);
			m.material.SetTexture("_MainTex", m_slotTeam.m_boxTexture);
		}
	}

    public void Reset()
    {
        SetState();
    }

    public void ResetLastState()
    {
        SetState(_slotLastPlayer);
    }

    public void Init()
    {
        SetState();
        m_selectingParticle.Stop();
    }

    public void SetupCallbacks(Action<BoardSlot, Player> enter, Action exit)
    {
        _onEnterCallback = enter;
        _onExitCallback = exit;
    }

    public void SetPosition(int indexX, int indexY, float space)
    {
        gameObject.transform.position = new Vector3((indexX * GetBounds().size.x) + (indexX * space), gameObject.transform.position.y, (indexY * GetBounds().size.z) + (indexY * space));
    }
    public void SetParent(Transform t)
    {
        transform.SetParent(t);
    }

    private Bounds GetBounds()
    {
        return GetComponent<Collider>().bounds;
    }

	private void UpdateSlot()
	{
		m_selectingParticle.startColor = m_slotTeam != null ? m_slotTeam.m_teamColor : Color.white;
	}

    public void SetState(Player player = null)
    {
        if(player != null)
        {
            if(_slotLastPlayer != null && player.Team != _slotLastPlayer.Team)
            {
                _slotLastPlayer.RemoveScore();
                player.AddScore();
            }
            else if(_slotLastPlayer == null)
            {
                player.AddScore();
            }
            m_slotTeam = player.Team;
            _slotLastPlayer = player;
        }
        else
        {
            m_slotTeam = null;
            _slotLastPlayer = null;
        }

		UpdateSlot();
		UpdateMeshRenderersTexture();

		StartCoroutine(ShowPArticle());
        m_animator.SetBool("Selection", false);
    }

	IEnumerator ShowPArticle()
	{
		m_selectingParticle.Play();
		yield return new WaitForSeconds (1f);
		m_selectingParticle.Stop();
	}

    public TeamPlayer GetState()
    {
        return m_slotTeam;
    }
    
    public void SelectSlot(Player player)
    {
        SetState(player);
    }

    void OnTriggerEnter(Collider col)
    {
        if(_onEnterCallback != null && col.gameObject.tag == "PlayerTrigger")
        {
            _onEnterCallback(this, col.transform.parent.GetComponent<Player>());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (_onExitCallback != null)
        {
            _onExitCallback();
        }
    }
}

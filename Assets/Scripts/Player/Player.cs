using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public BoardSlot.BoardSlotState m_teamSlot;

	public TeamPlayer Team { get; private set; }

    public Transform m_projectileStart;

    public GameObject m_particle;
	public SkinnedMeshRenderer m_catMeshRenderer;
	public SkinnedMeshRenderer m_foodCanMeshRenderer;

    public Color m_blinkColor;
    public GameObject m_Key;

	public Animation m_catAnimation;

    public bool IsSpawning { get; private set; }

    private SkinnedMeshRenderer[] m_Renderers;
	private InputManager.Joystick _joystick;
	private BoardSlot _boardSlot;
	private bool _canShoot = true;
	private InputManager _input;
    private int _score;

    private bool _hasKey;
    public bool HasKey
    {
        get
        {
            return _hasKey;
        }
    }

	#region TEAM_COLOR
	public void ChangeTeamSlot(TeamPlayer team, int teamNum)
	{
		Team = team;
		m_teamSlot = (BoardSlot.BoardSlotState)teamNum;
        m_particle.GetComponent<ParticleSystem>().startColor = team.m_teamColor;
        ChangeCatTexture(team.m_catTexture);
		ChangeFoodTexture(team.m_foodTexture);
		_joystick = Team.m_joystick;
	}

	public void ChangeCatTexture(Texture2D texture)
	{
		m_catMeshRenderer.material.SetTexture("_MainTex",texture);
	}
	
	public void ChangeFoodTexture(Texture2D texture)
	{
		m_foodCanMeshRenderer.material.SetTexture("_MainTex",texture);
	}

	private void SetColor(Color color)
	{
        if (m_Renderers == null)
        {
            m_Renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        foreach (SkinnedMeshRenderer mesh in m_Renderers)
        {
            foreach (Material m in mesh.materials)
            {
                m.color = color;
            }
        }
	}

    public void AddScore()
    {
        _score++;
        UpdateUI();
    }

    public void RemoveScore()
    {
        _score--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        UIView.Instance.m_Hud.SetPlayerScore((int)m_teamSlot-1, _score);
    }
    
    public int GetScore()
    {
        return _score;
    }
    #endregion

    #region KEY
    public void AddKey()
    {
        _hasKey = true;
        UpdateKey();
        if(_boardSlot != null)
            _boardSlot.SelectSlot(this);
    }

    public void RemoveKey()
    {
        _hasKey = false;
        UpdateKey();
    }
    
    private void UpdateKey()
    {
        m_Key.SetActive(_hasKey);
    }
	#endregion

    void OnEnable()
    {
        m_catAnimation.Play("sapwn");
    }

    public void SetPostion(Vector3 pos)
    {
        transform.transform.position = pos;
    }

	#region STATES
    public void Init(float spawnDuration)
    {
        _input = gameObject.GetComponent<InputManager>();
        _input.m_joystick = _joystick;
        _input.m_player = this;
        IsSpawning = false;

        Reset();
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        _canShoot = true;
        _input.enabled = true;
        UpdateKey();
        StartCoroutine(SpawnAnimation(GameManager.Instance.SpawnInvincibilityTime));
        SetColor(Color.white);
    }

	public void Kill()
	{
		GameObject g = Instantiate(m_particle);
		g.GetComponent<ParticleSystem> ().startColor = Team.m_teamColor;
		g.GetComponent<ParticleSystem>().Play();
		g.transform.position = transform.position;
		gameObject.SetActive(false);
	}

	public IEnumerator SpawnAnimation(float duration)
	{
		float time = 0.0f;
		IsSpawning = true;
		
		while (time < duration)
		{
			float alpha = Mathf.Lerp(0.3f, 1.0f, Mathf.Cos(time * Mathf.PI * 5.0f));
			float r = Mathf.Lerp(m_blinkColor.r, 1.0f, alpha);
			float g = Mathf.Lerp(m_blinkColor.b, 1.0f, alpha);
			float b = Mathf.Lerp(m_blinkColor.r, 1.0f, alpha);
			SetColor(new Color(r, g, b, 1.0f));
			time += Time.deltaTime;
			yield return null;
		}
		
		SetColor(Color.white);
		IsSpawning = false;
	}
	#endregion

	#region TRIGGERS
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            ContactPoint point = col.contacts[0];

            Vector3 forceDirection = (point.thisCollider.transform.position - point.otherCollider.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(forceDirection * GameManager.Instance.m_shootRecoilStrength);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Projectile" && !IsSpawning)
        {
            GameManager.Instance.GetPlayerManager().KillPlayer(this, col.gameObject.GetComponent<Projectile>().m_player);
            //Kill(col.gameObject.GetComponent<Projectile>().m_player);
        }
        else
        {
            if (col.GetComponent<BoardSlot>())
            {
                _boardSlot = col.GetComponent<BoardSlot>();
                
            }
        }
    }
	#endregion

	#region SHOOT
    public void Shoot(Projectile projectile)
    {
        m_catAnimation.Play("shoot");
        _canShoot = false;
        projectile.transform.position = m_projectileStart.position;
        projectile.transform.eulerAngles = m_projectileStart.eulerAngles;
        _input.enabled = false;
        AddForce();
        StartCoroutine(ResetShoot());
        StartCoroutine(ActiveInput());
    }
	
	public bool CanShoot()
	{
		return _canShoot;
	}

	IEnumerator ResetShoot()
	{
		yield return new WaitForSeconds(GunManager.Instance.m_shootWaitTime);
		_canShoot = true;
	}
	#endregion

	#region INPUT
    IEnumerator ActiveInput()
    {
        yield return new WaitForSeconds(.3f);
        _input.enabled = true;
    }
	#endregion

    void Update()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        float maxSpeed = GameManager.Instance.m_maxPlayerSpeed;
        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }

        if (transform.position.y < .5f)
        {
            _input.enabled = false;
        }
        if (transform.position.y < -15f)
        {
            Reset();
            GameManager.Instance.GetPlayerManager().RandomizePosition(this);
            m_catAnimation.Play("sapwn");
        }
    }

    private void AddForce()
    {
        GetComponent<Rigidbody>().AddForce(-transform.forward * GameManager.Instance.m_shootRecoilStrength);
    }

}

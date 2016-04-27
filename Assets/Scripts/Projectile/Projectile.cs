using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float m_speed = 5f;
    public Player m_player;

    public Light m_light;
    public ParticleSystem m_particle;

    public GameObject m_sound;

    void Start()
    {
        StartCoroutine(AutoDestroy());
        UpdateColor();
        Instantiate(m_sound);
    }

    public void UpdateColor()
    {
        Color c = m_player.Team.m_teamColor;
        m_light.color = c;
        m_particle.startColor = c;
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

	void Update ()
    {
        transform.position += transform.forward * m_speed * Time.deltaTime;
	}

    //void OnCollisionEnter()
    //{
    //    Destroy(gameObject);
    //}
}

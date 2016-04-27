using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {
    
    public GameObject m_projectile;
    public float m_shootWaitTime;
    
    private static GunManager _instance;
    public static GunManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GunManager>();
            }

            return _instance;
        }
    }

    public void Shoot(Player player)
    {
        if (player.CanShoot())
        {
            GameObject g = Instantiate(m_projectile,new Vector3(0,-300,0), new Quaternion()) as GameObject;
            Projectile p = g.GetComponent<Projectile>();
            p.m_player = player;
            player.Shoot(p);
        }
    }
    
}

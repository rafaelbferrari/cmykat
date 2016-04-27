using UnityEngine;
using System.Collections;

public class PlayerSlotTrigger : MonoBehaviour {
    private Player m_player;

    void Awake()
    {
        m_player = transform.parent.GetComponent<Player>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (m_player != null)
        {
            m_player.OnTriggerEnter(col);
        }
    }
}

using UnityEngine;
using System.Collections;

public class LevelDescriptor : ScriptableObject
{
    public Vector2 m_boardSize;

    [Header("Board Setup")]
    public float m_timeToSelectPiece;
    public bool m_automaticPieceSelection;
    public bool m_keyRule;
    public GameObject m_boardSlotPrefab;
}

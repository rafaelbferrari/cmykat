using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudChargesView : MonoBehaviour
{
	[System.Serializable]
	public class Charges
	{
		public Image[] m_ChargeSlots;
	}

	public Charges[] m_ChargeList;

	public Sprite m_UnchargedSprite;

	public Sprite m_ChargedSprite;

	public void ActivateCharge(int p_Index)
	{
		Image[] charges = m_ChargeList [p_Index].m_ChargeSlots;

		foreach(Image charge in charges)
		{
			charge.sprite = m_ChargedSprite;
		}
	}

	public void ClearAllCharges()
	{
		foreach(Charges charges in m_ChargeList)
		{
			ClearCharge(charges.m_ChargeSlots);
		}

	}

	public void ClearCharge(Image[] p_chargeList)
	{
		foreach(Image charge in p_chargeList)
		{
			charge.sprite = m_UnchargedSprite;
		}
	}
}

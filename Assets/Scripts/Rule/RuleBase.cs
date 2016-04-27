using UnityEngine;
using System.Collections;

public class RuleBase : MonoBehaviour {

    public virtual void OnPieceEnter() { }
    public virtual void OnPieceStay() { }
    public virtual void OnPieceExit() { }

}

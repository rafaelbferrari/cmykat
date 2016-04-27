using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountValue : MonoBehaviour {

    private int _targetNumber;
    private int _number;

    private Text _text;

    public void Init(int num)
    {
        _number = 0;
        _targetNumber = num;
        _text = gameObject.GetComponent<Text>();
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(.03f);
        if(_number < _targetNumber)
        {
            _number++;
            _text.text = _number.ToString();
            StartCoroutine(Count());
        }
        else
        {
            Destroy(this);
        }
    }
}

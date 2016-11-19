using UnityEngine;
using System.Collections;

public class ColumnProperties : MonoBehaviour
{
    private enum ColumnType { Dynamic, Static };
    public enum ColumnStatus { Free, Locked };

    [SerializeField]
    private ColumnType _columnType;

    [HideInInspector]
    public ColumnStatus columnStatus;

    private bool _atBaseLevel = true;
    private float _columnResettingSpeed;
    private float _columnDisplacementSize;
    private float _baseYValue;
    private float _polarity;
    private float _movementDelta = 0.0f;

    private void Start()
    {

    }

    private void Update()
    {
        if (_atBaseLevel == false)
        {
            grindToArenaLevel();
        }
    }

    public void ResetColumn(float pColumnResettingSpeed, float pColumnDisplacementSize, float pBaseYValue, float pPolarity)
    {
        _columnResettingSpeed = pColumnResettingSpeed;
        _columnDisplacementSize = pColumnDisplacementSize;
        _baseYValue = pBaseYValue;
        _polarity = pPolarity;
        _atBaseLevel = false;
    }


    private void grindToArenaLevel()
    {
        if(_movementDelta < _columnDisplacementSize)
        //if (Mathf.Abs(gameObject.transform.position.y) + (columnDisplacementSize / 2) > baseYValue)
        {
            gameObject.transform.Translate(0, _polarity * _columnResettingSpeed, 0);
            _movementDelta += _columnResettingSpeed;
        }
        else
        {
            Debug.Log("My pos before .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, _baseYValue, gameObject.transform.position.z);                   
            Debug.Log("My pos after .set is: " + gameObject.transform.position + " while the baseY value I was given is:" + _baseYValue);
            _movementDelta = 0.0f;
            _atBaseLevel = true;
            //Debug.Log("I actually reached here!");
        }

    }

    public int GetColumnType()
    {
        return (int)_columnType;
    }
}

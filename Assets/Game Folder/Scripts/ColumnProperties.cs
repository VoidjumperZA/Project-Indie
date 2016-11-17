using UnityEngine;
using System.Collections;

public class ColumnProperties : MonoBehaviour
{
    private enum ColumnType { Dynamic, Static };
    public enum ColumnStatus { Free, Locked };

    [SerializeField]
    private ColumnType columnType;

    [HideInInspector]
    public ColumnStatus columnStatus;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GrindToArenaLevel()
    {

    }
}

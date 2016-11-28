using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePentagram(bool pState, Transform pPlayerTransform)
    {
        gameObject.SetActive(pState);

        SimpleLookAt rune = GetComponentInChildren(typeof(SimpleLookAt)) as SimpleLookAt;
        rune.AssignPlayer(pPlayerTransform);
    }

    public void TogglePentagram(bool pState)
    {
        gameObject.SetActive(pState);
    }

    public void MovePentagram(Transform pColumnTransform)
    {
        gameObject.transform.position = new Vector3(pColumnTransform.position.x, gameObject.transform.position.y, pColumnTransform.position.z);
    }

    public bool IsPentagramActive()
    {
        return gameObject.activeSelf;
    }

}

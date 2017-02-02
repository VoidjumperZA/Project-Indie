using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObjectHandler : MonoBehaviour
{
    [SerializeField]
    private Texture[] _emissionMaps;
    [SerializeField]
    private float _nextEmissionTime = 2.0f;

    private GameObject _manaObject;
    private Material _material;

    private void Start()
    {
        _manaObject = transform.GetChild(0).gameObject;
        _material = GetComponent<MeshRenderer>().material;

        _material.SetTexture("_EmissionMap", _emissionMaps[6]);
    }

    private void Update()
    {
    }

    public void PickUP()
    {
        _manaObject.SetActive(false);
        StartCoroutine(Switcheroo());
    }

    private IEnumerator Switcheroo()
    {
        for (int i = 0; i  < _emissionMaps.Length; i ++)
        {
            _material.SetTexture("_EmissionMap", _emissionMaps[i]);

            yield return new WaitForSeconds(_nextEmissionTime);
        }
        _manaObject.SetActive(true);
    }
}

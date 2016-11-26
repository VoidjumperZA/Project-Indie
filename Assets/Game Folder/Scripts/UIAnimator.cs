using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    [SerializeField]
    private Image[] HUDRuneLayer_01;

    [SerializeField]
    private Image[] HUDRuneLayer_02;

    [SerializeField]
    private Image[] HUDRuneLayer_03;
    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        animateRuneLayers(HUDRuneLayer_01, 1.0f);
        animateRuneLayers(HUDRuneLayer_02, 0.5f);
        animateRuneLayers(HUDRuneLayer_03, 0.25f);
    }

    private void animateRuneLayers(Image[] pHudRuneLayer, float pSpeed)
    {
        for (int i = 0; i < pHudRuneLayer.Length; i++)
        {
            float polarity = 1.0f;
            if (i % 2 == 0)
            {
                polarity = -1.0f;
            }
            pHudRuneLayer[i].transform.Rotate(0, 0, pSpeed * polarity);
        }
    }

}

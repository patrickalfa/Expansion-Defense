using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorVariation : MonoBehaviour
{
    public float minHue, maxHue;
    public float minSaturation, maxSaturation;
    public float minValue, maxValue;
    public float minAlpha, maxAlpha;

    private void Start()
    {
        GetComponent<SpriteRenderer>().material.color =
            Random.ColorHSV(minHue, maxHue, minSaturation, maxSaturation,
                minValue, maxValue, minAlpha, maxAlpha);
    }
}

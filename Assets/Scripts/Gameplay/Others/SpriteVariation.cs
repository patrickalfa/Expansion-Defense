using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteVariation : MonoBehaviour
{
    public Sprite[] variations;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite =
            variations[Random.Range(0, variations.Length)];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;


    private void Start()
    {
        InvokeRepeating("Spawn", 1, 1);
    }

    void Spawn() {
        GameObject go = Instantiate(Resources.Load("Prefabs/Circle")) as GameObject;
        go.GetComponent<SpriteRenderer>().sprite = sprites[GetSprite()];

    }

    int GetSprite() {
        return Random.Range(0, 4);
    }
}

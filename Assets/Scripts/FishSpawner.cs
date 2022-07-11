using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject fishPrefab;

    private void Start()
    {
        RandomlySpawnFish();
    }

    private void RandomlySpawnFish()
    {
        foreach(Transform child in transform)
        {
            Instantiate(fishPrefab, child);
        }
    }
}

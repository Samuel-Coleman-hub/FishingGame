using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject fishPrefab;
    public List<FishController> fishes = new List<FishController>();

    private GameObject newFish;

    private void Start()
    {
        RandomlySpawnFish();
    }

    private void RandomlySpawnFish()
    {
        foreach(Transform child in transform)
        {
            newFish = Instantiate(fishPrefab, child);
            fishes.Add(newFish.GetComponent<FishController>());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    
    [SerializeField] GameObject[] fishes;

    //private void Start()
    //{
    //    RandomlySpawnFish();
    //}

    //private void RandomlySpawnFish()
    //{
    //    foreach(Transform child in transform)
    //    {
    //        newFish = Instantiate(fishPrefab, child);
    //        fishes.Add(newFish.GetComponent<FishController>());
    //    }
    //}

    public IEnumerator WaitToRespawn(Vector3 spawnPosition ,FishTracker.Fishes fish)
    {
        yield return new WaitForSeconds(10f);

        for (int i = 0; i < fishes.Length; i++)
        {
            FishStateManager fishStateManager = fishes[i].GetComponent<FishStateManager>();

            if (fishStateManager.typeOfFish == fish)
            {
                Debug.Log("new fish spawned");
                GameObject.Instantiate(fishes[i], transform.TransformPoint(spawnPosition), gameObject.transform.rotation);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScrapBookInterface : MonoBehaviour
{
    private List<GameObject> scrapBookFish = new List<GameObject>();
    [SerializeField] TextMeshProUGUI totalFishText;
    [SerializeField] GameObject winText;

    private void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            scrapBookFish.Add(child.gameObject);
        }
    }

    public void UpdateScrapbook(FishTracker.Fishes fishType)
    {
        Transform fishUIObj = transform.Find(fishType.ToString());

        fishUIObj.Find("Panel").gameObject.SetActive(false);
        fishUIObj.Find("StrikeThrough").gameObject.SetActive(true);    
    }

    public void UpdateTotal(int fishCaught)
    {
        totalFishText.text = fishCaught.ToString() + "/" + "9";

        if (fishCaught >= 9)
        {
            Debug.Log("Enable Win Text");
            winText.SetActive(true);
        }
    }
}

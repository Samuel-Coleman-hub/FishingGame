using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTracker : MonoBehaviour
{
    private Dictionary<Fishes, bool> fishDictionary;
    public enum Fishes
    {
        Seabass,
        Chub,
        normal
    }

    private void Start()
    {
        //Dictionary of all unique fish, boolean denotes whether its been caught
        fishDictionary = new Dictionary<Fishes, bool>();

        foreach(Fishes fish in Enum.GetValues(typeof(Fishes)))
        {
            fishDictionary.Add(fish, false);
        }
    }

    public bool HasNewFishBeenCaught(Fishes fish)
    {
        if(fishDictionary[fish] != true)
        {
            fishDictionary[fish] = true;
            return true;
        }
        else
        {
            return false;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    public int Roll()
    {
        int value = Random.Range(1, 7);
        Debug.Log("Dice rolled: " + value);
        return value;


    }
}




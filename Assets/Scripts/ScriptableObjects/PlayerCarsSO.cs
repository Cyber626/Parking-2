using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCarsSO : ScriptableObject
{
    public CarCollection[] playerCars;

    [Serializable]
    public class CarCollection
    {
        public GameObject[] cars;
    }
}

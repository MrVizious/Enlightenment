using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    float minimumDistanceToSameSpawnable
    {
        get; set;
    }
    Vector3 GenerateVector();
}

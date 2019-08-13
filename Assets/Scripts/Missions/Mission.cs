using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : IMission
{
    public Mission(int difficulty)
    {
        Difficulty = difficulty;
    }

    public int Difficulty { get; private set; }
}

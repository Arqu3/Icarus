using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSingleton : MonoSingleton<MissionSingleton>
{
    public void StartNew(IMission mission)
    {
        Current = mission;
    }

    public IMission Current { get; private set; }
}

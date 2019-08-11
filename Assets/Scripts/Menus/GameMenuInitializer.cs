using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuInitializer : MonoBehaviour
{
    void Start()
    {
        var ui = new HealthAndResourceUI();
        ui.Show();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthDecorator
{
    BaseEntityHealthProvider provider { get; set; }
}

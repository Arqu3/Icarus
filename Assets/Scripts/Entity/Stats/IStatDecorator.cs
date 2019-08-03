using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatDecorator
{
    BaseStatProvider provider { get; set; }
}

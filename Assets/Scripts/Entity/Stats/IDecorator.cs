using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecorator<T>
{
    T provider { get; set; }
}

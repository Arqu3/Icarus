using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceDecorator
{
    BaseEntityResourceProvider provider { get; set; }
}

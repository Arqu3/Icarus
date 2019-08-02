using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorConversions {

	public static string ToHex(this Color color)
	{
		return ((int)(color.r * 255)).ToString("x2") + ((int)(color.g * 255)).ToString("x2") + ((int)(color.b * 255)).ToString("x2");
	}
}

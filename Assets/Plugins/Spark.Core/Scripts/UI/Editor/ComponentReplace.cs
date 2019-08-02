using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Spark.UI
{
	public static class ComponentReplace
	{
		[MenuItem("CONTEXT/Text/Convert to TextMesh")]
		private static void ReplaceText(MenuCommand command)
		{
			var text = command.context as Text;
			var go = text.gameObject;
			
			var str = text.text;
			var fontSize = text.fontSize;
			var alignment = text.alignment;
			var color = text.color;
			
			var rect = text.GetComponent<RectTransform>();
			var r = new
			{
				rect.anchoredPosition,
				rect.sizeDelta
			};

			Undo.DestroyObjectImmediate(text);
			var textPro = go.AddComponent<TMPro.TextMeshProUGUI>();
			textPro.text = str;
			textPro.fontSize = fontSize;
			textPro.color = color;
			rect.anchoredPosition = r.anchoredPosition;
			rect.sizeDelta = r.sizeDelta;
			switch (alignment)
			{
				case TextAnchor.LowerCenter:
					textPro.alignment = TMPro.TextAlignmentOptions.Bottom;
					break;
				case TextAnchor.LowerLeft:
					textPro.alignment = TMPro.TextAlignmentOptions.BottomLeft;
					break;
				case TextAnchor.LowerRight:
					textPro.alignment = TMPro.TextAlignmentOptions.BottomRight;
					break;
				case TextAnchor.MiddleCenter:
					textPro.alignment = TMPro.TextAlignmentOptions.Midline;
					break;
				case TextAnchor.MiddleLeft:
					textPro.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
					break;
				case TextAnchor.MiddleRight:
					textPro.alignment = TMPro.TextAlignmentOptions.MidlineRight;
					break;
				case TextAnchor.UpperCenter:
					textPro.alignment = TMPro.TextAlignmentOptions.Top;
					break;
				case TextAnchor.UpperLeft:
					textPro.alignment = TMPro.TextAlignmentOptions.TopLeft;
					break;
				case TextAnchor.UpperRight:
					textPro.alignment = TMPro.TextAlignmentOptions.TopRight;
					break;
			}
			Undo.RegisterCreatedObjectUndo(textPro, "TextMeshPro");
		}
	}

}

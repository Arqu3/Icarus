using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

namespace Spark.UI
{
	public class KeyBinding : Selectable, IPointerClickHandler, ISubmitHandler, IEventSystemHandler, ICancelHandler
	{
		
		public string keyName
		{
			get { return _keyName; }
			set
			{
				_keyName = value;
				switch (currentSelectionState)
				{
					case SelectionState.Disabled:
						keyText.text = string.Format(disabledFormat, _keyName);
						break;
					case SelectionState.Pressed:
						keyText.text = string.Format(pressedFormat, _keyName);
						break;
					case SelectionState.Highlighted:
						keyText.text = string.Format(highlightedFormat, _keyName);
						break;
					default:
						keyText.text = string.Format(normalFormat, _keyName);
						break;
				}
			}
		}
		public string normalFormat = "{0}";
		public string highlightedFormat = "{0}";
		public string pressedFormat = "<{0}>";
		public string disabledFormat = "{0}";

		[SerializeField]
		private TMP_Text keyText;

		private string _keyName;
		private Coroutine activeInputSequence;
		private Navigation.Mode storedNavMode;

		public void OnPointerClick(PointerEventData eventData)
		{
			activeInputSequence = StartCoroutine(InputSequence());
		}

		public void OnSubmit(BaseEventData eventData)
		{
			activeInputSequence = StartCoroutine(InputSequence());
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			StopInputIfRunning();
		}


		public void OnCancel(BaseEventData eventData)
		{
			StopInputIfRunning();
		}

		private void StopInputIfRunning()
		{
			if (activeInputSequence != null)
			{
				StopCoroutine(activeInputSequence);
				var nav = navigation;
				nav.mode = storedNavMode;
				navigation = nav;
				switch (currentSelectionState)
				{
					case SelectionState.Disabled:
						keyText.text = string.Format(disabledFormat, _keyName);
						break;
					case SelectionState.Pressed:
						keyText.text = string.Format(pressedFormat, _keyName);
						break;
					case SelectionState.Highlighted:
						keyText.text = string.Format(highlightedFormat, _keyName);
						break;
					default:
						keyText.text = string.Format(normalFormat, _keyName);
						break;
				}
			}
		}


		private IEnumerator InputSequence()
		{
			var nav = navigation;
			storedNavMode = nav.mode;
			nav.mode = Navigation.Mode.None;
			navigation = nav;

			keyText.text = string.Format(pressedFormat, keyName);
			while(true)yield break;
		}
	}
}


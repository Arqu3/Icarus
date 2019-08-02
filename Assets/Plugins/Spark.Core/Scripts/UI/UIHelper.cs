using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Spark.UI
{
	public static class UIHelper
	{
		public static EventTrigger AddEventTrigger(this Selectable element)
		{
			EventTrigger et = element.GetComponent<EventTrigger>();
			if (!et) et = element.gameObject.AddComponent<EventTrigger>();
			return et;
		}

		public static void AddTriggerEvent(this EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction function)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = triggerType;

			entry.callback.AddListener((data) =>
			{
				function.Invoke();
			});
			eventTrigger.triggers.Add(entry);
		}
	}
}
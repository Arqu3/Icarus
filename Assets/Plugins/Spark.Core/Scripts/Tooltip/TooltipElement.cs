using UnityEngine;
using UnityEngine.EventSystems;
namespace Spark.Tooltip
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class TooltipElement : MonoBehaviour
	{
		public abstract string GetTooltipText();

		private bool isShowing = false;
		private void Start()
		{
			EventTrigger s = GetComponent<EventTrigger>();
			if (!s)
				s = gameObject.AddComponent<EventTrigger>();

			var enter = new EventTrigger.TriggerEvent();
			enter.AddListener(OnPointerEnter);
			var exit = new EventTrigger.TriggerEvent();
			exit.AddListener(OnPointerExit);

			s.triggers.Add(new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerEnter,
				callback = enter
			});
			s.triggers.Add(new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerExit,
				callback = exit
			});
		}

		protected void RefreshIfShowing()
		{
			if (isShowing)
			{
				TooltipManager.Instance.UpdateText(GetTooltipText());
			}
		}


		public void OnPointerEnter(BaseEventData eventData)
		{
			var r = GetComponent<RectTransform>();
			isShowing = true;
			if (r.position.x - Screen.width / 2 > 0)
			{
				TooltipManager.Instance.SetTooltip(GetTooltipText(), r.position - Vector3.right * r.rect.width * r.lossyScale.x, true);
			}
			else
			{
				TooltipManager.Instance.SetTooltip(GetTooltipText(), r.position + Vector3.right * r.rect.width * r.lossyScale.x);
			}

		}

		public void OnPointerExit(BaseEventData eventData)
		{
			isShowing = false;
			TooltipManager.Instance.ClearTooltip();
		}

		private void OnDisable()
		{
			if (isShowing)
				TooltipManager.Instance.ClearTooltip();
		}
	}
}
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
namespace Spark.UI
{
	public abstract class InstantiatableCanvas : MonoBehaviour
	{
		public readonly UnityEvent onDestroy = new UnityEvent();

		public RectTransform rectTransform { get; private set; }

		public const string CanvasResourcePath = "UI/Instantiatable/{0}";
		public static string FullResourcePath
		{
			get
			{
				return Application.dataPath + "/Resources/" + CanvasResourcePath;
			}
		}
		public static string RelativePath
		{
			get
			{
				return "Assets/Resources/" + CanvasResourcePath;
			}
		}

		public static string NameToResourcePath(string resourceName)
		{
			return string.Format(CanvasResourcePath, resourceName);
		}

		private void OnValidate()
		{
			gameObject.name = GetType().Name;
		}
		protected virtual void OnDestroy()
		{
			onDestroy.Invoke();
		}
#if UNITY_EDITOR
		[ContextMenu("Force Save")]
		private void ForceSave()
		{
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
			UnityEditor.SceneManagement.EditorSceneManager.SaveScene(gameObject.scene);
		}
#endif

		public void CreateTrigger(GameObject obj, UnityEngine.EventSystems.EventTriggerType triggerType, UnityAction<UnityEngine.EventSystems.BaseEventData> action)
		{
			var trigger = obj.AddComponent<UnityEngine.EventSystems.EventTrigger>();
			var tr = trigger.triggers.Find(e => e.eventID == triggerType);
			if (tr != null)
			{
				tr.callback.AddListener(action);
			}
			else
			{
				var evt = new UnityEngine.EventSystems.EventTrigger.TriggerEvent();
				evt.AddListener(action);
				trigger.triggers.Add(new UnityEngine.EventSystems.EventTrigger.Entry { eventID = triggerType, callback = evt });

			}
		}

		public void Init()
		{
			rectTransform = GetComponent<RectTransform>();
		}
	}
}

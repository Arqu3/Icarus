using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Spark.UI
{
	public abstract class InstantiatableUIBase : IDisplayable
	{
		public abstract bool IsShowing { get; }

		/// <summary>
		/// Called after the canvas has been activated
		/// </summary>
		public readonly UnityEvent onShow = new UnityEvent();

		/// <summary>
		/// Called before the canvas is deactivated
		/// </summary>
		public readonly UnityEvent onHide = new UnityEvent();

		/// <summary>
		/// Called before the canvas is destroyed
		/// </summary>
		public readonly UnityEvent onDestroy = new UnityEvent();

		public abstract void DontDestroyOnLoad();

		public virtual void Dispose()
		{
			onDestroy.Invoke();
		}
		public virtual void Hide()
		{
			onHide.Invoke();
		}
		public virtual void Show()
		{
			onShow.Invoke();
		}

		/// <summary>
		/// Listen to the event of an external source for the lifeTime of this ui
		/// </summary>
		/// <param name="evt"></param>
		/// <param name="action"></param>
		protected void AddExternalListener(UnityEvent evt, UnityAction action)
		{
			evt.AddListener(action);
			onDestroy.AddListener(() => evt.RemoveListener(action));
		}
		protected void AddExternalListener<T>(UnityEvent<T> evt, UnityAction action)
		{
			UnityAction<T> act = (i) => action();
			evt.AddListener(act);
			onDestroy.AddListener(() => evt.RemoveListener(act));
		}
		protected void AddExternalListener<T>(UnityEvent<T> evt, UnityAction<T> action)
		{
			evt.AddListener(action);
			onDestroy.AddListener(() => evt.RemoveListener(action));
		}
		/// <summary>
		/// Link a button, toggle etc. event to an event
		/// </summary>
		/// <typeparam name="TSelectable"></typeparam>
		/// <typeparam name="UEventType"></typeparam>
		/// <param name="selectable"></param>
		/// <param name="selector"></param>
		/// <param name="target"></param>
		protected void HookSelectableEvent<TSelectable, UEventType>(TSelectable selectable, System.Func<TSelectable, UnityEvent<UEventType>> selector, UnityEvent<UEventType> target) where TSelectable : Selectable where UEventType : struct
		{
			if (selectable == null)
			{
				Debug.LogError("A selectable is null in  " + GetType().Name + " (type: " + typeof(TSelectable).Name + ")");
				return;
			}
			selector(selectable).AddListener(target.Invoke);
		}
		protected void HookSelectableEvent<TSelectable>(TSelectable selectable, System.Func<TSelectable, UnityEvent> selector, UnityEvent target) where TSelectable : Selectable
		{
			if (selectable == null)
			{
				Debug.LogError("A selectable is null in  " + GetType().Name + " (type: " + typeof(TSelectable).Name + ")");
				return;
			}
			selector(selectable).AddListener(target.Invoke);
		}
		protected void HookSelectableEvent<TObject, UEventType>(TObject selectable, System.Func<TObject, UnityEvent<UEventType>> selector, UnityAction<UEventType> target) where TObject : Object
		{
			if (selectable == null)
			{
				Debug.LogError("A selectable is null in  " + GetType().Name + " (type: " + typeof(TObject).Name + ")");
				return;
			}
			selector(selectable).AddListener(target.Invoke);
		}
		protected void HookSelectableEvent<TObject>(TObject selectable, System.Func<TObject, UnityEvent> selector, UnityAction act) where TObject : Object
		{
			if (selectable == null)
			{
				Debug.LogError("A selectable is null in  " + GetType().Name + " (type: " + typeof(TObject).Name + ")");
				return;
			}
			selector(selectable).AddListener(act);
		}

		protected void HookSelectableEvent(Button button, UnityEvent target)
		{
			HookSelectableEvent(button, b => b.onClick, target);
		}

		protected void HookSelectableEvent(Button button, UnityAction act)
		{
			HookSelectableEvent(button, b => b.onClick, act);
		}
	}
}


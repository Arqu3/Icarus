// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.BaseInputModule
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Spark.EventSystems
{
	/// <summary>
	///   <para>A base module that raises events and sends them to GameObjects.</para>
	/// </summary>
	[RequireComponent(typeof(EventSystem))]
	public abstract class SparkBaseInputModule : UIBehaviour
	{
		[NonSerialized]
		protected List<RaycastResult> m_RaycastResultCache = new List<RaycastResult>();
		private AxisEventData m_AxisEventData;
		private EventSystem m_EventSystem;
		private BaseEventData m_BaseEventData;

		protected EventSystem eventSystem
		{
			get
			{
				return this.m_EventSystem;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_EventSystem = this.GetComponent<EventSystem>();
			this.m_EventSystem.UpdateModules();
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			this.m_EventSystem.UpdateModules();
			base.OnDisable();
		}

		/// <summary>
		///   <para>Process the current tick for the module.</para>
		/// </summary>
		public abstract void Process();

		protected static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			for (int index = 0; index < candidates.Count; ++index)
			{
				if (!((UnityEngine.Object)candidates[index].gameObject == (UnityEngine.Object)null))
					return candidates[index];
			}
			return new RaycastResult();
		}

		/// <summary>
		///   <para>Given an input movement, determine the best MoveDirection.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="deadZone">Dead zone.</param>
		protected static MoveDirection DetermineMoveDirection(float x, float y)
		{
			return SparkBaseInputModule.DetermineMoveDirection(x, y, 0.6f);
		}

		/// <summary>
		///   <para>Given an input movement, determine the best MoveDirection.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="deadZone">Dead zone.</param>
		protected static MoveDirection DetermineMoveDirection(float x, float y, float deadZone)
		{
			if ((double)new Vector2(x, y).sqrMagnitude < (double)deadZone * (double)deadZone)
				return MoveDirection.None;
			if ((double)Mathf.Abs(x) > (double)Mathf.Abs(y))
				return (double)x > 0.0 ? MoveDirection.Right : MoveDirection.Left;
			return (double)y > 0.0 ? MoveDirection.Up : MoveDirection.Down;
		}

		/// <summary>
		///   <para>Given 2 GameObjects, return a common root GameObject (or null).</para>
		/// </summary>
		/// <param name="g1"></param>
		/// <param name="g2"></param>
		protected static GameObject FindCommonRoot(GameObject g1, GameObject g2)
		{
			if ((UnityEngine.Object)g1 == (UnityEngine.Object)null || (UnityEngine.Object)g2 == (UnityEngine.Object)null)
				return (GameObject)null;
			for (Transform transform1 = g1.transform; (UnityEngine.Object)transform1 != (UnityEngine.Object)null; transform1 = transform1.parent)
			{
				for (Transform transform2 = g2.transform; (UnityEngine.Object)transform2 != (UnityEngine.Object)null; transform2 = transform2.parent)
				{
					if ((UnityEngine.Object)transform1 == (UnityEngine.Object)transform2)
						return transform1.gameObject;
				}
			}
			return (GameObject)null;
		}

		/// <summary>
		///   <para>Handle sending enter and exit events when a new enter targer is found.</para>
		/// </summary>
		/// <param name="currentPointerData"></param>
		/// <param name="newEnterTarget"></param>
		protected void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
		{
			if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null || (UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)null)
			{
				for (int index = 0; index < currentPointerData.hovered.Count; ++index)
					ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[index], (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
				currentPointerData.hovered.Clear();
				if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null)
				{
					currentPointerData.pointerEnter = newEnterTarget;
					return;
				}
			}
			if ((UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)newEnterTarget && (bool)((UnityEngine.Object)newEnterTarget))
				return;
			GameObject commonRoot = SparkBaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
			if ((UnityEngine.Object)currentPointerData.pointerEnter != (UnityEngine.Object)null)
			{
				for (Transform transform = currentPointerData.pointerEnter.transform; (UnityEngine.Object)transform != (UnityEngine.Object)null && (!((UnityEngine.Object)commonRoot != (UnityEngine.Object)null) || !((UnityEngine.Object)commonRoot.transform == (UnityEngine.Object)transform)); transform = transform.parent)
				{
					ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
					currentPointerData.hovered.Remove(transform.gameObject);
				}
			}
			currentPointerData.pointerEnter = newEnterTarget;
			if (!((UnityEngine.Object)newEnterTarget != (UnityEngine.Object)null))
				return;
			for (Transform transform = newEnterTarget.transform; (UnityEngine.Object)transform != (UnityEngine.Object)null && (UnityEngine.Object)transform.gameObject != (UnityEngine.Object)commonRoot; transform = transform.parent)
			{
				ExecuteEvents.Execute<IPointerEnterHandler>(transform.gameObject, (BaseEventData)currentPointerData, ExecuteEvents.pointerEnterHandler);
				currentPointerData.hovered.Add(transform.gameObject);
			}
		}

		/// <summary>
		///   <para>Given some input data generate an AxisEventData that can be used by the event system.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="moveDeadZone">Dead Zone.</param>
		protected virtual AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
		{
			if (this.m_AxisEventData == null)
				this.m_AxisEventData = new AxisEventData(this.eventSystem);
			this.m_AxisEventData.Reset();
			this.m_AxisEventData.moveVector = new Vector2(x, y);
			this.m_AxisEventData.moveDir = SparkBaseInputModule.DetermineMoveDirection(x, y, moveDeadZone);
			return this.m_AxisEventData;
		}

		/// <summary>
		///   <para>Generate a BaseEventData that can be used by the EventSystem.</para>
		/// </summary>
		protected virtual BaseEventData GetBaseEventData()
		{
			if (this.m_BaseEventData == null)
				this.m_BaseEventData = new BaseEventData(this.eventSystem);
			this.m_BaseEventData.Reset();
			return this.m_BaseEventData;
		}

		/// <summary>
		///   <para>Is the pointer with the given ID over an EventSystem object?</para>
		/// </summary>
		/// <param name="pointerId">Pointer ID.</param>
		public virtual bool IsPointerOverGameObject(int pointerId)
		{
			return false;
		}

		/// <summary>
		///   <para>Should be activated.</para>
		/// </summary>
		/// <returns>
		///   <para>Should the module be activated.</para>
		/// </returns>
		public virtual bool ShouldActivateModule()
		{
			if (this.enabled)
				return this.gameObject.activeInHierarchy;
			return false;
		}

		/// <summary>
		///   <para>Called when the module is deactivated. Override this if you want custom code to execute when you deactivate your module.</para>
		/// </summary>
		public virtual void DeactivateModule()
		{
		}

		/// <summary>
		///   <para>Called when the module is activated. Override this if you want custom code to execute when you activate your module.</para>
		/// </summary>
		public virtual void ActivateModule()
		{
		}

		/// <summary>
		///   <para>Update the internal state of the Module.</para>
		/// </summary>
		public virtual void UpdateModule()
		{
		}

		/// <summary>
		///   <para>Check to see if the module is supported. Override this if you have a platfrom specific module (eg. TouchInputModule that you do not want to activate on standalone.</para>
		/// </summary>
		/// <returns>
		///   <para>Is the module supported.</para>
		/// </returns>
		public virtual bool IsModuleSupported()
		{
			return true;
		}
	}
}

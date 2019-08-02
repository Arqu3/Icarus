// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.PointerInputModule
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Spark.EventSystems
{
	/// <summary>
	///   <para>A BaseInputModule for pointer input.</para>
	/// </summary>
	public abstract class SparkPointerInputModule : BaseInputModule
	{
		protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData>();
		private readonly SparkPointerInputModule.MouseState m_MouseState = new SparkPointerInputModule.MouseState();
		/// <summary>
		///   <para>Id of the cached left mouse pointer event.</para>
		/// </summary>
		public const int kMouseLeftId = -1;
		/// <summary>
		///   <para>Id of the cached right mouse pointer event.</para>
		/// </summary>
		public const int kMouseRightId = -2;
		/// <summary>
		///   <para>Id of the cached middle mouse pointer event.</para>
		/// </summary>
		public const int kMouseMiddleId = -3;
		/// <summary>
		///   <para>Touch id for when simulating touches on a non touch device.</para>
		/// </summary>
		public const int kFakeTouchesId = -4;


		protected GameObject lastSelectable;

		protected bool GetPointerData(int id, out PointerEventData data, bool create)
		{
			if (this.m_PointerData.TryGetValue(id, out data) || !create)
				return false;
			data = new PointerEventData(this.eventSystem)
			{
				pointerId = id
			};
			this.m_PointerData.Add(id, data);
			return true;
		}

		/// <summary>
		///   <para>Remove the PointerEventData from the cache.</para>
		/// </summary>
		/// <param name="data"></param>
		protected void RemovePointerData(PointerEventData data)
		{
			this.m_PointerData.Remove(data.pointerId);
		}

		protected PointerEventData GetTouchPointerEventData(Touch input, out bool pressed, out bool released)
		{
			PointerEventData data;
			bool pointerData = this.GetPointerData(input.fingerId, out data, true);
			data.Reset();
			pressed = pointerData || input.phase == TouchPhase.Began;
			released = input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended;
			if (pointerData)
				data.position = input.position;
			data.delta = !pressed ? input.position - data.position : Vector2.zero;
			data.position = input.position;
			data.button = PointerEventData.InputButton.Left;
			this.eventSystem.RaycastAll(data, this.m_RaycastResultCache);
			RaycastResult firstRaycast = SparkPointerInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			data.pointerCurrentRaycast = firstRaycast;
			this.m_RaycastResultCache.Clear();
			return data;
		}

		/// <summary>
		///   <para>Copy one PointerEventData to another.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		/// <summary>
		///   <para>Given a mouse button return the current state for the frame.</para>
		/// </summary>
		/// <param name="buttonId">Mouse Button id.</param>
		protected static PointerEventData.FramePressState StateForMouseButton(int buttonId)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(buttonId);
			bool mouseButtonUp = Input.GetMouseButtonUp(buttonId);
			if (mouseButtonDown && mouseButtonUp)
				return PointerEventData.FramePressState.PressedAndReleased;
			if (mouseButtonDown)
				return PointerEventData.FramePressState.Pressed;
			return mouseButtonUp ? PointerEventData.FramePressState.Released : PointerEventData.FramePressState.NotChanged;
		}

		/// <summary>
		///   <para>Return the current MouseState.</para>
		/// </summary>
		/// <param name="id"></param>
		protected virtual SparkPointerInputModule.MouseState GetMousePointerEventData()
		{
			return this.GetMousePointerEventData(0);
		}

		/// <summary>
		///   <para>Return the current MouseState.</para>
		/// </summary>
		/// <param name="id"></param>
		protected virtual SparkPointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData data1;
			bool pointerData = this.GetPointerData(-1, out data1, true);
			data1.Reset();
			if (pointerData)
				data1.position = (Vector2)Input.mousePosition;
			Vector2 mousePosition = (Vector2)Input.mousePosition;
			data1.delta = mousePosition - data1.position;
			data1.position = mousePosition;
			data1.scrollDelta = Input.mouseScrollDelta;
			data1.button = PointerEventData.InputButton.Left;
			this.eventSystem.RaycastAll(data1, this.m_RaycastResultCache);
			RaycastResult firstRaycast = SparkPointerInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			data1.pointerCurrentRaycast = firstRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData data2;
			this.GetPointerData(-2, out data2, true);
			this.CopyFromTo(data1, data2);
			data2.button = PointerEventData.InputButton.Right;
			PointerEventData data3;
			this.GetPointerData(-3, out data3, true);
			this.CopyFromTo(data1, data3);
			data3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, SparkPointerInputModule.StateForMouseButton(0), data1);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, SparkPointerInputModule.StateForMouseButton(1), data2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, SparkPointerInputModule.StateForMouseButton(2), data3);
			return this.m_MouseState;
		}

		/// <summary>
		///   <para>Return the last PointerEventData for the given touch / mouse id.</para>
		/// </summary>
		/// <param name="id"></param>
		protected PointerEventData GetLastPointerEventData(int id)
		{
			PointerEventData data;
			this.GetPointerData(id, out data, false);
			return data;
		}

		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			if (!useDragThreshold)
				return true;
			return (double)(pressPos - currentPos).sqrMagnitude >= (double)threshold * (double)threshold;
		}

		/// <summary>
		///   <para>Process movement for the current frame with the given pointer event.</para>
		/// </summary>
		/// <param name="pointerEvent"></param>
		protected virtual void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			this.HandlePointerExitAndEnterSpecial(pointerEvent, gameObject);
		}

		protected void HandlePointerExitAndEnterSpecial(PointerEventData currentPointerData, GameObject newEnterTarget)
		{
			if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null || (UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)null)
			{
				for (int index = 0; index < currentPointerData.hovered.Count; ++index)
					ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[index], (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
				currentPointerData.hovered.Clear();
				if (newEnterTarget)
				{
					var c = newEnterTarget.GetComponentInParent<Selectable>();
					lastSelectable = c == null ? lastSelectable : c.gameObject;
				}
				if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null)
				{
					currentPointerData.pointerEnter = newEnterTarget;
					return;
				}
			}
			if ((UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)newEnterTarget && (bool)((UnityEngine.Object)newEnterTarget))
				return;
			GameObject commonRoot = BaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
			if ((UnityEngine.Object)currentPointerData.pointerEnter != (UnityEngine.Object)null)
			{
				for (Transform transform = currentPointerData.pointerEnter.transform; (UnityEngine.Object)transform != (UnityEngine.Object)null && (!((UnityEngine.Object)commonRoot != (UnityEngine.Object)null) || !((UnityEngine.Object)commonRoot.transform == (UnityEngine.Object)transform)); transform = transform.parent)
				{
					ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
					currentPointerData.hovered.Remove(transform.gameObject);
				}
				eventSystem.SetSelectedGameObject(null);
				if (newEnterTarget)
				{
					var c = newEnterTarget.GetComponentInParent<Selectable>();
					lastSelectable = c == null ? lastSelectable : c.gameObject;
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
		///   <para>Process the drag for the current frame with the given pointer event.</para>
		/// </summary>
		/// <param name="pointerEvent"></param>
		protected virtual void ProcessDrag(PointerEventData pointerEvent)
		{
			bool flag = pointerEvent.IsPointerMoving();
			if (flag && (Object)pointerEvent.pointerDrag != (Object)null && (!pointerEvent.dragging && SparkPointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)this.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold)))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (!pointerEvent.dragging || !flag || !((Object)pointerEvent.pointerDrag != (Object)null))
				return;
			if ((Object)pointerEvent.pointerPress != (Object)pointerEvent.pointerDrag)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerUpHandler);
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = (GameObject)null;
				pointerEvent.rawPointerPress = (GameObject)null;
			}
			ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.dragHandler);
		}

		public override bool IsPointerOverGameObject(int pointerId)
		{
			PointerEventData pointerEventData = this.GetLastPointerEventData(pointerId);
			if (pointerEventData != null)
				return (Object)pointerEventData.pointerEnter != (Object)null;
			return false;
		}

		/// <summary>
		///   <para>Clear all pointers and deselect any selected objects in the EventSystem.</para>
		/// </summary>
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			using (Dictionary<int, PointerEventData>.ValueCollection.Enumerator enumerator = this.m_PointerData.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
					this.HandlePointerExitAndEnter(enumerator.Current, (GameObject)null);
			}
			this.m_PointerData.Clear();
			this.eventSystem.SetSelectedGameObject((GameObject)null, baseEventData);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + (object)this.GetType());
			stringBuilder.AppendLine();
			using (Dictionary<int, PointerEventData>.Enumerator enumerator = this.m_PointerData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, PointerEventData> current = enumerator.Current;
					if (current.Value != null)
					{
						stringBuilder.AppendLine("<B>Pointer:</b> " + (object)current.Key);
						stringBuilder.AppendLine(current.Value.ToString());
					}
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		///   <para>Deselect the current selected GameObject if the currently pointed-at GameObject is different.</para>
		/// </summary>
		/// <param name="currentOverGo">The GameObject the pointer is currently over.</param>
		/// <param name="pointerEvent">Current event data.</param>
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (!((Object)ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != (Object)this.eventSystem.currentSelectedGameObject))
				return;
			this.eventSystem.SetSelectedGameObject((GameObject)null, pointerEvent);
		}

		protected class ButtonState
		{
			private PointerEventData.InputButton m_Button;
			private SparkPointerInputModule.MouseButtonEventData m_EventData;

			public SparkPointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			public PointerEventData.InputButton button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}
		}

		protected class MouseState
		{
			private List<SparkPointerInputModule.ButtonState> m_TrackedButtons = new List<SparkPointerInputModule.ButtonState>();

			public bool AnyPressesThisFrame()
			{
				for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
				{
					if (this.m_TrackedButtons[index].eventData.PressedThisFrame())
						return true;
				}
				return false;
			}

			public bool AnyReleasesThisFrame()
			{
				for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
				{
					if (this.m_TrackedButtons[index].eventData.ReleasedThisFrame())
						return true;
				}
				return false;
			}

			public SparkPointerInputModule.ButtonState GetButtonState(PointerEventData.InputButton button)
			{
				SparkPointerInputModule.ButtonState buttonState = (SparkPointerInputModule.ButtonState)null;
				for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
				{
					if (this.m_TrackedButtons[index].button == button)
					{
						buttonState = this.m_TrackedButtons[index];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new SparkPointerInputModule.ButtonState()
					{
						button = button,
						eventData = new SparkPointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			public void SetButtonState(PointerEventData.InputButton button, PointerEventData.FramePressState stateForMouseButton, PointerEventData data)
			{
				SparkPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}
		}

		/// <summary>
		///   <para>Information about a mouse button event.</para>
		/// </summary>
		public class MouseButtonEventData
		{
			/// <summary>
			///   <para>The state of the button this frame.</para>
			/// </summary>
			public PointerEventData.FramePressState buttonState;
			/// <summary>
			///   <para>Pointer data associated with the mouse event.</para>
			/// </summary>
			public PointerEventData buttonData;

			/// <summary>
			///   <para>Was the button pressed this frame?</para>
			/// </summary>
			public bool PressedThisFrame()
			{
				if (this.buttonState != PointerEventData.FramePressState.Pressed)
					return this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
				return true;
			}

			/// <summary>
			///   <para>Was the button released this frame?</para>
			/// </summary>
			public bool ReleasedThisFrame()
			{
				if (this.buttonState != PointerEventData.FramePressState.Released)
					return this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
				return true;
			}
		}
	}
}
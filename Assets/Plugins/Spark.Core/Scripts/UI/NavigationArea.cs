//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//using UnityEngine.EventSystems;
//using System;
//using Spark.Services.Input;
//using Spark.Services;
//using Spark.EventSystems;

//namespace Spark.UI
//{
//	[AddComponentMenu("UI/Navigation Area")]
//	[RequireComponent(typeof(RectTransform))]
//	public class NavigationArea : MonoBehaviour
//	{
//		public enum AssumeControlMode
//		{
//			None,
//			AssumeIfNone,
//			ForceAssume
//		}
//		public enum TabSwitchMode
//		{
//			None,
//			Dynamic,
//			Explicit
//		}

//		public NavMode navMode = NavMode.Vertical;
//		public bool loop = true;
//		public bool dynamic = false;
//		public int priority;
//		public AssumeControlMode assumeControlMode;
//		public GameObject defaultSelectionTarget;
//		public TabSwitchMode tabSwitchMode = TabSwitchMode.Dynamic;
//		public NavigationArea onTabSwitchTarget;


//		private void Start()
//		{
//			if (dynamic) Recalculate();
//		}

//		private void OnValidate()
//		{
//			var s = GetComponentInChildren<Selectable>(true);
//			defaultSelectionTarget = s ? s.gameObject : null;
//			if (dynamic) return;
//			Recalculate();
//		}

//		private void OnEnable()
//		{
//			Recalculate();
//			switch (assumeControlMode)
//			{
//				case AssumeControlMode.AssumeIfNone:
//					if (!EventSystemSingleton.Instance.inputModule.currentNavArea)
//						goto case AssumeControlMode.ForceAssume;
//					else
//						break;
//				case AssumeControlMode.ForceAssume:
//					EventSystemSingleton.Instance.inputModule.SetNavigationArea(this);
//					break;
//			}
//		}

//		private void OnDisable()
//		{
//			if (!EventSystemSingleton.Instance) return;
//			if(EventSystemSingleton.Instance.inputModule.currentNavArea.Equals(this))
//				EventSystemSingleton.Instance.inputModule.SetNavigationArea(null);
//		}

//		[ContextMenu("Recalculate")]
//		public void Recalculate()
//		{
//			var sel = GetComponentsInChildren<Selectable>(false);

//			for (int i = 0; i < sel.Length; i++)
//			{
//				var s = sel[i];
//				var nav = s.navigation;

//				nav.mode = UnityEngine.UI.Navigation.Mode.Explicit;

//				var prevIndex = (i - 1);
//				if (prevIndex < 0 && loop) prevIndex = sel.Length - 1;
//				var nextIndex = loop ? (i + 1) % sel.Length : (i+1);

//				switch (navMode)
//				{
//					case NavMode.Horizontal:
//						nav.selectOnDown = null;
//						nav.selectOnUp = null;
//						nav.selectOnRight = nextIndex >= sel.Length && !loop ? null : sel[nextIndex];
//						nav.selectOnLeft = prevIndex < 0 && !loop ? null : sel[prevIndex];
//						break;
//					case NavMode.Vertical:
//						nav.selectOnDown = nextIndex >= sel.Length && !loop ? null : sel[nextIndex];
//						nav.selectOnUp = prevIndex < 0 && !loop ? null : sel[prevIndex];
//						nav.selectOnRight = null;
//						nav.selectOnLeft = null;
//						break;
//				}
//				s.navigation = nav;
//			}
//		}

//	}


//	public enum NavMode
//	{
//		Horizontal,
//		Vertical,
//		Custom
//	}

	

//}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace Spark.UI
{
	public abstract class InstantiatableUI<HudT, CanvasT> : InstantiatableUIBase where HudT : InstantiatableUI<HudT, CanvasT> where CanvasT : InstantiatableCanvas
	{
		public string CanvasName
		{
			get
			{
				return typeof(CanvasT).Name;
			}
		}

		public override bool IsShowing
		{
			get
			{
				return Canvas.gameObject.activeSelf;
			}
		}

		protected CanvasT Canvas
		{
			get; private set;
		}

		private static Dictionary<int, InstantiatableCanvas> canvasResources;


		public InstantiatableUI(UnityAction<HudT> configure)
		{
			InstantiateCanvas(string.Format(InstantiatableCanvas.CanvasResourcePath, CanvasName));

			if (configure != null)
				configure(this as HudT);

			Canvas.gameObject.SetActive(false);
		}

		private void InstantiateCanvas(string resourcePath)
		{
			var inst = Object.Instantiate(Resources.Load<InstantiatableCanvas>(resourcePath));
			Debug.Assert(inst.GetType().Equals(typeof(CanvasT)), "Instantiated canvas (" + resourcePath + ") is not of type [" + typeof(CanvasT) + "]");
			Canvas = inst as CanvasT;
			Canvas.onDestroy.AddListener(onDestroy.Invoke);
			Canvas.Init();
		}

		public override void DontDestroyOnLoad()
		{
			Object.DontDestroyOnLoad(Canvas);
		}

		public override void Show()
		{
			Canvas.gameObject.SetActive(true);
			base.Show();

		}

		public override void Hide()
		{
			base.Hide();
			Canvas.gameObject.SetActive(false);
		}

		public override void Dispose()
		{
			Canvas.onDestroy.RemoveListener(onDestroy.Invoke);
			onDestroy.Invoke();
			//Debug.Log( "disposed of" + CanvasName );
			if (Canvas && Canvas.gameObject) Object.Destroy(Canvas.gameObject);
			else Debug.LogWarning(CanvasName + " already destroyed");
		}

		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			return Canvas.StartCoroutine(routine);
		}

	}
}

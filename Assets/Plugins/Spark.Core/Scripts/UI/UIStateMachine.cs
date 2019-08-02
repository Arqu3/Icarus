using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.UI
{
	public class UIStateMachine
	{
		private readonly Stack<InstantiatableUIBase> uiStack;

		public UIStateMachine()
		{
			uiStack = new Stack<InstantiatableUIBase>();
		}

		public void Push(InstantiatableUIBase ui)
		{
			if (uiStack.Count > 0) uiStack.Peek().Hide();
			uiStack.Push(ui);
			ui.onHide.AddListener(Pop);
			ui.Show();
		}

		public void Pop()
		{
			if (uiStack.Count > 0)
			{
				var ui = uiStack.Peek();
				ui.onHide.RemoveListener(Pop);
				ui.Hide();
			}
			if (uiStack.Count > 0)
			{
				var ui = uiStack.Peek();
				ui.onHide.AddListener(Pop);
				ui.Show();
			}
		}
	}
}


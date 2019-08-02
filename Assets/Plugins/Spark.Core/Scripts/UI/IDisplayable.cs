using System;
namespace Spark.UI
{
	public interface IDisplayable : IDisposable
	{

		void Show();

		void Hide();
	}
}


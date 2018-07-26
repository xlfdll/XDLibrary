using System;
using System.Windows;
using Xlfdll.Windows.API;

namespace Xlfdll.Windows.Presentation
{
	public static class SystemFunctions
	{
		public static void PreventSleep(this Application application, Boolean includeDisplay)
		{
			PowerManagement.ExecutionState state
				= PowerManagement.ExecutionState.ES_SYSTEM_REQUIRED
				| PowerManagement.ExecutionState.ES_CONTINUOUS;

			if (includeDisplay)
			{
				state |= PowerManagement.ExecutionState.ES_DISPLAY_REQUIRED;
			}

			PowerManagement.SetThreadExecutionState(state);
		}
	}
}
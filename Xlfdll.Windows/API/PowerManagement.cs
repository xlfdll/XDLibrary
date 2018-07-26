using System;
using System.Runtime.InteropServices;

namespace Xlfdll.Windows.API
{
	public static class PowerManagement
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

		[Flags]
		public enum ExecutionState : UInt32
		{
			ES_CONTINUOUS = 0x80000000,
			ES_SYSTEM_REQUIRED = 0x00000001,
			ES_DISPLAY_REQUIRED = 0x00000002,
			ES_AWAYMODE_REQUIRED = 0x00000040,
			// Legacy flag, should not be used.
			ES_USER_PRESENT = 0x00000004
		}
	}
}
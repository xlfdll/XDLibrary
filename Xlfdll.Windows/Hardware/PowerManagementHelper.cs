using System;

using Xlfdll.Windows.API;

namespace Xlfdll.Windows.Hardware
{
    public static class PowerManagementHelper
    {
        public static void PreventSleep(Boolean includeDisplay)
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
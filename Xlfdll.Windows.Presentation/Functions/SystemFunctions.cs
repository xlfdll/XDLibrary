using System;
using System.Windows;

using Xlfdll.Windows.Hardware;

namespace Xlfdll.Windows.Presentation
{
    public static class SystemFunctions
    {
        public static void PreventSleep(this Application application, Boolean includeDisplay)
        {
            PowerManagementHelper.PreventSleep(includeDisplay);
        }
    }
}
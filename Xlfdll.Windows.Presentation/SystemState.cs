using System;
using System.Windows;

namespace Xlfdll.Windows.Presentation
{
    public static class SystemState
    {
        public static Size GetScreenResolution(PresentationSource presentationSource)
        {
            Double screenWidth = SystemParameters.PrimaryScreenWidth * presentationSource.CompositionTarget.TransformToDevice.M11;
            Double screenHeight = SystemParameters.PrimaryScreenHeight * presentationSource.CompositionTarget.TransformToDevice.M22;

            return new Size(screenWidth, screenHeight);
        }
    }
}
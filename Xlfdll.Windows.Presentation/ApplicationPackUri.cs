using System;

namespace Xlfdll.Windows.Presentation
{
    public class ApplicationPackUri : Uri
    {
        public ApplicationPackUri(String path)
            : base("pack://application:,,,/"
                  + (path[0] == '/' ? path.Remove(0, 1) : path))
        { }
    }
}
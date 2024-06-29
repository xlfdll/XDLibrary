using System.Text;

namespace Xlfdll.Text
{
    public static class AdditionalEncodings
    {
        public static Encoding UTF8WithoutBOM { get; } = new UTF8Encoding(false);
    }
}
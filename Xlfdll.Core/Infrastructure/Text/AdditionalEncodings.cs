using System.Text;

namespace Xlfdll.Core
{
	public static class AdditionalEncodings
	{
		public static Encoding UTF8WithoutBOM { get; } = new UTF8Encoding(false);
	}
}
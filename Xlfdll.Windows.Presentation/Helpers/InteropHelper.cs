using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Xlfdll.Windows.Presentation
{
    public static class InteropHelper
    {
        public static BitmapImage ConvertToImage(this Bitmap bitmap)
        {
            BitmapImage bitmapimage = new BitmapImage();

            using (MemoryStream memory = new MemoryStream()
            {
                Position = 0
            })
            {
                bitmap.Save(memory, ImageFormat.Bmp);

                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
            }

            return bitmapimage;
        }
    }
}
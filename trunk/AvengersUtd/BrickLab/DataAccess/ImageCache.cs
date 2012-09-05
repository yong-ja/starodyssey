using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AvengersUtd.BrickLab.DataAccess
{

    public class ImageCache
    {
        private static readonly Cache<long, CacheNode<Uri>> images;

        internal static void InitCache()
        {
            var files = Directory.EnumerateFiles(Path.Combine(Global.CurrentDir, Global.CacheDir));

            foreach (string filename in files)
            {
                FileInfo fileInfo = new FileInfo(filename);
                Uri localFileUri = new Uri(filename);
                BitmapImage image = new BitmapImage(localFileUri) { CacheOption = BitmapCacheOption.OnLoad};
                images.Add(image.GetHashCode(), new CacheNode<Uri>((int)fileInfo.Length, localFileUri));
            }
           
        }

        static ImageCache()
        {
            images = new Cache<long, CacheNode<Uri>>(10 * 1024 * 1024);
        }

        public void DownloadImages(IEnumerable<Uri> imageList)
        {
            foreach (BitmapImage image in from uri in imageList 
                                          where uri.IsAbsoluteUri 
                                          select new BitmapImage(uri))
            {
                image.DownloadCompleted += DownloadCompleted;
            }
        }

        private void DownloadCompleted(object sender, EventArgs e)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapImage image = (BitmapImage)sender;
            string filename = Path.GetFileName(image.UriSource.LocalPath);
            Guid photoId = Guid.NewGuid();
            string photolocation = Path.Combine(Global.CacheDir, filename);  //file name 
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var filestream = new FileStream(photolocation, FileMode.Create))
                encoder.Save(filestream);

            long size = new FileInfo(photolocation).Length;

            images.Add(image.GetHashCode(), new CacheNode<Uri>((int)size, image.UriSource));
            
        } 

    }
}

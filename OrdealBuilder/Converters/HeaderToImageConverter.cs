using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OrdealBuilder
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;

            if (string.IsNullOrEmpty(path))
                return null;

            var file = "Images/FileIcon.png";
            if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                file = "Images/FolderIcon.png";

            return new BitmapImage(new Uri($"pack://application:,,,/OrdealBuilder;component/{file}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

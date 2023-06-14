using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Converters
{
    public class LabelStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string styleKey = value as string;

                if (string.IsNullOrEmpty(styleKey))
                    return null;

                foreach (var mergedDictionary in Application.Current.Resources.MergedDictionaries)
                {
                    if (mergedDictionary != null && mergedDictionary.TryGetValue(styleKey, out object style))
                    {
                        return style;
                    }
                }

                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

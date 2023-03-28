using MessagerClient.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MessagerClient
{
    public class MessagerCheckIsCurrentUser : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean returnValue = false;
            User user = App.GetCurrent().GetAppUser();
            if (user is null)
                return returnValue;
            if (value is not User)
                return returnValue;
            return user.Equals(value as User);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.ValueConverter
{
    public class GameObjectTypeToColorConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (GameObjectType)value;

            switch (type)
            {
                case GameObjectType.Player1:
                    return new SolidColorBrush(Colors.Red);
                case GameObjectType.Player2:
                    return new SolidColorBrush(Colors.Green);
                case GameObjectType.Searcher:
                    return new SolidColorBrush(Colors.CornflowerBlue);
                case GameObjectType.Projectile:
                    return new SolidColorBrush(Colors.Black);
                case GameObjectType.Tank:
                    return new SolidColorBrush(Colors.Pink);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
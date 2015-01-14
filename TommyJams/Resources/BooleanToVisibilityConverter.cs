﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TommyJams.Resources
{
    /// <summary>
    /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
       {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo c)
          {
          return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
          }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo c)
          {
          return value is Visibility && (Visibility)value == Visibility.Visible;
          }
       }
}

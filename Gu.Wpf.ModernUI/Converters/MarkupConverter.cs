namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    using Gu.Wpf.ModernUI.Converters.TypeConverters;
    using Gu.Wpf.ModernUI.TypeConverters;

    /// <summary>
    /// Class implements a base for a typed value converter used as a markup extension. Override the Convert method in the inheriting class
    /// </summary>
    /// <typeparam name="TInput">Type of the expected input - value to be converted</typeparam>
    /// <typeparam name="TResult">Type of the result of the conversion</typeparam>
    public abstract class MarkupConverter<TInput, TResult> : MarkupExtension, IValueConverter
    {
        private static readonly ITypeConverter<TInput> inputTypeConverter = TypeConverterFactory.Create<TInput>();
        private static readonly ITypeConverter<TResult> resultTypeConverter = TypeConverterFactory.Create<TResult>();
        protected MarkupConverter()
        {
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (ModernUIHelper.IsInDesignMode)
            {
                if (parameter != null)
                {
                    throw new ArgumentException(string.Format("Converterparameter makes no sense for MarkupConverter. Parameter was: {0}", parameter));
                }
                if (!inputTypeConverter.IsValid(value))
                {
                    throw new ArgumentException("{0}.Convert() value is not valid", "value");
                }
            }
            if (inputTypeConverter.IsValid(value))
            {
                var convertTo = inputTypeConverter.ConvertTo(value, culture);
                return Convert(convertTo, culture);
            }
            return ConvertDefault();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (ModernUIHelper.IsInDesignMode)
            {
                if (parameter != null)
                {
                    throw new ArgumentException(string.Format("Converterparameter makes no sense for MarkupConverter. Parameter was: {0}", parameter));
                }
                if (!resultTypeConverter.IsValid(value))
                {
                    throw new ArgumentException("{0}.ConvertBack() value is not valid", "value");
                }
            }
            if (resultTypeConverter.CanConvertTo(value, culture))
            {
                var convertTo = resultTypeConverter.ConvertTo(value, culture);
                return ConvertBack(convertTo, culture);
            }
            return ConvertBackDefault();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        protected abstract TResult Convert(TInput value, CultureInfo culture);

        protected virtual TResult ConvertDefault()
        {
            return default(TResult);
        }

        protected abstract TInput ConvertBack(TResult value, CultureInfo culture);

        protected virtual TInput ConvertBackDefault()
        {
            return default(TInput);
        }
    }
}

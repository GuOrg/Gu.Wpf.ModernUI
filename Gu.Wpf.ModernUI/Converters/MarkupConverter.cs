namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Data;
    using System.Windows.Markup;

    using Converters.TypeConverters;

    using Internals;

    using TypeConverters;

    /// <summary>
    /// Class implements a base for a typed value converter used as a markup extension. Override the Convert method in the inheriting class
    /// </summary>
    /// <typeparam name="TInput">Type of the expected input - value to be converted</typeparam>
    /// <typeparam name="TResult">Type of the result of the conversion</typeparam>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupConverter<TInput, TResult> : MarkupExtension, IValueConverter
    {
        private static readonly ITypeConverter<TInput> inputTypeConverter = TypeConverterFactory.Create<TInput>();
        private static readonly ITypeConverter<TResult> resultTypeConverter = TypeConverterFactory.Create<TResult>();
        protected MarkupConverter()
        {
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VerifyValue(value, parameter);
            if (inputTypeConverter.IsValid(value))
            {
                var convertTo = inputTypeConverter.ConvertTo(value, culture);
                return Convert(convertTo, culture);
            }
            return ConvertDefault();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VerifyValue(value, parameter);
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

        private void VerifyValue(object value, object parameter, [CallerMemberName] string caller = null)
        {
            if (ModernUIHelper.IsInDesignMode)
            {
                if (parameter != null)
                {
                    throw new ArgumentException(string.Format("ConverterParameter makes no sense for MarkupConverter. Parameter was: {0} for converter of type {1}", parameter, GetType().Name));
                }
                if (!inputTypeConverter.IsValid(value))
                {
                    var message = string.Format(
                            "{0} value: {1} is not valid for converter of type: {2} from: {3} to {4}",
                            caller,
                            value,
                            GetType().Name,
                            typeof(TInput).Name,
                            typeof(TResult).Name);
                    throw new ArgumentException(message, "value");
                }
            }
        }
    }
}

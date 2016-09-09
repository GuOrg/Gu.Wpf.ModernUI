namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Data;
    using System.Windows.Markup;
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
            this.VerifyValue(value, parameter);
            if (inputTypeConverter.IsValid(value))
            {
                var convertTo = inputTypeConverter.ConvertTo(value, culture);
                return this.Convert(convertTo, culture);
            }
            return this.ConvertDefault();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            this.VerifyValue(value, parameter);
            if (resultTypeConverter.CanConvertTo(value, culture))
            {
                var convertTo = resultTypeConverter.ConvertTo(value, culture);
                return this.ConvertBack(convertTo, culture);
            }
            return this.ConvertBackDefault();
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
                    var message = $"ConverterParameter makes no sense for MarkupConverter. Parameter was: {parameter} for converter of type {this.GetType() .Name}";
                    throw new ArgumentException(
                        message);
                }
                if (!inputTypeConverter.IsValid(value))
                {
                    var message = $"{caller} value: {value} is not valid for converter of type: {this.GetType() .Name} from: {typeof(TInput).Name} to {typeof(TResult).Name}";
                    throw new ArgumentException(message, nameof(value));
                }
            }
        }
    }
}

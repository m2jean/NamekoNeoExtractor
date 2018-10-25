using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace NamekoPedia.src
{
    public class MultilingualValue<T>
    {
        public readonly T[] Values;
        public MultilingualValue(T[] values) { Values = values; }
        public MultilingualValue(List<T> raw) { Values = raw.ToArray<T>(); }
        public MultilingualValue(Dictionary<PediaLanguage, T> dict)
        {
            Values = new T[dict.Count];
            foreach(var kvp in dict)
            {
                Values[Array.FindIndex(LangConfig.LANGUAGE_LIST, a => a == kvp.Key)] = kvp.Value;
            }
        }
    }

    [ValueConversion(typeof(PediaLanguage),typeof(Object))]
    public class MultilingualConverter<T> : IValueConverter
    {
        private MultilingualValue<T> contents;
        public MultilingualConverter(MultilingualValue<T> values)
        {
            contents = values;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return contents.Values[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (PediaLanguage) Array.FindIndex(contents.Values,t=>(t.Equals(value)));
        }
    }

    public class MultilingualTextBlock : ContentControl
    {
        MultilingualValue<TextBlock> texts = new MultilingualValue<TextBlock>(
            new TextBlock[LangConfig.LANGUAGE_LIST.Length]);

        public MultilingualTextBlock()
        {
            Binding alignbdg = new Binding("HorizontalAlignment");
            alignbdg.Source = this;
            alignbdg.Mode = BindingMode.TwoWay;
            SetBinding(HorizontalContentAlignmentProperty, alignbdg);

            Binding fontsizebdg = new Binding("FontSize");
            fontsizebdg.Source = this;
            for (int i = 0; i < texts.Values.Length; ++i)
            {
                texts.Values[i] = new TextBlock();
                texts.Values[i].SetBinding(TextBlock.FontSizeProperty, fontsizebdg);
                texts.Values[i].SetBinding(TextBlock.HorizontalAlignmentProperty, alignbdg);
            }

            Binding langbdg = new Binding();
            langbdg.Source = this;
            langbdg.Path = new PropertyPath(MainWindow.PediaLanguageProperty);
            langbdg.Converter = new MultilingualConverter<TextBlock>(texts);
            SetBinding(ContentProperty, langbdg);
        }

        public static readonly DependencyProperty MultilingualTextsProperty =
            DependencyProperty.Register("MultilingualTexts", typeof(MultilingualValue<string>)
                , typeof(MultilingualTextBlock), new FrameworkPropertyMetadata(OnTextsChanged));
        public MultilingualValue<string> MultilingualTexts
        {
            get { return (MultilingualValue<string>)GetValue(MultilingualTextsProperty); }
            set { SetValue(MultilingualTextsProperty, value); }
        }
        private static void OnTextsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultilingualTextBlock mtb = (MultilingualTextBlock)d;
            for(int i = 0; i < mtb.MultilingualTexts.Values.Length; ++i)
            {
                mtb.texts.Values[i].Text = mtb.MultilingualTexts.Values[i];
            }
        }

        public static readonly DependencyProperty StringIDProperty =
            DependencyProperty.Register("StringID", typeof(StringID), typeof(MultilingualTextBlock)
                , new FrameworkPropertyMetadata(OnStringIDChanged));
        public StringID StringID
        {
            get { return (StringID)GetValue(StringIDProperty); }
            set { SetValue(StringIDProperty, value); }
        }
        private static void OnStringIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultilingualTextBlock mtb = (MultilingualTextBlock)d;
            mtb.MultilingualTexts = LangConfig.strings[mtb.StringID];
        }

        protected override Size MeasureOverride(Size available)
        {
            Size max = new Size(0, 0);
            Size measure;
            foreach (TextBlock tb in texts.Values)
            {
                tb.Measure(available);
                measure = tb.DesiredSize;
                if (measure.Width > max.Width) max.Width = measure.Width;
                if (measure.Height > max.Height) max.Height = measure.Height;
            }
            measure = base.MeasureOverride(available);
            if (measure.Width > max.Width) max.Width = measure.Width;
            if (measure.Height > max.Height) max.Height = measure.Height;
            //this.Width = max.Width;
            //this.Height = max.Height;
            return max;
        }
    }

    public class MultilingualImage : Image
    {
        public static readonly DependencyProperty MultilingualSourceProperty =
            DependencyProperty.Register("MultilingualSource", typeof(MultilingualValue<ImageSource>)
                , typeof(MultilingualImage), new FrameworkPropertyMetadata(OnSourceChanged));
        public MultilingualValue<ImageSource> MultilingualSource
        {
            get { return (MultilingualValue<ImageSource>) GetValue(MultilingualSourceProperty); }
            set { SetValue(MultilingualSourceProperty, value); }
        }
        
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultilingualImage img = (MultilingualImage)d;
            Binding bdg = new Binding();
            bdg.Source = img;
            bdg.Path = new PropertyPath(MainWindow.PediaLanguageProperty);
            bdg.Converter = new MultilingualConverter<ImageSource>(img.MultilingualSource);
            img.SetBinding(SourceProperty, bdg);
        }
    }
}

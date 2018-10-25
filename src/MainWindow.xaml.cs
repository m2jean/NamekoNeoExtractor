using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;

using Imazen.WebP;
using NamekoPedia.src;
using System.Globalization;

namespace NamekoPedia.src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty PediaLanguageProperty =
            DependencyProperty.RegisterAttached("PediaLanguage", typeof(PediaLanguage), typeof(UIElement)
                , new FrameworkPropertyMetadata(LangConfig.defaultlang
                    , FrameworkPropertyMetadataOptions.Inherits));
        public static PediaLanguage GetPediaLanguage(UIElement e)
            { return (PediaLanguage)e.GetValue(PediaLanguageProperty); }
        public static void SetPediaLanguage(UIElement e, PediaLanguage lang)
            { e.SetValue(PediaLanguageProperty, lang); }

        private NeoLibrary lib = NeoReader.readNeoLibrary();
        public List<NeoNamekoListDisplay> NamekoList
        {
            get
            {
                List<NeoNamekoListDisplay> ret = new List<NeoNamekoListDisplay>(lib.nmklist.Count);
                foreach (NeoNameko nmk in lib.nmklist.Values)
                    ret.Add(new NeoNamekoListDisplay(nmk));
                return ret;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            this.InputBindings.Add(new KeyBinding(ValueUpDown.IncrementValueCommand
                , new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl+'='")));
            this.InputBindings.Add(new KeyBinding(ValueUpDown.DecrementValueCommand
                , new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl+'-'")));
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            if (nmklistview.SelectedItem != null)
            {
                backtbn.IsEnabled = true;
                NeoNamekoListDisplay nmk = (NeoNamekoListDisplay) nmklistview.SelectedItem;
                mainview.Content = new NamekoPage(lib.nmklist[nmk.Nameko.GlobalID]);
            }
        }
        private void OnBack(object sender, RoutedEventArgs e)
        {
            backtbn.IsEnabled = false;
            mainview.Content = nmklistview;
        }
    }

    public class CalculateColumnConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue)
                return 1;

            double width = (double)values[0];
            UIElementCollection items = (UIElementCollection)values[1];
            if (items.Count == 0) return 1;

            FrameworkElement e = (FrameworkElement)items[0];
            if (e.ActualWidth == 0) return 1;

            return (int)(width / e.ActualWidth);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace NamekoPedia.src
{
    /// <summary>
    /// Interaction logic for LanguagePanel.xaml
    /// </summary>
    [ContentProperty("ItemsSource")]
    public partial class LanguagePanel : ListBox
    {
        public new IEnumerable<PediaLanguage> ItemsSource
        {
            get { return (IEnumerable<PediaLanguage>)base.ItemsSource; }
            set
            {
                base.ItemsSource = value;
                base.SelectedIndex = 0;
            }
        }
        public new PediaLanguage SelectedItem
        {
            get { return (PediaLanguage)base.SelectedItem; }
            set
            {
                base.SelectedItem = value;
            }
        }
        static LanguagePanel()
        {
            ListBox.SelectedItemProperty.OverrideMetadata(typeof(LanguagePanel), new FrameworkPropertyMetadata(OnSelectedItemChanged));
        }
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LanguagePanel lp = (LanguagePanel)d;
            ListBox lb = (ListBox)d;
            if(lp.SelectedItem != (PediaLanguage)lb.SelectedItem)
                lp.SelectedItem = (PediaLanguage)lb.SelectedItem;
        }

        public Orientation Orientation { get; set; }

        public LanguagePanel()
        {
            InitializeComponent();
        }
    }
}

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

namespace NamekoPedia.src
{
    /// <summary>
    /// Interaction logic for NamekoPage.xaml
    /// </summary>
    public partial class NamekoPage : DockPanel
    {
        public NeoNameko Nameko { set { nmkview.Content = new NeoNamekoFullDisplay(value); } }
        public NamekoPage(NeoNameko nmk)
        {
            InitializeComponent();
            Nameko = nmk;
        }
    }
}

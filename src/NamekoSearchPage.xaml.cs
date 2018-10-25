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
    /// Interaction logic for NamekoSearchPage.xaml
    /// </summary>
    public partial class NamekoSearchPage : Page
    {
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
        public NamekoSearchPage()
        {
            InitializeComponent();
        }
    }
}

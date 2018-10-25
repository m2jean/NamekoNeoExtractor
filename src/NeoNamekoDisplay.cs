using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NamekoPedia.src
{
    public partial class NeoNamekoDisplayResources : ResourceDictionary
    {
        public NeoNamekoDisplayResources()
        {
            InitializeComponent();
        }

        private void OnClicked(Object o, RoutedEventArgs e)
        {
            DependencyObject d = (DependencyObject)o;
            do
            {
                d = VisualTreeHelper.GetParent(d);
            } while (d.GetType() != typeof(ListBox));
            ((ListBox)d).SelectedItem = ((Button)o).Tag;
        }
    }

    public class NeoNamekoFullDisplay
    {
        public static readonly List<MultilingualValue<ImageSource>> AttributeList =
            NeoReader.getAttributeIcons();

        public NeoNameko Nameko { get; }
        public LinkedList<MultilingualValue<ImageSource>> Attributes { get; }
            = new LinkedList<MultilingualValue<ImageSource>>();
        public List<NeoNamekoFormDisplay> Forms { get; }
        public NeoNamekoFullDisplay(NeoNameko nmk)
        {
            Nameko = nmk;

            for(int i = 0; i < AttributeList.Count; ++i)
            {
                if (Nameko.Attributes[i])
                    Attributes.AddLast(AttributeList[i]);
            }

            Forms = new List<NeoNamekoFormDisplay>();
            MultilingualValue<string> origin_theme = LangConfig.strings[StringID.NEO_NORMAL_FORM_NAME];
            MultilingualValue<string> origin_comment = LangConfig.strings[StringID.NEO_NORMAL_FORM_COMMENT];
            AddThemeFormToDisplay(0, origin_theme, origin_comment);
            foreach (var kvp in nmk.favortheme)
            {
                MultilingualValue<string> theme_name = new MultilingualValue<string>(
                    new Dictionary<PediaLanguage, string>()
                    {
                        { PediaLanguage.ENGLISH, kvp.Key.ToString() },
                        { PediaLanguage.JAPANESE, kvp.Key.ToString() }
                    }
                );
                AddThemeFormToDisplay(kvp.Key, theme_name, kvp.Value);
            }
        }

        private void AddThemeFormToDisplay(uint tid , MultilingualValue<string> name, MultilingualValue<string> comment)
        {
            string[] form_paths = PathConfig.getNeoNamekoWebpFiles(Nameko.NeoID, tid);
            if (form_paths.Length == 1)
            {
                Forms.Add(new NeoNamekoFormDisplay(tid, name, comment
                    , NeoReader.getWebpImage(form_paths[0]), 0));
            }
            else
            {
                for (uint i = 0, l = (uint)form_paths.Length; i < l; ++i)
                {
                    string[] numbered = name.Values.Select(nm => nm + (i + 1).ToString()).ToArray();
                    Forms.Add(new NeoNamekoFormDisplay(tid
                        , new MultilingualValue<string>(numbered), comment
                        , NeoReader.getWebpImage(form_paths[i]), i + 1));
                }
            }
        }
    }

    public class NeoNamekoFormDisplay
    {
        public uint ThemeID { get; }
        public MultilingualValue<string> ThemeName { get; }
        public MultilingualValue<string> ThemeComment { get; }
        public ImageSource ThemeForm { get; }
        public uint FormNo { get; }
        internal NeoNamekoFormDisplay(uint themeid, MultilingualValue<string> name,
            MultilingualValue<string> comment, ImageSource form, uint formi)
        {
            ThemeID = themeid;
            ThemeName = name;
            ThemeComment = comment;
            ThemeForm = form;
            FormNo = formi;
        }
    }

    public class NeoNamekoListDisplay
    {
        public NeoNameko Nameko { get; }
        public ImageSource Portrait { get; }
        public NeoNamekoListDisplay(NeoNameko nmk)
        {
            Nameko = nmk;
            Portrait = new CroppedBitmap(
                NeoReader.getWebpImage(PathConfig.getNeoNamekoWebpFiles(nmk.NeoID, 0)[0])
                , new System.Windows.Int32Rect(0, 0, 128, 128));
        }
    }
    public class NeoNamekoList : List<NeoNamekoListDisplay>
    {
        public List<NeoNameko> NamekoList
        {
            set
            {
                this.Capacity = value.Count;
                foreach(NeoNameko nmk in value)
                    this.Add(new NeoNamekoListDisplay(nmk));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using MultilingualString = System.Collections.Generic.Dictionary<NamekoPedia.src.PediaLanguage, string>;

namespace NamekoPedia.src
{
    class PathConfig
    {
        private PathConfig() { }
        public static string neo_rootdir { get; set; }
        public static string neo_lib { get; set; }
        public static string neo_nmkdir { get; set; }
        public static string neo_nmkattrs { get; set; }
        public static string neo_common_icons_en { get; set; }
        public static string neo_common_icons_jp { get; set; }
        static PathConfig()
        {
            neo_rootdir = "D:\\mushroomNEO.app\\";
            neo_lib = neo_rootdir + "en\\library\\library.bin";
            neo_nmkdir = neo_rootdir + "nameko\\";
            neo_nmkattrs = neo_rootdir + "varData\\nameko_attrs.bin";
            neo_common_icons_en = neo_rootdir + "en\\ui\\common.webp";
            neo_common_icons_jp = neo_rootdir + "jp\\ui\\common.webp";
        }
        public static string[] getNeoNamekoWebpFiles(uint neoid, uint theme)
        {
            string[] paths = System.IO.Directory.GetFiles(neo_nmkdir
                , "nmk_s" + neoid.ToString().PadLeft(3,'0') + "_" + theme.ToString().PadLeft(5,'0') + "*.webp");
            Array.Sort(paths);
            return paths;
        }
    }

    // the order of languages should be consistent to that in binary files
    public enum PediaLanguage { JAPANESE, ENGLISH };
    class LangConfig
    {
        public const int defaultfontsize = 16;

        public static PediaLanguage defaultlang = PediaLanguage.JAPANESE;
        public static readonly PediaLanguage[] LANGUAGE_LIST = (PediaLanguage[]) Enum.GetValues(typeof(PediaLanguage));
        
        public static readonly Dictionary<StringID, MultilingualValue<string>> strings =
            new Dictionary<StringID, MultilingualValue<string>>()
            {
                {
                    StringID.NEO_NORMAL_FORM_NAME,
                    new MultilingualValue<string>(
                        new Dictionary<PediaLanguage, string>()
                        {
                            { PediaLanguage.JAPANESE, "普段の姿" }, { PediaLanguage.ENGLISH, "Normal" }
                        }
                    )
                },
                {
                    StringID.NEO_NORMAL_FORM_COMMENT,
                    new MultilingualValue<string>(
                        new Dictionary<PediaLanguage, string>()
                        {
                            { PediaLanguage.JAPANESE,  "ふつうの姿です" }, {PediaLanguage.ENGLISH, "Original Form" }
                        }
                    )
                },
                {
                    StringID.NEO_NAMEKO_FULL_DISPLAY_VALUE,
                    new MultilingualValue<string>(
                        new Dictionary<PediaLanguage, string>()
                        {
                            { PediaLanguage.JAPANESE,  "換金額" }, {PediaLanguage.ENGLISH, "Value" }
                        }
                    )
                }
            };
    }
    public enum StringID
    {
        NEO_NORMAL_FORM_NAME,
        NEO_NORMAL_FORM_COMMENT,
        NEO_NAMEKO_FULL_DISPLAY_VALUE
    }
}

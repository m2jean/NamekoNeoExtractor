using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization;
using System.Diagnostics;

using MsgPack.Serialization;
using Imazen.WebP;

namespace NamekoPedia.src
{
    public class NeoReader
    {
        public static NeoLibrary readNeoLibrary()
        {
            MessagePackSerializer<RawNeoLibrary> szer = MessagePackSerializer.Get<RawNeoLibrary>();
            RawNeoLibrary lib = szer.Unpack(File.Open(PathConfig.neo_lib, FileMode.Open));
            //_libstat(lib);
            NeoLibrary ret = new NeoLibrary();
            ret.nmklist = new SortedDictionary<uint, NeoNameko>(
                lib.lib.ToDictionary<KeyValuePair<uint, RawNeoNameko>, uint, NeoNameko>
                    (kvp => kvp.Key, kvp => new NeoNameko(kvp.Value)));
            return ret;
        }
        private static void _libstat(RawNeoLibrary lib)
        {

            //int namel = 0, attribc = 0, descl = 0, remarkl = 0, thmdescr = 0, thmdescl = 0;
            foreach (RawNeoNameko nmk in lib.lib.Values)
            {
                //foreach (string s in nmk.name) if (s.Length > namel) namel = s.Length;

                //Debug.Assert(nmk.attrib.Count == 2);
                //int _attribc = 0;
                //for (uint num = nmk.attrib[0]; num != 0; num = num & (num - 1)) ++_attribc;
                //for (uint num = nmk.attrib[1]; num != 0; num = num & (num - 1)) ++_attribc;
                //if (_attribc > attribc) attribc = _attribc;

                //Debug.Assert(nmk.desc.Count == 2);
                //foreach (List<string> desc in nmk.desc)
                //{
                //    Debug.Assert(desc.Count <= 3);
                //    foreach (string s in desc) if (s.Length > descl) descl = s.Length;
                //}

                //Debug.Assert(nmk.remarks.Count == 2);
                //foreach (List<string> remark in nmk.remarks)
                //{
                //    Debug.Assert(remark.Count == 16);
                //    foreach (string s in remark) if (s.Length > remarkl) remarkl = s.Length;
                //}

                //foreach (RawNeoFavorTheme thm in nmk.favortheme)
                //{
                //    Debug.Assert(thm.desc.Count == 2);
                //    foreach (string s in thm.desc)
                //    {
                //        string[] ss = s.Split('\n');
                //        if (ss.Length > thmdescr) thmdescr = ss.Length;
                //        foreach (string sss in ss) if (sss.Length > thmdescl) thmdescl = sss.Length;
                //    }
                //}
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource getWebpImage(string path)
        {
            byte[] raw = File.ReadAllBytes(path);
            SimpleDecoder decoder = new SimpleDecoder();
            Bitmap bmp = decoder.DecodeFromBytes(raw, raw.LongLength);

            IntPtr hbmp = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                    (hbmp
                    ,IntPtr.Zero
                    ,new System.Windows.Int32Rect(0, 0, bmp.Width, bmp.Height)
                    ,BitmapSizeOptions.FromEmptyOptions()
                    );
            }
            finally
            {
                DeleteObject(hbmp);
            }
        }

        public static List<MultilingualValue<ImageSource>> getAttributeIcons()
        {
            MessagePackSerializer<List<Dictionary<uint, RawNeoNamekoAttribute>>> szer =
                MessagePackSerializer.Get<List<Dictionary<uint, RawNeoNamekoAttribute>>>();
            List<Dictionary<uint, RawNeoNamekoAttribute>> attrs =
                szer.Unpack(File.Open(PathConfig.neo_nmkattrs, FileMode.Open));
            
            // icon coorinations (1024,1024) (1792,1280) 16column 4row iconsize48x64
            // thumbnail coordinations (1792,1024) (2048,1280) 8column 8row thumbnailsize32x32
            const int iconsx = 1024, iconsy = 1024, iconwidth = 48, iconheight = 64, iconperrow=16;
            List<MultilingualValue<ImageSource>> ret = new List<MultilingualValue<ImageSource>>();
            BitmapSource common_en = getWebpImage(PathConfig.neo_common_icons_en);
            BitmapSource common_jp = getWebpImage(PathConfig.neo_common_icons_jp);
            for(int i =0; i < attrs[0].Count; ++i)
            {
                int x = iconsx + iconwidth * (i % iconperrow);
                int y = iconsy + iconheight * (i / iconperrow);
                ret.Add(
                    new MultilingualValue <ImageSource>
                    (
                        new List<ImageSource>()
                        {
                            new CroppedBitmap(common_jp, new Int32Rect(x, y, iconwidth, iconheight)),
                            new CroppedBitmap(common_en, new Int32Rect(x, y, iconwidth, iconheight))
                        }
                    )
                );
            }
            return ret;
        }
    }

    public class RawNeoLibrary
    {
        public SortedDictionary<uint, RawNeoNameko> lib;
    }

    [DataContract]
    public class RawNeoNameko
    {
        //9b array size 11
        [DataMember(Order = 0)] public uint globalid;
        [DataMember(Order = 1)] public uint neoid;
        [DataMember(Order = 2)] public List<string> name;//size2
        [DataMember(Order = 3)] public uint np;
        [DataMember(Order = 4)] public string _4;//empty
        [DataMember(Order = 5)] public List<uint> attrib;//size2
        [DataMember(Order = 6)] public List<List<string>> desc;//size2 of size3
        [DataMember(Order = 7)] public List<uint> _7;//size8
        [DataMember(Order = 8)] public List<List<string>> remarks;//size2 of size16
        [DataMember(Order = 9)] public List<uint> _9;//size3
        [DataMember(Order = 10)] public List<RawNeoFavorTheme> favortheme;
        //Length: name:13 attrib:8 desc:41 remark:20 thmdesc:21*3
    }
    [DataContract]
    public class RawNeoFavorTheme
    {
        [DataMember(Order = 0)] public uint themeid;
        [DataMember(Order = 1)] public List<string> desc;//size2
    }

    public class NeoLibrary { public SortedDictionary<uint, NeoNameko> nmklist; }
    public class NeoNameko {
        public uint GlobalID { get; }
        public uint NeoID { get; }
        public MultilingualValue<string> Name { get; }
        public uint NP { get; }
        public BitArray Attributes { get; }//size2
        public MultilingualValue<string> Description { get; }
        public List<Tuple<MultilingualValue<string>, MultilingualValue<string>>> Remarks { get; }//size8
        internal Dictionary<uint, MultilingualValue<string>> favortheme { get; }
        public NeoNameko(RawNeoNameko raw)
        {
            GlobalID = raw.globalid;
            NeoID = raw.neoid;
            Name = new MultilingualValue<string>(raw.name);
            NP = raw.np;
            Attributes = new BitArray(new int[] { (int)raw.attrib[0], (int)raw.attrib[1] });

            StringBuilder sb = new StringBuilder();
            List<string> descs = new List<String>();
            foreach(List<string> desc in raw.desc)
            {
                sb.Clear();
                foreach(string line in desc)
                {
                    sb.Append(line);
                    sb.Append('\n');
                }
                sb.Remove(sb.Length - 1, 1);
                descs.Add(sb.ToString());
            }
            Description = new MultilingualValue<string>(descs);

            Remarks = new List<Tuple<MultilingualValue<string>, MultilingualValue<string>>>(raw.remarks[0].Count/2);
            for (int j = 0; j < raw.remarks[0].Count; j+=2)
            {
                List<string> item = new List<string>();
                List<string> remark = new List<string>();
                for (int i = 0; i <raw.remarks.Count; ++i)
                {
                    item.Add(raw.remarks[i][j]);
                    remark.Add(raw.remarks[i][j+1]);
                }
                Remarks.Add(Tuple.Create(
                    new MultilingualValue<string>(item)
                    , new MultilingualValue<string>(remark)));
            }

            favortheme = raw.favortheme.ToDictionary
                    (fvrthm => fvrthm.themeid, fvrthm => new MultilingualValue<string>(fvrthm.desc));
        }
    }

    [DataContract]
    public class RawNeoNamekoAttribute
    {
        [DataMember(Order = 0)] public uint attri;
        [DataMember(Order = 1)] public uint attrid;
    }
}

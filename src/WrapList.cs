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
using System.Windows.Controls.Primitives;

namespace NamekoPedia.src
{
    /// <summary>
    /// </summary>
    /// can be used as ItemsPanel
    public class WrapList : StackPanel
    {
        public static readonly DependencyProperty FixedCellProperty =
            DependencyProperty.Register("FixedCell", typeof(uint), typeof(WrapList)
                , new FrameworkPropertyMetadata(0, OnFixedCellChanged));
        public int FixedCell
        {
            get { return (int) GetValue(FixedCellProperty); }
            set { SetValue(FixedCellProperty, value); }
        }
        private static void OnFixedCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapList wl = ((WrapList)d);
            wl.pnls = new List<VirtualizingStackPanel>(wl.FixedCell);
            for(int i = 0; i < wl.FixedCell; i++)
            {
                VirtualizingStackPanel lb = new VirtualizingStackPanel();
                wl.pnls.Add(lb);
            }
            wl.display();
        }

        static WrapList()
        {
        }

        private List<VirtualizingStackPanel> pnls;
        private void display()
        {
            List<List<Object>> groups = new List<List<Object>>(FixedCell);
            for(int i = 0; i < FixedCell; ++i)
            {
                groups.Add(new List<Object>());
            }
            UIElementCollection children = InternalChildren;
            for (int i = 0; i < children.Count; ++i)
            {
                pnls[i % FixedCell].Children.Add(children[i]);
            }
        }
    }

    public class WrapUniformGrid : UniformGrid
    {
        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapUniformGrid)
                , new FrameworkPropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation) this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size available)
        {
            if (Orientation == Orientation.Horizontal)
            {
                double maxchildwidth = 0;
                for (int i = 0, count = InternalChildren.Count; i < count; ++i) {
                    InternalChildren[i].Measure(available);
                    if (InternalChildren[i].DesiredSize.Width > maxchildwidth)
                        maxchildwidth = InternalChildren[i].DesiredSize.Width;
                }
                Rows = 0;
                Columns = maxchildwidth==0 || maxchildwidth>=available.Width
                    ? 1 : (int) (available.Width / maxchildwidth);
            }
            else
            {
                double maxchildheight = 0;
                for (int i = 0, count = InternalChildren.Count; i < count; ++i)
                {
                    InternalChildren[i].Measure(available);
                    if (InternalChildren[i].DesiredSize.Height > maxchildheight)
                        maxchildheight = InternalChildren[i].DesiredSize.Height;
                }
                Columns = 0;
                Rows = maxchildheight == 0 || maxchildheight >= available.Height
                    ? 1 : (int)(available.Height / maxchildheight);
            }
            return base.MeasureOverride(available);
        }
    }
}

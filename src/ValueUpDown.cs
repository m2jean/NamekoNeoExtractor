using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
namespace NamekoPedia.src
{
    /// <summary>
    /// </summary>
    //[ContentProperty("Value")]
    [TemplatePart(Name = "IncButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "DecButton", Type = typeof(RepeatButton))]
    public class ValueUpDown : Control
    {
        static ValueUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueUpDown), new FrameworkPropertyMetadata(typeof(ValueUpDown)));
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(ValueUpDown)
                , new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure, OnValueChanged, CoerceValue));
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static Object CoerceValue(DependencyObject d, Object value)
        {
            ValueUpDown vud = (ValueUpDown)d;
            int val = (int)value;
            if (val >= vud.MaxValue) return vud.MaxValue;
            if (val <= vud.MinValue) return vud.MinValue;
            return value;
        }
        public int Value
        {
            get { return (int) GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(ValueUpDown)
                , new FrameworkPropertyMetadata(10, OnMaxValueChanged));
        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValueUpDown vud = (ValueUpDown)d;
            if (vud.MinValue > vud.MaxValue) vud.MinValue = vud.MaxValue;
            if (vud.Value > vud.MaxValue) vud.Value = vud.MaxValue;
        }
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(ValueUpDown)
                , new FrameworkPropertyMetadata(0, OnMinValueChanged));
        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValueUpDown vud = (ValueUpDown)d;
            if (vud.MaxValue < vud.MinValue) vud.MaxValue = vud.MinValue;
            if (vud.Value < vud.MinValue) vud.Value = vud.MinValue;
        }
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        private RepeatButton incbtn,decbtn;
        public RepeatButton IncButton
        {
            get { return incbtn; }
            set
            {
                incbtn = value;
                if (value != null)
                {
                    incbtn.Command = IncrementValueCommand;
                    incbtn.CommandTarget = this;
                }
            }
        }
        public RepeatButton DecButton
        {
            get { return decbtn; }
            set
            {
                decbtn = value;
                if (value != null)
                {
                    decbtn.Command = DecrementValueCommand;
                    incbtn.CommandTarget = this;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            IncButton = GetTemplateChild("IncButton") as RepeatButton;
            DecButton = GetTemplateChild("DecButton") as RepeatButton;
        }

        public ValueUpDown()
        {
            this.CommandBindings.Add(new CommandBinding(IncrementValueCommand, (o, e) => ++Value));
            this.CommandBindings.Add(new CommandBinding(DecrementValueCommand, (o, e) => --Value));
        }

        public static RoutedUICommand IncrementValueCommand =
            new RoutedUICommand("Increment the value in the ValueUpDown","IncrementValueCommand",typeof(ValueUpDown));
        public static RoutedUICommand DecrementValueCommand =
            new RoutedUICommand("Decrement the value in the ValueUpDown", "DecrementValueCommand", typeof(ValueUpDown));
    }
}

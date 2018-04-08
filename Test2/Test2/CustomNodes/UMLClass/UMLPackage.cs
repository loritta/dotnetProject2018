using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Test2
{
    public class UMLPackage : TemplatedNode
    {
        private static string PackageNamePlaceholder = "<Package Name>";

        static UMLPackage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLPackage), new FrameworkPropertyMetadata(typeof(UMLPackage)));

        }
        public UMLPackage()
        {
            Init();
        }

        public UMLPackage(Diagram parent)
            : base(parent)
        {
            Init();
        }

        void Init()
        {
            PackageName = PackageNamePlaceholder;
            Stroke = Brushes.LightYellow;
            StrokeThickness = 3;

            HandlesStyle = HandlesStyle.HatchHandles3; // the way the node looks when selected
        }


        // properties
        public string PackageName
        {
            get { return (string)GetValue(PackageNameProperty); }
            set { SetValue(PackageNameProperty, value); }
        }

        public static readonly DependencyProperty PackageNameProperty = DependencyProperty.Register(
            "PackageName", typeof(string), typeof(UMLPackage), new PropertyMetadata(""));
    }
}

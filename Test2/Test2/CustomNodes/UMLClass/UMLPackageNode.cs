using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test2
{
    public class UMLPackageNode : TemplatedNode
    {
        private const string PackageNamePlaceholder = "<Package Name>";

        static UMLPackageNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLPackageNode), new FrameworkPropertyMetadata(typeof(UMLPackageNode)));
        }
        public UMLPackageNode()
        {
            Init();
        }

        public UMLPackageNode(Diagram parent)
            : base(parent)
        {
            Init();
        }

        void Init()
        {
            PackageName = PackageNamePlaceholder;
            HandlesStyle = HandlesStyle.HatchHandles3;
        }

        public string PackageName
        {
            get { return (string)GetValue(PackageNameProperty); }
            set { SetValue(PackageNameProperty, value); }
        }
        public static readonly DependencyProperty PackageNameProperty = DependencyProperty.Register(
            "PackageName", typeof(string), typeof(UMLInterfaceNode), new PropertyMetadata(""));

    }
}

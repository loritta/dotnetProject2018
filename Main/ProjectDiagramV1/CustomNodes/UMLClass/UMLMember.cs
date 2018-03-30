using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectDiagramV1
{
    public class UMLMember : TemplatedNode
    {
        private static string MemberNamePlaceholder = "<Member Name>";

        static UMLMember()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLMember), new FrameworkPropertyMetadata(typeof(UMLMember)));

        }
        public UMLMember()
        {
            Init();
        }

        public UMLMember(Diagram parent)
			: base(parent)
		{
            Init();
        }

        void Init()
        {
            MemberName = MemberNamePlaceholder;
            Stroke = Brushes.Gray;
            StrokeThickness = 5;

            HandlesStyle = HandlesStyle.HatchHandles3; // the way the node looks when selected
        }

        
        // properties
        public string MemberName
        {
            get { return (string)GetValue(MemberNameProperty); }
            set { SetValue(MemberNameProperty, value); }
        }

        public static readonly DependencyProperty MemberNameProperty = DependencyProperty.Register(
            "MemberName", typeof(string), typeof(UMLMember), new PropertyMetadata(""));
    }
}

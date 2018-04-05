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

        AnchorPattern squareAnchors = new AnchorPattern(new AnchorPoint[]
                {
                    // if you picture the node as a grid, top left is 0,0
                    // bottom right is 100,100, center is 50,50, etc...
                    // first value is X axis, 2nd is the Y axis
                    // yes i had to figure this out myself, no i dont know what true is for but its true
                    // what is documentation
                    new AnchorPoint(50, 0, true, true),
                    new AnchorPoint(100, 50, true, true),
                    new AnchorPoint(50, 100, true, true),
                    new AnchorPoint(0, 50, true, true)
                });

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
            this.AnchorPattern = squareAnchors;
            this.TextAlignment = TextAlignment.Center;
            this.TextVerticalAlignment = AlignmentY.Center;
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

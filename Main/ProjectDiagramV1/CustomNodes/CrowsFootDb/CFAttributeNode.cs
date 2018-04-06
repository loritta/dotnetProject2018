using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectDiagramV1
{
    class CFAttributeNode : TemplatedNode
    {
        private static string AttributeNamePlaceholder = "<Attribute Name>";

        /* Patterns might be added later
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
                */
        static CFAttributeNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CFAttributeNode), new FrameworkPropertyMetadata(typeof(CFAttributeNode)));

        }
        public CFAttributeNode()
        {
            Init();
        }

        public CFAttributeNode(Diagram parent)
			: base(parent)
		{
            Init();
        }

        void Init()
        {
            AttributeName = AttributeNamePlaceholder;

            /*
            Stroke = Brushes.Gray;
            StrokeThickness = 5;
            */
            HandlesStyle = HandlesStyle.HatchHandles3; // the way the node looks when selected

        }


        // properties
        public string AttributeName
        {
            get { return (string)GetValue(AttributeNameProperty); }
            set { SetValue(AttributeNameProperty, value); }
        }

        public static readonly DependencyProperty AttributeNameProperty = DependencyProperty.Register(
            "AttributeName", typeof(string), typeof(CFAttributeNode), new PropertyMetadata(""));
    }
}


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
    public class UMLInterfaceNode : TemplatedNode
    {

        private const string InterfaceNamePlaceholder = "<<Interface>>" + "\n" + "<interface name>";
        private const string MemberNamePlaceholder = "<enter member name>";
        private const string TextPlaceholder = "<enter description>";

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

        static UMLInterfaceNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLInterfaceNode), new FrameworkPropertyMetadata(typeof(UMLInterfaceNode)));
        }
        public UMLInterfaceNode()
        {
            Init();
        }

        public UMLInterfaceNode(Diagram parent)
            : base(parent)
        {
            Init();
        }

        void Init()
        {
            // event when you add a new member via button
            AddHandler(Button.ClickEvent, new RoutedEventHandler(OnClick));
            // event for when you "drag" a member node over a class node
            //AddHandler(Ruler., new DragEventHandler(OnDragOver));
            InterfaceName = InterfaceNamePlaceholder;
            Text = TextPlaceholder;
            Stroke = Brushes.Gray;
            StrokeThickness = 5;

            HandlesStyle = HandlesStyle.HatchHandles3;
            this.AnchorPattern = squareAnchors;
            this.TextAlignment = TextAlignment.Center;
            this.TextVerticalAlignment = AlignmentY.Center;

        }

        // "Add Member" clicked
        void OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            UMLInterfaceNode diag = sender as UMLInterfaceNode;

            Border buttonFirstParent = button.Parent as Border;
            Grid grid = buttonFirstParent.Parent as Grid;

            switch (button.Name)
            {
                case "BtAddMember":
                    if (Parent == null)
                    {
                        return;
                    }
                    else
                    {
                        // add a row to the node's grid, set focus on the textbox of the new row...

                        var rowDefinition = new RowDefinition();

                        rowDefinition.Height = GridLength.Auto;
                        grid.RowDefinitions.Add(rowDefinition);

                        var newTextbox = new TextBox();
                        grid.Children.Add(newTextbox);
                        newTextbox.Focus();

                        Grid.SetRow(newTextbox, grid.RowDefinitions.Count - 2);

                        Grid.SetRow(buttonFirstParent, grid.RowDefinitions.Count - 1);

                    }
                    break;
            }
        }
        // when a member is dropped over a class

        // properties

        public string MemberName
        {
            get { return (string)GetValue(MemberNameProperty); }
            set { SetValue(MemberNameProperty, value); }
        }

        public static readonly DependencyProperty MemberNameProperty = DependencyProperty.Register(
            "MemberName", typeof(string), typeof(UMLInterfaceNode), new PropertyMetadata(""));

        public string InterfaceName
        {
            get { return (string)GetValue(InterfaceNameProperty); }
            set { SetValue(InterfaceNameProperty, value); }
        }
        public static readonly DependencyProperty InterfaceNameProperty = DependencyProperty.Register(
            "InterfaceName", typeof(string), typeof(UMLInterfaceNode), new PropertyMetadata(""));
    }
}

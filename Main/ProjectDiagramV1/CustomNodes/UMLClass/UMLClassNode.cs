using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectDiagramV1
{
    public class UMLClassNode : TemplatedNode
    {
        private static List<Brush> Fills;

        private const string ClassNamePlaceholder = "<class name>";
        private const string MemberNamePlaceholder = "<enter member name>";
        private const string TextPlaceholder = "<enter description>";


        static UMLClassNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLClassNode), new FrameworkPropertyMetadata(typeof(UMLClassNode)));

            Fills = new List<Brush>(new Brush[]
            {
                new LinearGradientBrush(Colors.White, Colors.PaleGoldenrod, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.PaleGreen, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.PaleTurquoise, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.PaleVioletRed, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.PapayaWhip, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.PeachPuff, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.Peru, new Point(0, 0), new Point(0, 1)),
                new LinearGradientBrush(Colors.White, Colors.Pink, new Point(0, 0), new Point(0, 1)),
            });
        }
        public UMLClassNode()
        {
            Init();
        }

        public UMLClassNode(Diagram parent)
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
            ClassName = ClassNamePlaceholder;
            Text = TextPlaceholder;
            Stroke = Brushes.Gray;
            StrokeThickness = 5;

            HandlesStyle = HandlesStyle.HatchHandles3;
        }

        // "Add Member" clicked
        void OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            UMLClassNode diag = sender as UMLClassNode;

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
            "MemberName", typeof(string), typeof(UMLClassNode), new PropertyMetadata(""));

        public string ClassName
        {
            get { return (string)GetValue(ClassNameProperty); }
            set { SetValue(ClassNameProperty, value); }
        }
        public static readonly DependencyProperty ClassNameProperty = DependencyProperty.Register(
            "ClassName", typeof(string), typeof(UMLClassNode), new PropertyMetadata(""));

        void OnDragOver(object sender, DragEventArgs e)
        {
            
            MessageBox.Show(e.ToString());
        }
    }
}

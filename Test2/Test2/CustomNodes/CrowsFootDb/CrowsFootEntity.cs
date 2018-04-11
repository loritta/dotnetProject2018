using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Test2
{
    public class CrowsFootEntity : TemplatedNode
    {

        private const string EntityNamePlaceholder = "<entity name>";
        private const string AttributeNamePlaceholder = "<attribute name>";
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

        static CrowsFootEntity()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CrowsFootEntity), new FrameworkPropertyMetadata(typeof(CrowsFootEntity)));

        }
        public CrowsFootEntity()
        {
            Init();
        }

        public CrowsFootEntity(Diagram parent)
			: base(parent)
		{
            Init();
        }

        void Init()
        {
            AddHandler(Button.ClickEvent, new RoutedEventHandler(OnClick));

            EntityName = EntityNamePlaceholder;
            AttributeName = AttributeNamePlaceholder;
            Text = TextPlaceholder;
            Stroke = Brushes.Gray;
            StrokeThickness = 5;

            HandlesStyle = HandlesStyle.HatchHandles3;

            this.AnchorPattern = squareAnchors;
            this.TextAlignment = TextAlignment.Center;
            this.TextVerticalAlignment = AlignmentY.Center;
        }

        // "Add Attribute" clicked
        void OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            Border buttonFirstParent = button.Parent as Border;
            Grid grid = buttonFirstParent.Parent as Grid;

            switch (button.Name)
            {
                case "BtAddAttribute":
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

        // properties
        public string AttributeName
        {
            get { return (string)GetValue(AttributeNameProperty); }
            set { SetValue(AttributeNameProperty, value); }
        }

        public static readonly DependencyProperty AttributeNameProperty = DependencyProperty.Register(
            "AttributeName", typeof(string), typeof(CrowsFootEntity), new PropertyMetadata(""));

        public string EntityName
        {
            get { return (string)GetValue(EntityNameProperty); }
            set { SetValue(EntityNameProperty, value); }
        }
        public static readonly DependencyProperty EntityNameProperty = DependencyProperty.Register(
            "EntityName", typeof(string), typeof(CrowsFootEntity), new PropertyMetadata(""));

    }


}

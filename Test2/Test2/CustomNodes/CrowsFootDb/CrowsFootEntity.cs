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

            HandlesStyle = HandlesStyle.HatchHandles2;
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

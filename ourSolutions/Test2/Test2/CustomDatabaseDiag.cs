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
    class CustomDatabaseDiag: TemplatedNode
    {
        private static List<ImageSource> Images;
        private static List<Brush> Fills;
        private static List<string> Titles;

        private const string ClassNamePlaceholder = "<class name>";
        private const string MemberNamePlaceholder = "<enter member name>";
        private const string TextPlaceholder = "<enter description>";


        static CustomDatabaseDiag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CustomDatabaseDiag), new FrameworkPropertyMetadata(typeof(CustomDatabaseDiag)));

            // test
            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("Resources/CustomDiagram.xaml", UriKind.RelativeOrAbsolute);


            Images = new List<ImageSource>(new ImageSource[]
            {
                new BitmapImage(new Uri("Images/011.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/012.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/019.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/020.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/026.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/051.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/057.png", UriKind.Relative)),
                new BitmapImage(new Uri("Images/058.png", UriKind.Relative)),
            });
            Titles = new List<string>(new string[]
            {
                "Support",
                "Public Relations",
                "CEO",
                "Delivery",
                "Research",
                "Management",
                "Development",
                "Consulting",
            });
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
        public CustomDatabaseDiag()
        {
            Init();
        }

        public CustomDatabaseDiag(Diagram parent)
            : base(parent)
        {
            Init();
        }

        void Init()
        {
            AddHandler(Button.ClickEvent, new RoutedEventHandler(OnClick));

            ClassName = ClassNamePlaceholder;
            //MemberName = MemberNamePlaceholder;
            Text = TextPlaceholder;
            Stroke = Brushes.Gray;
            StrokeThickness = 5;

            Index = 0;

            HandlesStyle = HandlesStyle.HatchHandles3;
        }

        // "Add Member" clicked
        void OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            CustomDatabaseDiag diag = sender as CustomDatabaseDiag;

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
        // properties
        // not sure if this is needed
        public int Index
        {
            get { return Images.IndexOf(Image); }
            set
            {
                if (value != -1)
                {
                    Image = Images[value];
                    Brush = Fills[value];
                }
                else
                {
                    Image = null;
                }

                InvalidateVisual();
            }
        }

        public string MemberName
        {
            get { return (string)GetValue(MemberNameProperty); }
            set { SetValue(MemberNameProperty, value); }
        }

        public static readonly DependencyProperty MemberNameProperty = DependencyProperty.Register(
            "MemberName", typeof(string), typeof(CustomDatabaseDiag), new PropertyMetadata(""));

        public string ClassName
        {
            get { return (string)GetValue(ClassNameProperty); }
            set { SetValue(ClassNameProperty, value); }
        }
        public static readonly DependencyProperty ClassNameProperty = DependencyProperty.Register(
            "ClassName", typeof(string), typeof(CustomDatabaseDiag), new PropertyMetadata(""));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(CustomDatabaseDiag), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));


    }
}


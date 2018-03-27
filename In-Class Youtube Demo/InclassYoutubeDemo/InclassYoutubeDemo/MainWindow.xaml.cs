using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InclassYoutubeDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // add individual shape to list
            //shapeList.Items.Add(new ShapeNode { Shape = Shapes.RoundRect, Bounds = new Rect(0, 0, 40, 40) });

            shapeList.ItemsSource = MindFusion.Diagramming.Wpf.Shape.Shapes.Cast<MindFusion.Diagramming.Wpf.Shape>().Select(
                shape => new ShapeNode { Shape = shape, Bounds = new Rect(0, 0, 40, 40) });
        }
    }
}

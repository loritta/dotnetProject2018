using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Layout;
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

namespace Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public object LoadDiagramShapes { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            // load the built in nodes list
            //shapeList.ItemsSource = MindFusion.Diagramming.Wpf.Shape.Shapes.Cast<MindFusion.Diagramming.Wpf.Shape>().Select(
            //  shape => new ShapeNode { Shape = shape, Bounds = new Rect(0, 0, 40, 40) });


            // testing below
            diagram.LinkHeadShape = ArrowHeads.Triangle;
            diagram.DiagramLinkStyle = new System.Windows.Style();
            diagram.DiagramLinkStyle.Setters.Add(new Setter(DiagramLink.BrushProperty, Brushes.Black));
            diagram.LinkHeadShapeSize = 12;
            diagram.LinkShape = LinkShape.Cascading;
            diagram.RoundedLinks = true;
            diagram.RoundedLinksRadius = 10;
            diagram.LinkSegments = 3;
            diagram.Behavior = Behavior.Modify;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.Diagram = diagram;
            Commands.BindCommandsToDiagram(diagram);
        }
        private void diagram_Drop(object sender, DragEventArgs e)
        {
            ShapeNode node = diagram.Items[diagram.Items.Count - 1] as ShapeNode;
            node.Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 75, 75);

            diagram.Items.Remove(node); // dont add the default shape
            DiagramNodeCreated(node);   // this passes the default shape and adds the custom one instead
        }
        // this checks the dragged node type and creates the right custom node for it...
        private void DiagramNodeCreated(ShapeNode nodeShape)
        {
            if (nodeShape.Shape.Equals(Shapes.DividedProcess))
            {

                var node1 = new CustomDatabaseDiag
                {
                    Bounds = new Rect(nodeShape.Bounds.Left, nodeShape.Bounds.Top, 300, 160),
                    Index = 0 // might not need this
                };
                diagram.Nodes.Add(node1);
            }
            else
            {
                MessageBox.Show("Invalid Shape Selected");
            }

        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Add a specific node to the list           
            //shapeList.Items.Add(new ShapeNode { Shape = Shapes.DividedProcess, Bounds = new Rect(0, 0, 40, 40) });


            // using a method in LoadDiagramShapes to load shapes
            Test2.LoadDiagramShapes.LoadUmlNodes(shapeList);



            // Create the hierarchy
            //var node1 = new ShapeNode { Shape = Shapes.DividedProcess, Bounds = new Rect(0, 0, 40, 40) };
            //var node1 = new ContainerNode { Shape = SimpleShape.Rectangle, Bounds = new Rect(0, 0, 200, 200) };

            /* test stuff below
            var node1 = new CustomDatabaseDiag
            {
                Bounds = new Rect(0, 0, 300, 160),
                ClassName = "Mike Powell",
                MemberName = "Member 1",
                //emberName = "Member 1",
                Index = 0
            };         
            diagram.Nodes.Add(node1);

            var node2 = new CustomDatabaseDiag
            {
                Bounds = new Rect(0, 0, 300, 160),
                ClassName = "Emily Williams",
                MemberName = "Emily is the leader highest in the PR hierarchy.",
                Index = 1
            };
            diagram.Nodes.Add(node2);
            */
            TreeLayout layout = new TreeLayout();
            layout.Type = TreeLayoutType.Centered;
            layout.LinkStyle = TreeLayoutLinkType.Cascading3;
            layout.Direction = TreeLayoutDirections.TopToBottom;
            layout.KeepRootPosition = false;
            layout.LevelDistance = 40;
            layout.Arrange(diagram);

        }

        private void diagramView_Drop(object sender, DragEventArgs e)
        {

        }
        #region New Command
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
          
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }
        #endregion
    }
}

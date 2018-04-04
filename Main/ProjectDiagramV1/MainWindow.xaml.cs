using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Layout;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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

namespace ProjectDiagramV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShapeLibrary shapeLibrary = new ShapeLibrary();

        public MainWindow()
        {
            InitializeComponent();

            // load the built in nodes list
            //shapeList.ItemsSource = MindFusion.Diagramming.Wpf.Shape.Shapes.Cast<MindFusion.Diagramming.Wpf.Shape>().Select(
            //  shape => new ShapeNode { Shape = shape, Bounds = new Rect(0, 0, 40, 40) });

            // for some style effects
            diagram.NodeEffects.Add(new GlassEffect());
            diagram.NodeEffects.Add(new AeroEffect());


            // testing below
            /*
            diagram.LinkHeadShape = ArrowHeads.Triangle;
            diagram.DiagramLinkStyle = new System.Windows.Style();
            diagram.DiagramLinkStyle.Setters.Add(new Setter(DiagramLink.BrushProperty, Brushes.Black));
            diagram.LinkHeadShapeSize = 12;
            diagram.LinkShape = LinkShape.Cascading;
            diagram.RoundedLinks = true;
            diagram.RoundedLinksRadius = 10;
            diagram.LinkSegments = 3;
            diagram.Behavior = Behavior.Modify;
            */
        }
        private void diagram_Drop(object sender, DragEventArgs e)
        {
            ShapeNode node = diagram.Items[diagram.Items.Count - 1] as ShapeNode;
            node.Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 75, 75);

            //diagram.Items.Remove(node); // dont add the default shape
            //DiagramNodeCreated(node);   // this passes the default shape and adds the custom one instead
        }
        // this checks the dragged node type and creates the right custom node for it...
        private void DiagramNodeCreated(ShapeNode nodeShape)
        {
            /*
            if (nodeShape.Shape.Equals(Shapes.DividedProcess))
            {
                var node1 = new UMLClassNode
                {
                    Bounds = new Rect(nodeShape.Bounds.Left, nodeShape.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (nodeShape.Shape.Equals(Shapes.DividedEvent))
            {
                var node1 = new CrowsFootEntity
                {
                    Bounds = new Rect(nodeShape.Bounds.Left, nodeShape.Bounds.Top, 300, 160),
                };
                diagram.Nodes.Add(node1);
            }
            else if (nodeShape.Shape.Equals(Shapes.Rectangle))
            {
                var node1 = new UMLMember
                {
                    Bounds = new Rect(nodeShape.Bounds.Left, nodeShape.Bounds.Top, 300, 100),

                };
                diagram.Nodes.Add(node1);
            }
            else
            {
                MessageBox.Show("FML");
            }
            */
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            /* test stuff below
            var node1 = new UMLClassNode
            {
                Bounds = new Rect(0, 0, 300, 160),
                ClassName = "Mike Powell",
                MemberName = "Member 1",
                //emberName = "Member 1",
                ` = 0
            };         
            diagram.Nodes.Add(node1);

            var node2 = new UMLClassNode
            {
                Bounds = new Rect(0, 0, 300, 160),
                ClassName = "Emily Williams",
                MemberName = "Emily is the leader highest in the PR hierarchy.",
                Index = 1
            };
            diagram.Nodes.Add(node2);
           

            LoadCustomShapes();
            */
            TreeLayout layout = new TreeLayout();
            layout.Type = TreeLayoutType.Centered;
            layout.LinkStyle = TreeLayoutLinkType.Cascading3;
            layout.Direction = TreeLayoutDirections.TopToBottom;
            layout.KeepRootPosition = false;
            layout.LevelDistance = 40;
            layout.Arrange(diagram);

        }

        private void LoadBasicFlowchartNodes()
        {
            shapeLibrary.LoadFromXml(@"../../CustomNodes/CustomLibrary/BasicFlowchart.sl");
            NodesFromLib();
            shapeList.SelectedIndex = 0;

            // Process Node (square anchors)
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

            ShapeNode processNode = shapeList.Items.GetItemAt(0) as ShapeNode;
            processNode.AnchorPattern = squareAnchors;

            // Start/End Node (same square anchors)
            ShapeNode startEndNode = shapeList.Items.GetItemAt(1) as ShapeNode;
            startEndNode.AnchorPattern = squareAnchors;

            // Decision Node
            ShapeNode decisionNode = shapeList.Items.GetItemAt(2) as ShapeNode;
            decisionNode.AnchorPattern = squareAnchors;

            // Data Node
            ShapeNode dataNode = shapeList.Items.GetItemAt(3) as ShapeNode;
            dataNode.AnchorPattern = squareAnchors;


            diagram.LinkHeadShape = MindFusion.Diagramming.Wpf.ArrowHeads.Circle;
            diagram.LinkHeadShapeSize = 10; // dont ask me what 10 is refering to in this case.
        }
        private void LoadUmlNodes()
        {
            shapeList.Items.Clear();

            // Class Node
            shapeList.Items.Add(new ShapeNode { Shape = Shapes.DividedProcess, Bounds = new Rect(0, 0, 40, 40) });
            shapeList.Items.Add(new ShapeNode { Shape = Shapes.Rectangle, Bounds = new Rect(0, 0, 40, 10) });
        }

        private void LoadCrowsFootNodes()
        {
            shapeList.Items.Clear();

            // Entity Node
            shapeList.Items.Add(new ShapeNode { Shape = Shapes.DividedEvent, Bounds = new Rect(0, 0, 40, 40) });
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string);

            if (selectedItem == null)
            {
                return;
            }

            if (selectedItem.Equals("Crow's Foot Notation"))
            {
                LoadCrowsFootNodes();
            }
            else if (selectedItem.Equals("UML Class"))
            {
                LoadUmlNodes();
            }
            else if (selectedItem.Equals("Basic Flowchart"))
            {
                LoadBasicFlowchartNodes();
            }
            else
            {
                MessageBox.Show("Invalid ComboBox Selection, try again?");
            }
            
        }

        // ALL methods below are for testing custom shapes

        private void LoadCustomShapes()
        {
            /*
            shapeLibrary.LoadFromXml(@"../../CustomNodes/CustomLibrary/BasicFlowchart.sl");
            NodesFromLib();
            shapeList.SelectedIndex = 0;
            */
        }

        void NodesFromLib()
        {
            shapeList.Items.Clear();
            foreach (var shape in shapeLibrary.Shapes)
            {
                var node = new ShapeNode();
                node.Shape = shape;
                node.Text = node.Shape.Id;
                node.Resize(80, 40);
                shapeList.Items.Add(node);
            }
        }

    }


}

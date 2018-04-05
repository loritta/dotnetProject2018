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
            // get the "Name" value from the dragged node in the list
            ShapeNode draggedNode = shapeList.SelectedItem as ShapeNode;

            ShapeNode node = diagram.Items[diagram.Items.Count - 1] as ShapeNode;
            node.Name = draggedNode.Name;
            node.Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 75, 75);            

             
            DiagramNodeCreated(node);   // this passes the default shape and adds the custom one instead
        }

        // this checks the dragged node type and creates the right custom node for it...
        private void DiagramNodeCreated(ShapeNode node)
        {
            // no other actions needed if diagram type is basic flowchart
            if (node.Name.Equals("processNode") || node.Name.Equals("startEndNode") ||
                node.Name.Equals("decisionNode") || node.Name.Equals("dataNode") ||
                node.Name.Equals("subprocessNode") || node.Name.Equals("documentNode") )
            {
                return;
            }

            diagram.Items.Remove(node); // dont add the default shape
            // UML Nodes
            if (node.Name.Equals("classNode"))
            {
                var node1 = new UMLClassNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            if (node.Name.Equals("interfaceNode"))
            {
                var node1 = new UMLInterfaceNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }

            else if (node.Shape.Equals(Shapes.DividedEvent))
            {
                var node1 = new CrowsFootEntity
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),
                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Shape.Equals(Shapes.Rectangle))
            {
                var node1 = new UMLMember
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 100),

                };
                diagram.Nodes.Add(node1);
            }
            else
            {
                return;
            }
            
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {

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

            // probably should have a better way of doing this instead of hard coding
            // the getItemAt()

            ShapeNode processNode = shapeList.Items.GetItemAt(0) as ShapeNode;
            processNode.Name = "processNode";
            processNode.AnchorPattern = squareAnchors;
            processNode.TextAlignment = TextAlignment.Center;
            processNode.TextVerticalAlignment = AlignmentY.Center;
            processNode.ResizeToFitText(FitSize.KeepRatio);

            // Start/End Node (same square anchors)
            ShapeNode startEndNode = shapeList.Items.GetItemAt(4) as ShapeNode;
            startEndNode.Name = "startEndNode";
            startEndNode.AnchorPattern = squareAnchors;
            startEndNode.PolygonalTextLayout = true;
            startEndNode.TextAlignment = TextAlignment.Center;
            startEndNode.TextVerticalAlignment = AlignmentY.Center;
            startEndNode.ResizeToFitText(FitSize.KeepRatio);

            // Decision Node
            ShapeNode decisionNode = shapeList.Items.GetItemAt(5) as ShapeNode;
            decisionNode.Name = "decisionNode";
            decisionNode.AnchorPattern = squareAnchors;
            decisionNode.PolygonalTextLayout = true;
            decisionNode.TextAlignment = TextAlignment.Center;
            decisionNode.TextVerticalAlignment = AlignmentY.Center;
            decisionNode.ResizeToFitText(FitSize.KeepRatio);

            // Data Node
            ShapeNode dataNode = shapeList.Items.GetItemAt(1) as ShapeNode;
            dataNode.Name = "dataNode";
            dataNode.AnchorPattern = squareAnchors;
            dataNode.PolygonalTextLayout = true;
            dataNode.TextAlignment = TextAlignment.Center;
            dataNode.TextVerticalAlignment = AlignmentY.Center;
            dataNode.ResizeToFitText(FitSize.KeepRatio);

            // Subprocess Node
            ShapeNode subprocessNode = shapeList.Items.GetItemAt(2) as ShapeNode;
            subprocessNode.Name = "subprocessNode";
            subprocessNode.AnchorPattern = squareAnchors;
            subprocessNode.AnchorPattern = squareAnchors;
            subprocessNode.TextAlignment = TextAlignment.Center;
            subprocessNode.TextVerticalAlignment = AlignmentY.Center;
            subprocessNode.ResizeToFitText(FitSize.KeepRatio);

            // Document Node
            ShapeNode documentNode = shapeList.Items.GetItemAt(3) as ShapeNode;
            documentNode.Name = "documentNode";
            documentNode.AnchorPattern = squareAnchors;
            documentNode.AnchorPattern = squareAnchors;
            documentNode.TextAlignment = TextAlignment.Center;
            documentNode.TextVerticalAlignment = AlignmentY.Center;
            documentNode.ResizeToFitText(FitSize.KeepRatio);

            diagram.LinkHeadShape = MindFusion.Diagramming.Wpf.ArrowHeads.Circle;
            diagram.LinkHeadShapeSize = 10; // dont ask me what 10 is refering to in this case.
        }
        private void LoadUmlNodes()
        {
            shapeList.Items.Clear();

            shapeLibrary.LoadFromXml(@"../../CustomNodes/CustomLibrary/UMLClasses.sl");
            NodesFromLib();
            shapeList.SelectedIndex = 0;

            ShapeNode classNode = shapeList.Items.GetItemAt(0) as ShapeNode;
            classNode.Name = "classNode";

            ShapeNode interfaceNode = shapeList.Items.GetItemAt(1) as ShapeNode;
            interfaceNode.Name = "interfaceNode";

            ShapeNode packageNode = shapeList.Items.GetItemAt(2) as ShapeNode;
            packageNode.Name = "packageNode";

            ShapeNode separatorNode = shapeList.Items.GetItemAt(3) as ShapeNode;
            separatorNode.Name = "separatorNode";

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

        // proof of concept save/load xml, add a dialog
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            diagram.SaveToXml(@"../../SaveTest/diagram.xml");
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            diagram.LoadFromXml(@"../../SaveTest/diagram.xml");
        }
    }


}

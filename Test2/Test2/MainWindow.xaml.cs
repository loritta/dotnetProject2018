using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ShapeLibrary shapeLibrary = new ShapeLibrary();
        public object LoadDiagramShapes { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += new RoutedEventHandler(MainWindow_Loaded);


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
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.Diagram = diagram;
            Commands.BindCommandsToDiagram(diagram);
        }
        private void diagram_Drop(object sender, DragEventArgs e)
        {
            // get the "Name" value from the dragged node in the list
            ShapeNode draggedNode = shapeList.SelectedItem as ShapeNode;

            ShapeNode node = diagram.Items[diagram.Items.Count - 1] as ShapeNode;
            node.Name = draggedNode.Name;
            node.Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 75, 75);

            // for special cases where you would drop a member or separator node on top of a node
            if (node.Name.Equals("memberNode"))
            {
                DiagramNode dropTarget = diagram.GetNearestNode(e.GetPosition(this), 100, null);
                if (dropTarget != null)
                {
                    if (dropTarget.Name.Equals("classNode"))
                    {
                        MessageBox.Show("dropped member node on class node...");
                    }
                    else
                    {
                        MessageBox.Show("Now I'm Lost");
                    }
                    

                }
            }
            

            else
            {
                DiagramNodeCreated(node);   // this passes the default shape and adds the custom one instead

            }



        }


        // this checks the dragged node type and creates the right custom node for it...
        // this checks the dragged node type and creates the right custom node for it...
        #region Node created
        private void DiagramNodeCreated(ShapeNode node)
        {
            // check if the node is a connector
            if (node.Shape.Id == "InheritanceLink")
            {
                // replace the dummy connector node with a DiagramLink
                var bounds = node.Bounds;
                diagram.Items.Remove(node);

                var link = diagram.Factory.CreateDiagramLink(
                    bounds.TopLeft, bounds.BottomRight);
                link.SegmentCount = 2;
                link.Shape = LinkShape.Cascading;
                link.HeadShape = ArrowHeads.Triangle;
                ConnectToNearbyNode(link);
            }
            else if (node.Shape.Id == "InterfaceLink")
            {
                var bounds = node.Bounds;
                diagram.Items.Remove(node);

                var link = diagram.Factory.CreateDiagramLink(
                    bounds.TopLeft, bounds.BottomRight);
                link.SegmentCount = 1;
                link.Shape = LinkShape.Cascading;

                System.Windows.Media.DashStyle dash = new System.Windows.Media.DashStyle();
                dash.Dashes = new DoubleCollection(new double[] { 1, 7 });

                Pen pen = new Pen(Brushes.Black, 1);
                pen.DashStyle = dash;

                link.Pen = pen;

                link.HeadShape = ArrowHeads.Triangle;
                ConnectToNearbyNode(link);
            }
            if (node.Shape.Id == "RelationshipLink")
            {
                // replace the dummy connector node with a DiagramLink
                var bounds = node.Bounds;
                diagram.Items.Remove(node);

                var link = diagram.Factory.CreateDiagramLink(
                    bounds.TopLeft, bounds.BottomRight);
                link.SegmentCount = 2;
                link.Shape = LinkShape.Cascading;
                link.HeadShape = ArrowHeads.DefaultFlow; // might need a custom arrowhead here, supposed to be a straight line with 2
                link.BaseShape = ArrowHeads.RevWithLine; // vertical || across (see visio)
                ConnectToNearbyNode(link);
            }
            else
            {
                node.AnchorPattern = AnchorPattern.Decision2In2Out;
                ConnectToNearbyLink(node);
            }

            // no other actions needed if diagram type is basic flowchart
            if (node.Name.Equals("processNode") || node.Name.Equals("startEndNode") ||
                node.Name.Equals("decisionNode") || node.Name.Equals("dataNode") ||
                node.Name.Equals("subprocessNode") || node.Name.Equals("documentNode"))
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
            else if (node.Name.Equals("interfaceNode"))
            {
                var node1 = new UMLInterfaceNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("memberNode"))
            {
                var node1 = new UMLMember
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("separatorNode"))
            {
                var node1 = new UMLSeparatorNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 5),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("packageNode"))
            {
                var node1 = new UMLPackageNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            // 
            // Crow's foot nodes
            else if (node.Name.Equals("entityNode"))
            {
                var node1 = new CrowsFootEntity
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("primaryKeyNode"))
            {
                var node1 = new CFPrimaryKeyNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("attributeNode"))
            {
                var node1 = new CFAttributeNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 160),

                };
                diagram.Nodes.Add(node1);
            }
            else if (node.Name.Equals("primaryKeySeparatorNode"))
            {
                var node1 = new CFPrimaryKeySeparatorNode
                {
                    Bounds = new Rect(node.Bounds.Left, node.Bounds.Top, 300, 5),

                };
                diagram.Nodes.Add(node1);
            }
            else
            {
                return;
            }

        }
        #endregion

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // still not sure if this is needed
            TreeLayout layout = new TreeLayout();
            layout.Type = TreeLayoutType.Centered;
            layout.LinkStyle = TreeLayoutLinkType.Cascading3;
            layout.Direction = TreeLayoutDirections.TopToBottom;
            layout.KeepRootPosition = false;
            layout.LevelDistance = 40;
            layout.Arrange(diagram);

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
            else if (selectedItem.Equals("UML Class Diagram"))
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

        #region Load Basic Flowchart
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
        #endregion
        #region Load UML
        private void LoadUmlNodes()
        {
            diagram.AutoSnapLinks = true;
            diagram.AllowUnanchoredLinks = false;
            diagram.Behavior = Behavior.PanAndModify; // user can select diagrams and pan the screen
            diagram.RouteLinks = true;                  // links will be automatically routed properly
            shapeList.Items.Clear();        

            shapeLibrary.LoadFromXml(@"../../CustomNodes/CustomLibrary/UMLClasses.sl");
            NodesFromLib();
            shapeList.SelectedIndex = 0;

            ShapeNode classNode = shapeList.Items.GetItemAt(0) as ShapeNode;
            classNode.Name = "classNode";
            classNode.Text = "";

            ShapeNode interfaceNode = shapeList.Items.GetItemAt(1) as ShapeNode;
            interfaceNode.Name = "interfaceNode";
            interfaceNode.Text = "";

            ShapeNode packageNode = shapeList.Items.GetItemAt(2) as ShapeNode;
            packageNode.Name = "packageNode";
            packageNode.Text = "";

            ShapeNode separatorNode = shapeList.Items.GetItemAt(3) as ShapeNode;
            separatorNode.Name = "separatorNode";
            separatorNode.Text = "";

            ShapeNode memberNode = shapeList.Items.GetItemAt(4) as ShapeNode;
            memberNode.Name = "memberNode";
            memberNode.Text = "";

            // remove the text from the nodes, add a label instead (text shows on right)
            NodeListView.SetLabel(classNode, "Class");
            NodeListView.SetLabel(interfaceNode, "Interface");
            NodeListView.SetLabel(packageNode, "Package");
            NodeListView.SetLabel(separatorNode, "Separator");
            NodeListView.SetLabel(memberNode, "Member");
            // testing custom connectors
            // inheritance connector
            var inheritanceShape = new MindFusion.Diagramming.Wpf.Shape(
                null, // no borders
                new[] // decorations
	            {
                    new LineTemplate(10, 10, 10, 50),
                    new LineTemplate(10, 50, 90, 50),
                    new LineTemplate(90, 50, 90, 90)
                },
                null,
                FillRule.Nonzero, "InheritanceLink");

            var inheritanceListNode = new ShapeNode { Shape = inheritanceShape };
            NodeListView.SetLabel(inheritanceListNode, "Inheritance");
            shapeList.Items.Add(inheritanceListNode);

            // Interface realization
            var interfaceShape = new MindFusion.Diagramming.Wpf.Shape(
                null, // no borders
                new[] // decorations
	            {
                    new LineTemplate(10, 10, 10, 50, Color.FromRgb(0,0,0), DashStyles.Dot, 1),
                    new LineTemplate(10, 50, 90, 50, Color.FromRgb(0,0,0), DashStyles.Dot, 1),
                    new LineTemplate(90, 50, 90, 90, Color.FromRgb(0,0,0), DashStyles.Dot, 1)
                },
                null,
                FillRule.Nonzero, "InterfaceLink");

            var interfaceListNode = new ShapeNode { Shape = interfaceShape };
            NodeListView.SetLabel(interfaceListNode, "Interface Realization");
            shapeList.Items.Add(interfaceListNode);
        }
        #endregion
        #region Load Crow's Foot
        private void LoadCrowsFootNodes()
        {
            shapeList.Items.Clear();

            shapeLibrary.LoadFromXml(@"../../CustomNodes/CustomLibrary/CrowsFootDb.sl");
            NodesFromLib();
            shapeList.SelectedIndex = 0;

            ShapeNode entityNode = shapeList.Items.GetItemAt(0) as ShapeNode;
            entityNode.Name = "entityNode";
            entityNode.Text = "";

            ShapeNode primaryKeyNode = shapeList.Items.GetItemAt(1) as ShapeNode;
            primaryKeyNode.Name = "primaryKeyNode";
            primaryKeyNode.Text = "";

            ShapeNode attributeNode = shapeList.Items.GetItemAt(2) as ShapeNode;
            attributeNode.Name = "attributeNode";
            attributeNode.Text = "";

            /*
            ShapeNode pkSeparatorNode = shapeList.Items.GetItemAt(3) as ShapeNode;
            pkSeparatorNode.Name = "primaryKeySeparatorNode";
            attributeNode.Text = "";
            */

            // Primary key separator
            var primaryKeySeparatorShape = new MindFusion.Diagramming.Wpf.Shape(
                null, // no borders
                new[] // decorations
	            {
                    new LineTemplate(10, 50, 90, 50, Color.FromRgb(0,0,0), DashStyles.Dot, 1),
                },
                null,
                FillRule.Nonzero, "primaryKeySeparatorNode");

            var pkSeparatorListNode = new ShapeNode { Shape = primaryKeySeparatorShape };
            shapeList.Items.Add(pkSeparatorListNode);
            pkSeparatorListNode.Name = "primaryKeySeparatorNode";

            // remove the text from the nodes, add a label instead (text shows on right)
            NodeListView.SetLabel(entityNode, "Entity");
            NodeListView.SetLabel(primaryKeyNode, "Primary Key");
            NodeListView.SetLabel(pkSeparatorListNode, "Primary Key Separator");
            NodeListView.SetLabel(attributeNode, "Attribute");

            // Relationship connector
            var relationshipShape = new MindFusion.Diagramming.Wpf.Shape(
                null, // no borders
                new[] // decorations
	            {
                    new LineTemplate(10, 10, 10, 50),
                    new LineTemplate(10, 50, 90, 50),
                    //new LineTemplate(90, 50, 90, 90)
                },
                null,
                FillRule.Nonzero, "RelationshipLink");

            var relationshipListNode = new ShapeNode { Shape = relationshipShape };
            NodeListView.SetLabel(relationshipListNode, "Relationship");
            shapeList.Items.Add(relationshipListNode);

        }
        #endregion

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
        private void ConnectToNearbyNode(DiagramLink link)
        {
            // connect to a nearby origin node
            var origin = diagram.Nodes.OrderBy(n =>
                MindFusion.Utilities.Distance(n.GetCenter(), link.StartPoint)).FirstOrDefault();

            if (origin != null)
            {
                var distance = MindFusion.Utilities.Distance(origin.GetCenter(), link.StartPoint);
                if (distance < 120)
                {
                    link.Origin = origin;
                    link.Route();
                }
            }

            // connect to a nearby destination node
            var destination = diagram.Nodes.Where(n => n != origin).OrderBy(n =>
                MindFusion.Utilities.Distance(n.GetCenter(), link.EndPoint)).FirstOrDefault();

            if (destination != null)
            {
                var distance = MindFusion.Utilities.Distance(destination.GetCenter(), link.EndPoint);
                if (distance < 120)
                {
                    link.Destination = destination;
                    link.Route();
                }
            }
        }
        private void ConnectToNearbyLink(DiagramNode node)
        {
            var outLink = diagram.Links.Where(l => l.Origin is DummyNode).OrderBy(l =>
                MindFusion.Utilities.Distance(node.GetCenter(), l.StartPoint)).FirstOrDefault();

            if (outLink != null)
            {
                var distance = MindFusion.Utilities.Distance(node.GetCenter(), outLink.StartPoint);
                if (distance < 90)
                {
                    outLink.Origin = node;
                    outLink.Route();
                    return;
                }
            }

            var inLink = diagram.Links.Where(l => l.Destination is DummyNode).OrderBy(l =>
                MindFusion.Utilities.Distance(node.GetCenter(), l.EndPoint)).FirstOrDefault();

            if (inLink != null)
            {
                var distance = MindFusion.Utilities.Distance(node.GetCenter(), inLink.EndPoint);
                if (distance < 90)
                {
                    inLink.Destination = node;
                    inLink.Route();
                    return;
                }
            }
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

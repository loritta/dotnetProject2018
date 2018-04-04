//
// Copyright (c) 2016, MindFusion LLC - Bulgaria.
//

using System.Windows;
using System.Windows.Media;


namespace MindFusion.Diagramming.Wpf.Samples.CS.Anchors
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			diagram.DefaultShape = Shapes.Rectangle;
			diagram.Bounds = new Rect(0, 0, 3840, 3840);

			radioButton1.IsChecked = true;

			diagram.NodeEffects.Add(new GlassEffect());
			diagram.NodeEffects.Add(new AeroEffect());

			Style shapeNodeStyle = new Style();
			shapeNodeStyle.Setters.Add(new Setter(ShapeNode.BrushProperty,
				new LinearGradientBrush(Colors.LightGray, Colors.DarkTurquoise, 80)));
			shapeNodeStyle.Setters.Add(new Setter(ShapeNode.FontSizeProperty, 10.75));
			diagram.ShapeNodeStyle = shapeNodeStyle;

			ShapeNode pb1, pb2, pb3, pb4, decb1, decb2;

			apat1 = new AnchorPattern(new AnchorPoint[]
				{
					new AnchorPoint(50, 0, true, true),
					new AnchorPoint(100, 50, true, true),
					new AnchorPoint(50, 100, true, true),
					new AnchorPoint(0, 50, true, true)
				});

			apat2 = new AnchorPattern(new AnchorPoint[]
				{
					new AnchorPoint(10, 0, true, false, MarkStyle.Circle, Brushes.RoyalBlue),
					new AnchorPoint(50, 0, true, false, MarkStyle.Circle, Brushes.Blue),
					new AnchorPoint(90, 0, true, false, MarkStyle.Circle, Brushes.Firebrick),
					new AnchorPoint(10, 100, false, true, MarkStyle.Rectangle),
					new AnchorPoint(50, 100, false, true, MarkStyle.Rectangle),
					new AnchorPoint(90, 100, false, true, MarkStyle.Rectangle),
					new AnchorPoint(0, 50, true, true, MarkStyle.Custom)
				});

			pb1 = new ShapeNode(diagram);
			pb1.Bounds = new Rect(38.4, 26.88, 96, 69.12);
			pb1.Shape = Shapes.Ellipse;
			pb1.Text = "Start";
			pb1.AnchorPattern = apat1;
			diagram.Nodes.Add(pb1);

			pb2 = new ShapeNode(diagram);
			pb2.Bounds = new Rect(76.8, 288, 96, 69.12);
			pb2.Text = "node 1";
			pb2.AnchorPattern = apat2;
			diagram.Nodes.Add(pb2);

			pb3 = new ShapeNode(diagram);
			pb3.Bounds = new Rect(268.8, 268.8, 96, 69.12);
			pb3.Text = "node 2";
			pb3.AnchorPattern = apat2;
			diagram.Nodes.Add(pb3);

			pb4 = new ShapeNode(diagram);
			pb4.Bounds = new Rect(307.2, 384, 96, 69.12);
			pb4.Shape = Shapes.Ellipse;
			pb4.Text = "End";
			pb4.AnchorPattern = apat1;
			diagram.Nodes.Add(pb4);

			decb1 = new ShapeNode(diagram);
			decb1.Bounds = new Rect(76.8, 134.4, 115.2, 76.8);
			decb1.Shape = Shapes.Decision;
			decb1.Text = "check 1";
			decb1.AnchorPattern = AnchorPattern.Decision1In3Out;
			diagram.Nodes.Add(decb1);

			decb2 = new ShapeNode(diagram);
			decb2.Bounds = new Rect(268.8, 115.2, 115.2, 76.8);
			decb2.Shape = Shapes.Decision;
			decb2.Text = "check 2";
			decb2.AnchorPattern = AnchorPattern.Decision2In2Out;
			diagram.Nodes.Add(decb2);

			DiagramLink link = new DiagramLink(diagram, decb1, decb2);
			diagram.Links.Add(link);

			var router = diagram.LinkRouter as QuickRouter;
			if (router != null)
				router.UBendMaxLen = 10;
			diagram.RoutingOptions.TriggerRerouting |= RerouteLinks.WhileCreating;
			diagram.RoutingOptions.Anchoring = Anchoring.Keep;
		}

		private void radioButton1_Checked(object sender, RoutedEventArgs e)
		{
			diagram.DefaultShape = Shapes.Rectangle;
		}

		private void radioButton2_Checked(object sender, RoutedEventArgs e)
		{
			diagram.DefaultShape = Shapes.Decision;
		}

		private void diagram_NodeCreated(object sender, NodeEventArgs e)
		{
			ShapeNode node = e.Node as ShapeNode;
			if (node == null)
				return;

			if (node.Shape == Shapes.Decision)
				node.AnchorPattern = AnchorPattern.Decision1In3Out;
			else
				node.AnchorPattern = apat2;
		}

		private void OnLoadClick(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
			if (openDlg.ShowDialog() == true)
				diagram.LoadFromXml(openDlg.FileName);
		}

		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();
			if (saveDlg.ShowDialog() == true)
				diagram.SaveToXml(saveDlg.FileName);
		}


		private AnchorPattern apat1;
		private AnchorPattern apat2;
	}
}
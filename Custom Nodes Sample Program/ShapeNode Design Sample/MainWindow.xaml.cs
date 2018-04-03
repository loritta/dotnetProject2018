using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using MindFusion.Diagramming.Wpf;
using MindFusion.Licensing;


namespace ShapeDesign
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			NewShapeLib();
			nodeList.SelectionChanged += OnNodeListSelectionChanged;

			designer.ShowGrid = true;
			designer.GridSize = 10;
			designer.AlignToGrid = true;
		}

		// store custom shapes here; the library can be serialized to a file
		// using the SaveToXml method and deserialized using LoadFromXml
		ShapeLibrary shapeLibrary = new ShapeLibrary();

		void OnNodeListSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (e.RemovedItems.Count > 0)
				UpdateNode((ShapeNode)e.RemovedItems[0]);

			// show newly selected shape in designer
			if (e.AddedItems.Count > 0)
			{
				var node = (ShapeNode)e.AddedItems[0];
				designer.ImportShape(node.Shape);
			}
		}

		private void OnShapeChanged(object sender, EventArgs e)
		{
			// the shape definition has changed; udpate diagram node to show it
			UpdateNode((ShapeNode)nodeList.SelectedItem);
		}

		void UpdateNode(ShapeNode node)
		{
			if (node == null)
				return;

			var shape = designer.ExportShape(node.Shape.Id);

			// update sample node in diagram
			if (diagram.Nodes.Count == 0)
				diagram.Factory.CreateShapeNode(30, 30, 100, 100, shape);
			else
				((ShapeNode)diagram.Nodes[0]).Shape = shape;

			// update shape in palette
			node.Shape = shape;
		}

		private void OnFileNew(object sender, RoutedEventArgs e)
		{
			NewShapeLib();
		}

		private void OnFileSave(object sender, RoutedEventArgs e)
		{
			// Configure save file dialog box
			var dlg = new SaveFileDialog
			{
				FileName = "shapes",
				DefaultExt = ".sl",
				Filter = "Shape Library Files (.sl)|*.sl"
			};

			// Show save file dialog box
			var result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result == true)
			{
				LibFromNodes();
				shapeLibrary.SaveToXml(dlg.FileName);
			}
		}

		private void OnFileOpen(object sender, RoutedEventArgs e)
		{
			// Configure open file dialog box
			var dlg = new OpenFileDialog
			{
				FileName = "shapes",
				DefaultExt = ".sl",
				Filter = "Shape Library Files (.sl)|*.sl"
			};

			// Show open file dialog box
			var result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true)
			{
				shapeLibrary.LoadFromXml(dlg.FileName);
				NodesFromLib();
				nodeList.SelectedIndex = 0;
			}
		}

		private void OnShapesNew(object sender, RoutedEventArgs e)
		{
			var dialog = new ShapeIdDialog();
			dialog.Owner = this;
			dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			if (dialog.ShowDialog() == true)
			{
				LibFromNodes();
				var list = new List<Shape>(shapeLibrary.Shapes);
				var shape = NewShape(dialog.ShapeId);
				list.Add(shape);
				shapeLibrary.Shapes = list.ToArray();
				NodesFromLib();
				foreach (var item in nodeList.Items)
				{
					var node = (ShapeNode)item;
					if (node.Shape == shape)
						nodeList.SelectedItem = item;
				}
			}
		}

		private void OnShapesRemove(object sender, RoutedEventArgs e)
		{
			if (nodeList.SelectedItem != null)
			{
				nodeList.Items.Remove(nodeList.SelectedItem);
				LibFromNodes();
			}
		}

		void NodesFromLib()
		{
			nodeList.Items.Clear();
			foreach (var shape in shapeLibrary.Shapes)
			{
				var node = new ShapeNode();
				node.Shape = shape;
				node.Text = node.Shape.Id;
				node.Resize(80, 40);
				nodeList.Items.Add(node);
			}
		}

		void NewShapeLib()
		{
			shapeLibrary.Shapes = new[] { NewShape("MyShape") };
			NodesFromLib();
			nodeList.SelectedIndex = 0;
		}

		Shape NewShape(string id)
		{
			var tempShape = new Shape(
				Shapes.Rectangle.Outline, FillRule.Nonzero, id);
			    designer.ImportShape(tempShape);
			    return tempShape;
		}

		void LibFromNodes()
		{
			var list = new List<Shape>();
			foreach (ShapeNode node in nodeList.Items)
			{
				list.Add(node.Shape);
			}
			shapeLibrary.Shapes = list.ToArray();
		}
	}
}
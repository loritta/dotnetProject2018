using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShapeDesign
{
	/// <summary>
	/// Interaction logic for ShapeIdDialog.xaml
	/// </summary>
	public partial class ShapeIdDialog : Window
	{
		public ShapeIdDialog()
		{
			InitializeComponent();
		}

		public string ShapeId
		{
			get { return ShapeIdBox.Text; }
			set { ShapeIdBox.Text = value; }
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}

using MindFusion.Diagramming.Wpf;
using System.Windows;
namespace Test2
{

    class LoadDiagramShapes
    {
        public static void LoadUmlNodes(NodeListView shapeList)
        {
            // Add a specific node to the list           
            shapeList.Items.Add(new ShapeNode { Shape = Shapes.DividedProcess, Bounds = new Rect(0, 0, 40, 40) });
        }
    }

}

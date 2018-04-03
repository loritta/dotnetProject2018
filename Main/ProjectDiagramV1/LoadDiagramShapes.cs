using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectDiagramV1
{
    class LoadDiagramShapes
    {
        public static void LoadUmlNodes(NodeListView shapeList)
        {
            // Add a specific node to the list           
            shapeList.Items.Add(new ShapeNode { Shape = Shapes.DividedProcess, Bounds = new Rect(0, 0, 40, 40) });
        }

        public static void LoadCrowDatabaseNodes(NodeListView shapeList)
        {
            // Add a specific node to the list           
            //
        }

        public static void Test(NodeListView shapeList)
        {
            // Add a specific node to the list           
            
        }
    }
}

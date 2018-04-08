using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindFusion.Diagramming.Wpf;

namespace Test2
{
    class Globals
    {
        private static Diagram diagram;

        public static Diagram Diagram { get => diagram; set => diagram = value; }
    }
}
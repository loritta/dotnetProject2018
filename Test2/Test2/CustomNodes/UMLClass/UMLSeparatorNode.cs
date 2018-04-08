using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test2
{
    public class UMLSeparatorNode : TemplatedNode
    {
        static UMLSeparatorNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLSeparatorNode), new FrameworkPropertyMetadata(typeof(UMLSeparatorNode)));

        }
        public UMLSeparatorNode()
        {
            Init();
        }

        public UMLSeparatorNode(Diagram parent)
			: base(parent)
		{
            Init();
        }

        void Init()
        {
        }
    }
}

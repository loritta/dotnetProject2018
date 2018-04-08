using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test2
{
    class CFPrimaryKeySeparatorNode : TemplatedNode
    {
        static CFPrimaryKeySeparatorNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CFPrimaryKeySeparatorNode), new FrameworkPropertyMetadata(typeof(CFPrimaryKeySeparatorNode)));

        }
        public CFPrimaryKeySeparatorNode()
        {
            Init();
        }

        public CFPrimaryKeySeparatorNode(Diagram parent)
			: base(parent)
		{
            Init();
        }

        void Init()
        {
        }
    }
}

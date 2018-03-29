using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDiagramV1
{
    // Common interface for items that can be selected
    // on the DesignerCanvas; used by DesignerItem and Connection
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}

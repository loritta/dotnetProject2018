using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using MindFusion.Diagramming.Wpf;

namespace Test2
{
    public class Commands
    {

        private static Diagram diagram = Globals.diagram;
        private static RoutedUICommand _new;
        private static RoutedUICommand _group;
        private static RoutedUICommand _ungroup;
        private static RoutedUICommand _bringForward;
        private static RoutedUICommand _bringToFront;
        private static RoutedUICommand _sendBackward;
        private static RoutedUICommand _sendToBack;
        private static RoutedUICommand _alignTop;
        private static RoutedUICommand _alignVerticalCenters;
        private static RoutedUICommand _alignBottom;
        private static RoutedUICommand _alignLeft;
        private static RoutedUICommand _alignHorizontalCenters;
        private static RoutedUICommand _alignRight;
        private static RoutedUICommand _distributeHorizontal;
        private static RoutedUICommand _distributeVertical;
        private static RoutedUICommand _selectAll;

        static Commands()
        {
            _new = new RoutedUICommand("New page", "New_Executed", typeof(Commands));
            _group = new RoutedUICommand("Group elements", "Group", typeof(Commands));
            _ungroup = new RoutedUICommand("Ungroup elements", "Ungoup", typeof(Commands));
            _bringForward = new RoutedUICommand("Bring Forward elements", "BringForward", typeof(Commands));
            _bringToFront = new RoutedUICommand("BringToFront elements", "BringToFront", typeof(Commands));
            _sendBackward = new RoutedUICommand("SendBackward elements", "SendBackward", typeof(Commands));
            _sendToBack = new RoutedUICommand("SendToBack elements", "SendToBack", typeof(Commands));
            _alignTop = new RoutedUICommand("AlignTop elements", "AlignTop", typeof(Commands));
            _alignVerticalCenters = new RoutedUICommand("AlignVerticalCenters elements", "AlignVerticalCenters", typeof(Commands));
            _alignBottom = new RoutedUICommand("AlignBottom elements", "AlignBottom", typeof(Commands));
            _alignLeft = new RoutedUICommand("AlignLeft elements", "AlignLeft", typeof(Commands));
            _alignHorizontalCenters = new RoutedUICommand("AlignHorizontalCenters elements", "AlignHorizontalCenters", typeof(Commands));
            _alignRight = new RoutedUICommand("AlignRight elements", "AlignRight", typeof(Commands));
            _distributeHorizontal = new RoutedUICommand("DistributeHorizontal elements", "DistributeHorizontal", typeof(Commands));
            _distributeVertical = new RoutedUICommand("DistributeVertical elements", "DistributeVertical", typeof(Commands));
            _selectAll = new RoutedUICommand("SelectAll elements", "SelectAll", typeof(Commands));
        }

        public static RoutedUICommand Group { get => _group;  }
        public static RoutedUICommand Ungroup { get => _ungroup;  }
        public static RoutedUICommand BringForward { get => _bringForward;  }
        public static RoutedUICommand BringToFront { get => _bringToFront; }
        public static RoutedUICommand SendBackward { get => _sendBackward;  }
        public static RoutedUICommand SendToBack { get => _sendToBack;  }
        public static RoutedUICommand AlignTop { get => _alignTop; }
        public static RoutedUICommand AlignVerticalCenters { get => _alignVerticalCenters;  }
        public static RoutedUICommand AlignBottom { get => _alignBottom;  }
        public static RoutedUICommand AlignLeft { get => _alignLeft;  }
        public static RoutedUICommand AlignHorizontalCenters { get => _alignHorizontalCenters; }
        public static RoutedUICommand AlignRight { get => _alignRight;  }
        public static RoutedUICommand DistributeHorizontal { get => _distributeHorizontal;  }
        public static RoutedUICommand DistributeVertical { get => _distributeVertical;  }
        public static RoutedUICommand SelectAll { get => _selectAll;  }
        public static RoutedUICommand New { get => _new; }

        public static void BindCommandsToDiagram()
        {
            diagram.CommandBindings.Add(new CommandBinding(New, New_Executed));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(Group, Group_Executed, Group_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(Ungroup, Ungroup_Executed, Ungroup_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(BringForward, BringForward_Executed, Order_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(BringToFront, BringToFront_Executed, Order_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(SendBackward, SendBackward_Executed, Order_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(SendToBack, SendToBack_Executed, Order_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(AlignTop, AlignTop_Executed, Align_Enabled));
          // diagram.CommandBindings.Add(new CommandBinding(AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(AlignBottom, AlignBottom_Executed, Align_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(AlignLeft, AlignLeft_Executed, Align_Enabled));
           // diagram.CommandBindings.Add(new CommandBinding(AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(AlignRight, AlignRight_Executed, Align_Enabled));
          // diagram.CommandBindings.Add(new CommandBinding(DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
          //  diagram.CommandBindings.Add(new CommandBinding(DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));
            diagram.CommandBindings.Add(new CommandBinding(SelectAll, SelectAll_Executed));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            diagram.AllowDrop = true;
            Clipboard.Clear();
        }
        #region New Command

        public static void New_Executed (object sender, ExecutedRoutedEventArgs e)
        {
            Globals.diagram.ClearAll();
            Globals.diagram.Selection.Clear();
        }

        #endregion

        #region Open Command

        public static void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                openFileDialog.Filter = "XML files (*.xml)|*.xml";

            try
            {
                diagram.LoadFromXml(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Save Command

        public static void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            try
            {
                diagram.SaveToXml(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Print Command

        public static void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            diagram.Selection.Dispose();

            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                printDialog.PrintVisual(diagram, "WPF Diagram");
            }
            //attention to see if it works and then delete the top
            diagram.Print();
        }


        #endregion

        #region Copy Command

        public static void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //to test
            diagram.CopyToClipboard(false);
        }

        public static void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Paste Command

        public static void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Vector offset = new Vector(1, 1);
            diagram.PasteFromClipboard(offset, true);
            Clipboard.Clear();
        }

        public static void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        #endregion

        #region Delete Command

        public static void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
             diagram.Selection.Clear(); 

        }

        public static void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Cut Command

        public static void Cut_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.CutToClipboard(false, true);

        public static void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Group Command

        public static void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (diagram.Selection.Nodes.Count > 1)
            {
                var groupNode = diagram.Factory.CreateShapeNode(0, 0, 1, 1);
                groupNode.ZBottom(false);
                groupNode.Transparent = true;

                groupNode.HandlesStyle = HandlesStyle.RoundAndSquare2;
                //to ne tested
                diagram.ActiveItemHandlesStyle.HandleBrush = Brushes.LightBlue;
                diagram.ActiveItemHandlesStyle.DashPen.Brush = Brushes.LightBlue;
                //
                groupNode.HandlesStyle = HandlesStyle.MoveOnly;

                CreateGroup(groupNode, diagram.Selection.Nodes);
                foreach (DiagramNode child in groupNode.SubordinateGroup.AttachedNodes)
                    child.Locked = true;
                groupNode.SubordinateGroup.AutoDeleteItems = true;
                diagram.Selection.Change(groupNode);
            }
        }

        public static void CreateGroup(DiagramNode node, DiagramNodeCollection children)
        {
            Rect r = Rect.Empty;
            foreach (DiagramNode child in children)
            {
                Rect cb = child.Bounds;
                r = r.IsEmpty ? cb : Rect.Union(r, cb);
            }
            double margin = 5;
            r.Inflate(margin, margin);
            node.Bounds = r;
            foreach (DiagramNode child in children)
                child.AttachTo(node, AttachToNode.TopLeft);
        }


        public static void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region Ungroup Command

        public static void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var groupContainer = diagram.ActiveItem as ShapeNode;
            if (groupContainer != null &&
                  groupContainer.SubordinateGroup != null)
            {
                var attachedNodes = new List<DiagramNode>();
                foreach (DiagramNode node in groupContainer.SubordinateGroup.AttachedNodes)
                    attachedNodes.Add(node);
                foreach (DiagramNode node in attachedNodes)
                {
                    node.Locked = false;
                    node.Detach();
                }
                diagram.Nodes.Remove(groupContainer);
            }
        }

        public static void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = diagram.Selection.GetSize() > 0;

        #endregion

        #region BringForward Command

        public static void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            diagram.Selection.ZLevelUp(false);
        }

        public static void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region BringToFront Command

        public static void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZTop(false);

        #endregion

        #region SendBackward Command

        public static void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZLevelDown(false);

        #endregion

        #region SendToBack Command

        public static void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZBottom(false);


        #endregion

        #region AlignTop Command

        public static void AlignCenterX()
        {
            Align(
                r => r.X + r.Width / 2,
                (r, coord) => new Rect(coord - r.Width / 2, r.Y, r.Width, r.Height));
        }

        public static void AlignCenterY()
        {
            Align(
                r => r.Y + r.Height / 2,
                (r, coord) => new Rect(r.X, coord - r.Height / 2, r.Width, r.Height));
        }

        public static void Align(
            Func<Rect, double> getter,
            Func<Rect, double, Rect> setter)
        {
            var target = diagram.ActiveItem as DiagramNode;
            if (target != null)
            {
                var alignedCoord = getter(target.Bounds);
                foreach (var node in diagram.Selection.Nodes)
                {
                    if (node == target)
                        continue;
                    node.Bounds = setter(node.Bounds, alignedCoord);
                }
            }
        }

        public static void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            {
                Align(
                    r => r.Top,
                    (r, coord) => new Rect(r.X, coord, r.Width, r.Height));
            }

        }

        public static void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = diagram.Selection.GetSize() > 0;
        }

        #endregion

        #region AlignVerticalCenters Command

        /*public static void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }*/

        #endregion

        #region AlignBottom Command

        public static void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Bottom,
                (r, coord) => new Rect(r.X, coord - r.Height, r.Width, r.Height));
        }

        #endregion

        #region AlignLeft Command

        public static void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Left,
                (r, coord) => new Rect(coord, r.Y, r.Width, r.Height));
        }

        #endregion

        #region AlignHorizontalCenters Command

        /*public static void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double center = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = center - (Canvas.GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }*/

        #endregion

        #region AlignRight Command

        public static void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Right,
                (r, coord) => new Rect(coord - r.Width, r.Y, r.Width, r.Height));
        }

        #endregion

        #region DistributeHorizontal Command

        /*public static void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            diagram.DiagramItem.Arrange.AlignCenterX
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemLeft = Canvas.GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Double.MaxValue;
                double right = Double.MinValue;
                double sumWidth = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    left = Math.Min(left, diagram.);
                    right = Math.Max(right, Canvas.GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                double offset = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance;
                }
            }
        }

        public static void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        public static void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemTop = Canvas.GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Double.MaxValue;
                double bottom = Double.MinValue;
                double sumHeight = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    top = Math.Min(top, Canvas.GetTop(item));
                    bottom = Math.Max(bottom, Canvas.GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                double offset = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance;
                }
            }
        }*/

        #endregion

        #region SelectAll Command

        public static void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        
            {
                foreach (var item in diagram.Items)
                    diagram.Selection.AddItem(item);
            }
        
        internal void ClearSelection()
        {
            diagram.Selection.Clear();
        }

        #endregion

        #region Helper Methods

        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    return XElement.Load(openFile.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

       

        

        

        

        

        

       

       


      

        #endregion
    }
}

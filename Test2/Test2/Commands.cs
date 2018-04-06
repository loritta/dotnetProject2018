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
        private List<ISelectable> currentSelection;
        private Diagram diagram = Globals.diagram;

        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();

        public Commands()
        {
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(Group, Group_Executed, Group_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(Ungroup, Ungroup_Executed, Ungroup_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(BringForward, BringForward_Executed, Order_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(BringToFront, BringToFront_Executed, Order_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(SendBackward, SendBackward_Executed, Order_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(SendToBack, SendToBack_Executed, Order_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignTop, AlignTop_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignBottom, AlignBottom_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignLeft, AlignLeft_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(AlignRight, AlignRight_Executed, Align_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));
            Globals.diagram.CommandBindings.Add(new CommandBinding(SelectAll, SelectAll_Executed));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            Globals.diagram.AllowDrop = true;
            Clipboard.Clear();
        }
        #region New Command

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Globals.diagram.ClearAll();
            Globals.diagram.Selection.Clear();
        }

        #endregion

        #region Open Command

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                openFileDialog.Filter = "XML files (*.xml)|*.xml";
            diagram.LoadFromXml(openFileDialog.FileName);
        }

        #endregion

        #region Save Command

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            diagram.SaveToXml(saveFileDialog.FileName);
        }

        #endregion

        #region Print Command

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //to test
            diagram.CopyToClipboard(false);
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Paste Command

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Vector offset = new Vector(1, 1);
            diagram.PasteFromClipboard(offset, true);
            Clipboard.Clear();
        }
      
        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        #endregion

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
             diagram.Selection.Clear(); 

        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Cut Command

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.CutToClipboard(false, true);

        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region Group Command

        private void Group_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void CreateGroup(DiagramNode node, DiagramNodeCollection children)
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


        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region Ungroup Command

        private void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = diagram.Selection.GetSize() > 0;

        #endregion

        #region BringForward Command

        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            diagram.Selection.ZLevelUp(false);
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = diagram.Selection.Items.Count() > 0;
        }

        #endregion

        #region BringToFront Command

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZTop(false);

        #endregion

        #region SendBackward Command

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZLevelDown(false);

        #endregion

        #region SendToBack Command

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e) => diagram.Selection.ZBottom(false);


        #endregion

        #region AlignTop Command

        void AlignCenterX()
        {
            Align(
                r => r.X + r.Width / 2,
                (r, coord) => new Rect(coord - r.Width / 2, r.Y, r.Width, r.Height));
        }

        void AlignCenterY()
        {
            Align(
                r => r.Y + r.Height / 2,
                (r, coord) => new Rect(r.X, coord - r.Height / 2, r.Width, r.Height));
        }

        void Align(
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

        private void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            {
                Align(
                    r => r.Top,
                    (r, coord) => new Rect(r.X, coord, r.Width, r.Height));
            }

        }

        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = diagram.Selection.GetSize() > 0;
        }

        #endregion

        #region AlignVerticalCenters Command

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
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
        }

        #endregion

        #region AlignBottom Command

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Bottom,
                (r, coord) => new Rect(r.X, coord - r.Height, r.Width, r.Height));
        }

        #endregion

        #region AlignLeft Command

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Left,
                (r, coord) => new Rect(coord, r.Y, r.Width, r.Height));
        }

        #endregion

        #region AlignHorizontalCenters Command

        private void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
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
        }

        #endregion

        #region AlignRight Command

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Align(
                r => r.Right,
                (r, coord) => new Rect(coord - r.Width, r.Y, r.Width, r.Height));
        }

        #endregion

        #region DistributeHorizontal Command

        private void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        private void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
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
        }

        #endregion

        #region SelectAll Command

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            diagram.SelectAll();
        }
        internal void ClearSelection()
        {
            diagram.Selection.Clear();
        }
        internal void SelectionSelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(this.Children.OfType<ISelectable>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
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

        void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return null;
                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        private XElement SerializeDesignerItems(IEnumerable<DiagramNode> designerItems)
        {
            XElement serializedItems = new XElement("DiagramNode",
                                       from item in designerItems
                                       let contentXaml = XamlWriter.Save(((DiagramNode)item).Content)
                                       select new XElement("DesignerItem",
                                                  new XElement("Left", Canvas.GetLeft(item)),
                                                  new XElement("Top", Canvas.GetTop(item)),
                                                  new XElement("Width", item.Width),
                                                  new XElement("Height", item.Height),
                                                  new XElement("ID", item.ID),
                                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                                  new XElement("IsGroup", item.IsGroup),
                                                  new XElement("ParentID", item.ParentID),
                                                  new XElement("Content", contentXaml)
                                              )
                                   );

            return serializedItems;
        }

        private XElement SerializeConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", Canvas.GetZIndex(connection))
                                     )
                                  );

            return serializedConnections;
        }

        private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            DesignerItem item = new DesignerItem(id);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
            Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
            item.Content = content;
            return item;
        }

        private void CopyCurrentSelection()
        {
            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            List<Connection> selectedConnections =
                this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }

            XElement designerItemsXML = SerializeDesignerItems(selectedDesignerItems);
            XElement connectionsXML = SerializeConnections(selectedConnections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void DeleteCurrentSelection()
        {
            foreach (Connection connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
            }

            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                List<Connector> connectors = new List<Connector>();
                GetConnectors(cd, connectors);

                foreach (Connector connector in connectors)
                {
                    foreach (Connection con in connector.Connections)
                    {
                        this.Children.Remove(con);
                    }
                }
                this.Children.Remove(item);
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }

        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItem item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        private Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        #endregion
    }
}

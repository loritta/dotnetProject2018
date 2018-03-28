using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ProjectDiagramV1
{
    class UndoRedo
    {

    }
    interface IUndoRedo
    {
        void Undo(int level);
        void Redo(int level);
        void InsertObjectforUndoRedo(ChangeRepresentationObject dataobject);
    }
    public class ChangeRepresentationObject
    {
        public ActionType Action;
        public Point Margin;
        public double Width;
        public double height;
        public FrameworkElement UiElement;
    }

    public partial class UnDoRedo : IUndoRedo
    {
        private Stack<ChangeRepresentationObject> _UndoActionsCollection =
                    new Stack<ChangeRepresentationObject>();
        private Stack<ChangeRepresentationObject> _RedoActionsCollection =
                    new Stack<ChangeRepresentationObject>();

        private Canvas _Container;

        public Canvas Container
        {
            get { return _Container; }
            set { _Container = value; }
        }
        #region IUndoRedo Members

        public void Undo(int level)
        {
            for (int i = 1; i <= level; i++)
            {
                if (_UndoActionsCollection.Count == 0) return;

                ChangeRepresentationObject Undostruct = _UndoActionsCollection.Pop();
                if (Undostruct.Action == ActionType.Delete)
                {
                    Container.Children.Add(Undostruct.UiElement);
                    this.RedoPushInUnDoForDelete(Undostruct.UiElement);
                }
                else if (Undostruct.Action == ActionType.Insert)
                {
                    Container.Children.Remove(Undostruct.UiElement);
                    this.RedoPushInUnDoForInsert(Undostruct.UiElement);
                }
                else if (Undostruct.Action == ActionType.Resize)
                {
                    if (_UndoActionsCollection.Count != 0)
                    {
                        Point previousMarginOfSelectedObject = new Point
            (((FrameworkElement)Undostruct.UiElement).Margin.Left,
                            ((FrameworkElement)Undostruct.UiElement).Margin.Top);
                        this.RedoPushInUnDoForResize(previousMarginOfSelectedObject,
            Undostruct.UiElement.Width,
                            Undostruct.UiElement.Height, Undostruct.UiElement);
                        Undostruct.UiElement.Margin = new Thickness
            (Undostruct.Margin.X, Undostruct.Margin.Y, 0, 0);
                        Undostruct.UiElement.Height = Undostruct.height;
                        Undostruct.UiElement.Width = Undostruct.Width;
                    }
                }
                else if (Undostruct.Action == ActionType.Move)
                {
                    Point previousMarginOfSelectedObject = new Point
            (((FrameworkElement)Undostruct.UiElement).Margin.Left,
                        ((FrameworkElement)Undostruct.UiElement).Margin.Top);
                    this.RedoPushInUnDoForMove(previousMarginOfSelectedObject,
                            Undostruct.UiElement);
                    Undostruct.UiElement.Margin = new Thickness
            (Undostruct.Margin.X, Undostruct.Margin.Y, 0, 0);
                }
            }
        }

        public void Redo(int level)
        {
            for (int i = 1; i <= level; i++)
            {
                if (_RedoActionsCollection.Count == 0) return;

                ChangeRepresentationObject Undostruct = _RedoActionsCollection.Pop();
                if (Undostruct.Action == ActionType.Delete)
                {
                    Container.Children.Remove(Undostruct.UiElement);
                    this.PushInUnDoForDelete(Undostruct.UiElement);
                }
                else if (Undostruct.Action == ActionType.Insert)
                {
                    Container.Children.Add(Undostruct.UiElement);
                    this.PushInUnDoForInsert(Undostruct.UiElement);
                }
                else if (Undostruct.Action == ActionType.Resize)
                {
                    Point previousMarginOfSelectedObject = new Point
            (((FrameworkElement)Undostruct.UiElement).Margin.Left,
                        ((FrameworkElement)Undostruct.UiElement).Margin.Top);
                    this.PushInUnDoForResize(previousMarginOfSelectedObject,
                        Undostruct.UiElement.Width,
                        Undostruct.UiElement.Height, Undostruct.UiElement);
                    Undostruct.UiElement.Margin = new Thickness
            (Undostruct.Margin.X, Undostruct.Margin.Y, 0, 0);
                    Undostruct.UiElement.Height = Undostruct.height;
                    Undostruct.UiElement.Width = Undostruct.Width;
                }
                else if (Undostruct.Action == ActionType.Move)
                {
                    Point previousMarginOfSelectedObject = new Point
            (((FrameworkElement)Undostruct.UiElement).Margin.Left,
                        ((FrameworkElement)Undostruct.UiElement).Margin.Top);
                    this.PushInUnDoForMove(previousMarginOfSelectedObject,
                            Undostruct.UiElement);
                    Undostruct.UiElement.Margin = new Thickness
                (Undostruct.Margin.X, Undostruct.Margin.Y, 0, 0);
                }
            }
        }

        public void InsertObjectforUndoRedo(ChangeRepresentationObject dataobject)
        {
            _UndoActionsCollection.Push(dataobject); _RedoActionsCollection.Clear();
        }

    }

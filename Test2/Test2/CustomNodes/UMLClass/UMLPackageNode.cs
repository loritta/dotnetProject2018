﻿using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Test2
{
    public class UMLPackageNode : TemplatedNode
    {
        AnchorPattern squareAnchors = new AnchorPattern(new AnchorPoint[]
                {
                    // if you picture the node as a grid, top left is 0,0
                    // bottom right is 100,100, center is 50,50, etc...
                    // first value is X axis, 2nd is the Y axis
                    // yes i had to figure this out myself, no i dont know what true is for but its true
                    // what is documentation
                    new AnchorPoint(50, 0, true, true),
                    new AnchorPoint(100, 50, true, true),
                    new AnchorPoint(50, 100, true, true),
                    new AnchorPoint(0, 50, true, true)
                });

        private const string PackageNamePlaceholder = "<Package Name>";

        static UMLPackageNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UMLPackageNode), new FrameworkPropertyMetadata(typeof(UMLPackageNode)));
        }
        public UMLPackageNode()
        {
            Init();
        }

        public UMLPackageNode(Diagram parent)
            : base(parent)
        {
            Init();
        }

        void Init()
        {
            PackageName = PackageNamePlaceholder;
            HandlesStyle = HandlesStyle.HatchHandles3;

            this.AnchorPattern = squareAnchors;
            this.TextAlignment = TextAlignment.Center;
            this.TextVerticalAlignment = AlignmentY.Center;
        }

        public string PackageName
        {
            get { return (string)GetValue(PackageNameProperty); }
            set { SetValue(PackageNameProperty, value); }
        }
        public static readonly DependencyProperty PackageNameProperty = DependencyProperty.Register(
            "PackageName", typeof(string), typeof(UMLInterfaceNode), new PropertyMetadata(""));

    }
}

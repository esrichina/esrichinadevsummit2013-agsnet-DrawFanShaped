using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace DrawFanShaped
{
    public class DrawFanShaoped : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public DrawFanShaoped()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnMouseDown(MouseEventArgs arg)
        {
            IActiveView pActiveView = ArcMap.Document.ActiveView;
            IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
            IPoint pPoint = displayTransformation.ToMapPoint(arg.X, arg.Y);
            Parameter form1 = new Parameter(pPoint, pActiveView);
            form1.Show();            
            //base.OnMouseDown(arg);
        }
    }

}

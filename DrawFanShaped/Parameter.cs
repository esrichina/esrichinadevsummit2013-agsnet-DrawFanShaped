using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace DrawFanShaped
{
    public partial class Parameter : Form
    {
        private double radius = 0;
        private double small_radius = 0;
        private double start_angle = 0;
        private double central_angle = 0;
        private IPoint pPoint;
        private IActiveView pActiveView;

        public Parameter(IPoint point,IActiveView activeView)
        {
            InitializeComponent();
            pPoint = point;
            pActiveView = activeView;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            radius = Double.Parse(textBox1.Text.Trim());
            small_radius = Double.Parse(textBox2.Text.Trim());
            start_angle = Double.Parse(textBox3.Text.Trim());
            central_angle = Double.Parse(textBox4.Text.Trim());
            this.Hide();
            DrawFanShaped(pPoint.X, pPoint.Y, pActiveView);        
        }

        private void DrawFanShaped(double x, double y, IActiveView pActiveView)
        {
            double radius = this.radius;
            double small_radius = this.small_radius;
            double start_angle = this.start_angle;
            double central_angle = this.central_angle;

            if (pActiveView != null)
            {
                IGraphicsContainer graphicsContainer = pActiveView as IGraphicsContainer;

                //画大圆
                IPoint pCenterPoint = new PointClass();
                pCenterPoint.PutCoords(x, y);
                ICircularArc pCircularArc = new CircularArcClass();
                pCircularArc.PutCoordsByAngle(pCenterPoint, start_angle * Math.PI / 180.0, central_angle * Math.PI / 180.0, radius);
                IPoint pStartPoint = pCircularArc.FromPoint;
                IPoint pEndPoint = pCircularArc.ToPoint;
                ILine pLine1 = new LineClass();
                pLine1.PutCoords(pCenterPoint, pStartPoint);
                ILine pLine2 = new LineClass();
                pLine2.PutCoords(pEndPoint, pCenterPoint);
                ISegmentCollection pRing1 = new PolygonClass();
                ISegment pSegment1 = pLine1 as ISegment;
                pRing1.AddSegment(pSegment1);
                ISegment pSegment2 = pCircularArc as ISegment;
                pRing1.AddSegment(pSegment2);
                ISegment pSegment3 = pLine2 as ISegment;
                pRing1.AddSegment(pSegment3);
                //小圆
                ICircularArc pCircularArc1 = new CircularArcClass();
                pCircularArc1.PutCoordsByAngle(pCenterPoint, start_angle * Math.PI / 180.0, central_angle * Math.PI / 180.0, small_radius);
                IPoint pStartPoint1 = pCircularArc1.FromPoint;
                IPoint pEndPoint1 = pCircularArc1.ToPoint;
                ILine pLine3 = new LineClass();
                pLine3.PutCoords(pCenterPoint, pStartPoint1);
                ILine pLine4 = new LineClass();
                pLine4.PutCoords(pEndPoint1, pCenterPoint);
                ISegmentCollection pRing2 = new PolygonClass();
                ISegment pSegment4 = pLine3 as ISegment;
                pRing2.AddSegment(pSegment4);
                ISegment pSegment5 = pCircularArc1 as ISegment;
                pRing2.AddSegment(pSegment5);
                ISegment pSegment6 = pLine4 as ISegment;
                pRing2.AddSegment(pSegment6);
                //简化
                ITopologicalOperator pTopoLogical1 = pRing1 as ITopologicalOperator;
                pTopoLogical1.Simplify();
                ITopologicalOperator pTopoLogical2 = pRing2 as ITopologicalOperator;
                pTopoLogical2.Simplify();
                IGeometry geometry = pTopoLogical1.Difference(pTopoLogical2 as IGeometry);
                //产生一个SimpleFillSymbol符号
                IRgbColor rgbColor = new RgbColorClass();
                rgbColor.Red = 255;
                rgbColor.Green = 0;
                rgbColor.Blue = 0;
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Width = 1;
                ISimpleFillSymbol simpleFillSymbol;
                simpleFillSymbol = new SimpleFillSymbolClass();
                simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                //设置颜色
                IRgbColor rgbcolor = new RgbColorClass();
                rgbcolor.Red = 0;
                rgbcolor.Green = 0;
                rgbcolor.Blue = 255;
                simpleFillSymbol.Color = rgbcolor as IColor;
                simpleFillSymbol.Outline = simpleLineSymbol;
                IFillShapeElement fillShapeElement = new PolygonElementClass();
                fillShapeElement.Symbol = simpleFillSymbol;
                IElement pElement = fillShapeElement as IElement;
                pElement.Geometry = geometry;
                graphicsContainer.AddElement(pElement, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }
    }
}

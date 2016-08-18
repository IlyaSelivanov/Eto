using Eto.Drawing;
using System;
using sd = System.Drawing;
using sd2 = System.Drawing.Drawing2D;

namespace Eto.WinForms.Drawing
{
	/// <summary>
	/// Handler for <see cref="Pen"/>
	/// </summary>
	/// <copyright>(c) 2012-2014 by Curtis Wensley</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public class PenHandler : Pen.IHandler
	{
		public object Create (Brush brush, float thickness)
		{
			var pen = new sd.Pen (brush.ToSD(Rectangle.Empty), thickness);
			pen.StartCap = pen.EndCap = PenLineCap.Square.ToSD ();
			pen.DashCap = sd2.DashCap.Flat;
			pen.MiterLimit = 10f;
			return pen;
		}

		public Brush GetBrush(Pen widget)
		{
			return null;
		}

		public void SetColor (Pen widget, Color color)
		{
			widget.ToSD ().Color = color.ToSD ();
		}

		public float GetThickness (Pen widget)
		{
			return widget.ToSD ().Width;
		}

		public void SetThickness (Pen widget, float thickness)
		{
			widget.ToSD ().Width = thickness;
		}

		public PenLineJoin GetLineJoin (Pen widget)
		{
			return widget.ToSD ().LineJoin.ToEto ();
		}

		public void SetLineJoin (Pen widget, PenLineJoin lineJoin)
		{
			widget.ToSD ().LineJoin = lineJoin.ToSD ();
		}

		public PenLineCap GetLineCap (Pen widget)
		{
			return widget.ToSD ().StartCap.ToEto ();
		}

		public void SetLineCap (Pen widget, PenLineCap lineCap)
		{
			var pen = widget.ToSD ();
			// get dash style before changing cap
			var dashStyle = widget.DashStyle;
			pen.StartCap = pen.EndCap = lineCap.ToSD ();
			pen.DashCap = lineCap == PenLineCap.Round ? sd2.DashCap.Round : sd2.DashCap.Flat;
			SetDashStyle (widget, dashStyle);
		}

		public float GetMiterLimit (Pen widget)
		{
			return widget.ToSD ().MiterLimit;
		}

		public void SetMiterLimit (Pen widget, float miterLimit)
		{
			widget.ToSD ().MiterLimit = miterLimit;
		}

		public DashStyle GetDashStyle (Pen widget)
		{
			var pen = widget.ToSD ();
			if (pen.DashStyle == sd2.DashStyle.Solid)
				return DashStyles.Solid;
			else {
				var offset = pen.StartCap == sd2.LineCap.Square ? pen.DashOffset - 0.5f : pen.DashOffset;
				return new DashStyle (offset, pen.DashPattern);
			}
		}

		public void SetDashStyle (Pen widget, DashStyle dashStyle)
		{
			var pen = widget.ToSD ();

			pen.DashOffset = 0;
			if (dashStyle == null || dashStyle.IsSolid)
				pen.DashStyle = sd2.DashStyle.Solid;
			else if (dashStyle.Equals(DashStyles.Dash))
				pen.DashStyle = sd2.DashStyle.Dash;
			else if (dashStyle.Equals(DashStyles.DashDot))
				pen.DashStyle = sd2.DashStyle.DashDot;
			else if (dashStyle.Equals(DashStyles.DashDotDot))
				pen.DashStyle = sd2.DashStyle.DashDotDot;
			else
			{
				pen.DashStyle = sd2.DashStyle.Custom;
				pen.DashPattern = dashStyle.Dashes;
				pen.DashOffset = dashStyle.Offset;
			}
			if (pen.StartCap == sd2.LineCap.Square)
			{
				pen.DashOffset += 0.5f;
			}
		}
	}
}

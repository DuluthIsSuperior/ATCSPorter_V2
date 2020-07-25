using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Configuration;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	class Block {
		internal Bitmap bmp = null;
		internal Graphics g = null;
		internal SolidBrush brush = null;

		public virtual void PaintBlock(PictureBox pb, Color color) {
			if (g == null) {
				bmp = new Bitmap(pb.Image);
				g = Graphics.FromImage(bmp);
				brush = new SolidBrush(color);
			} else {
				brush.Dispose();
				brush = null;
				pb.Image = bmp;
				pb.Invalidate();
				g.Dispose();
				g = null;
			}
		}

		public virtual List<object> GetPolygons() {
			return null;
		}
	}

	class LinearBlock : Block {
		RectangleF westBorder;
		RectangleF block;
		RectangleF eastBorder;

		public LinearBlock(int westX, int westY, int blockLength) {
			westBorder = new RectangleF(westX, westY, 1, 12);
			block = new RectangleF(westX + 1, westY + 4, blockLength, 4);
			eastBorder = new RectangleF(westX + blockLength + 1, westY, 1, 12);
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			g.FillRectangle(brush, westBorder);
			g.FillRectangle(brush, block);
			g.FillRectangle(brush, eastBorder);
			base.PaintBlock(pb, color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				westBorder,
				block,
				eastBorder
			};
		}
	}

	class ArrowHead : Block {
		string direction;
		Point[] points;

		public ArrowHead(int x, int y, string pointingDirection) {
			direction = pointingDirection;
			if (direction == "W") {
				points = new Point[] { new Point(x, y), new Point(x, y + 13), new Point(x - 6, y + 6) };
			} else if (direction == "NE") {
				points = new Point[] { new Point(x, y), new Point(x + 7, y + 7), new Point(x + 7, y) };
			} else if (direction == "E") {
				points = new Point[] { new Point(x, y), new Point(x, y + 13), new Point(x + 6, y + 6) };
			} else if (direction == "SW") {
				points = new Point[] { new Point(x, y), new Point(x, y + 7), new Point(x + 7, y + 7) };
			}
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			g.FillPolygon(brush, points);
			base.PaintBlock(pb, color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				points[0], points[1], points[2]
			};
		}
	}

	class Turnout : Block {
		List<RectangleF> rectangles;

		public Turnout(int x, int y, string dir) {
			rectangles = new List<RectangleF>();
			if (dir == "/-") {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x - 2, y + 2, 4, 4));
				rectangles.Add(new RectangleF(x - 4, y + 4, 4, 4));
				rectangles.Add(new RectangleF(x - 6, y + 6, 4, 4));
			} else if (dir == "-/") {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x + 2, y - 2, 4, 4));
				rectangles.Add(new RectangleF(x + 4, y - 4, 4, 4));
				rectangles.Add(new RectangleF(x + 6, y - 6, 4, 4));
			} else if (dir == "-\\") {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x + 2, y + 2, 4, 4));
				rectangles.Add(new RectangleF(x + 4, y + 4, 4, 4));
				rectangles.Add(new RectangleF(x + 6, y + 6, 4, 4));
			} else if (dir == "\\-") {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x - 2, y - 2, 4, 4));
				rectangles.Add(new RectangleF(x - 4, y - 4, 4, 4));
				rectangles.Add(new RectangleF(x - 6, y - 6, 4, 4));
			} else {
				string message = "Invalid turnout specification";
				if (dir.Contains("--")) {
					message += "; Use one dash instead of two";
				}
				if (dir.Contains("//")) {
					message += "; Use one forward slash, not two";
				}
				throw new ArgumentException(message);
			}
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			foreach (RectangleF rectangle in rectangles) {
				g.FillRectangle(brush, rectangle);
			}
			base.PaintBlock(pb, color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				rectangles[0], rectangles[1], rectangles[2], rectangles[3]
			};
		}
	}

	class DiagonalBlock : Block {
		List<RectangleF> rectangles;

		public DiagonalBlock(int x, int y, char dir) {
			rectangles = new List<RectangleF>();
			if (dir == '/') {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x - 2, y + 2, 4, 4));
				rectangles.Add(new RectangleF(x - 4, y + 4, 4, 4));
				rectangles.Add(new RectangleF(x - 6, y + 6, 4, 4));
				rectangles.Add(new RectangleF(x - 8, y + 8, 4, 4));
				rectangles.Add(new RectangleF(x - 10, y + 10, 4, 4));
				rectangles.Add(new RectangleF(x - 12, y + 12, 4, 4));
			} else if (dir == '\\') {
				rectangles.Add(new RectangleF(x, y, 4, 4));
				rectangles.Add(new RectangleF(x + 2, y + 2, 4, 4));
				rectangles.Add(new RectangleF(x + 4, y + 4, 4, 4));
				rectangles.Add(new RectangleF(x + 6, y + 6, 4, 4));
				rectangles.Add(new RectangleF(x + 8, y + 8, 4, 4));
				rectangles.Add(new RectangleF(x + 10, y + 10, 4, 4));
				rectangles.Add(new RectangleF(x + 12, y + 12, 4, 4));
			} else {
				throw new ArgumentException("Invalid direction");
			}
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			foreach (RectangleF rectangle in rectangles) {
				g.FillRectangle(brush, rectangle);
			}
			base.PaintBlock(pb, color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				rectangles[0], rectangles[1], rectangles[2], rectangles[3], rectangles[4], rectangles[5], rectangles[6]
			};
		}
	}

	class BlockConfiguration : Block {
		List<object> Backdrop;
		Dictionary<string, List<object>> routes;

		//private void TestPaint(PictureBox pb, object shape, Color color) {
		//	if (shape.GetType().BaseType == typeof(Block)) {
		//		((Block) shape).PaintBlock(pb, color);
		//	} else {
		//		Bitmap bmp = new Bitmap(pb.Image);
		//		Graphics g = Graphics.FromImage(bmp);
		//		if (shape.GetType() == typeof(RectangleF)) {
		//			SolidBrush brush = new SolidBrush(color);   // Color.Lime
		//			g.FillRectangle(brush, (RectangleF) shape);
		//			brush.Dispose();
		//		} else if (shape.GetType() == typeof(Line)) {
		//			Line line = (Line) shape;
		//			Pen pen = new Pen(color);
		//			g.DrawLine(pen, line.GetPoint1(), line.GetPoint2());
		//			pen.Dispose();
		//		} else {
		//			throw new ArgumentException("Invalid shape type");
		//		}
		//		pb.Image = bmp;
		//		pb.Invalidate();
		//	}
		//}

		public BlockConfiguration(PictureBox pb, Dictionary<string, List<object>> blocks, List<object> backdrop) {
			Backdrop = new List<object>();
			foreach (object shape in backdrop) {
				if (shape.GetType().BaseType == typeof(Block)) {
					List<object> polygons = ((Block) shape).GetPolygons();
					foreach (object polygon in polygons) {
						Backdrop.Add(polygon);
					}
				} else {
					Backdrop.Add(shape);
				}
			}
			//foreach (object shape in backdrop) {
			//	TestPaint(pb, shape, Color.Lime);
			//}

			//foreach (string config in blocks.Keys) {
			//	foreach (object shape in blocks[config]) {
			//		TestPaint(pb, shape, Color.Red);
			//	}
			//}

			routes = blocks;
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			// TODO: Create logic to paint the correct route needed
			base.PaintBlock(pb, color);
		}
	}

	class PartialLinearBlock : Block {
		RectangleF border;
		RectangleF block;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">X-coordinate refers to the top-left corner of the block not including the border</param>
		/// <param name="y">Y-coordinate refers to the top-left corner of the block not including the border</param>
		/// <param name="width"></param>
		/// <param name="borderSide"></param>
		public PartialLinearBlock(int x, int y, int width, char borderSide) {
			block = new RectangleF(x, y, width, 4);
			if (borderSide == 'W') {
				border = new RectangleF(x - 1, y - 4, 1, 12);
			} else if (borderSide == 'E') {
				border = new RectangleF(x + width, y - 4, 1, 12);
			} else {
				throw new ArgumentException("Invalid border side declaration");
			}
		}

		public override void PaintBlock(PictureBox pb, Color color) {
			base.PaintBlock(pb, color);
			g.FillRectangle(brush, border);
			g.FillRectangle(brush, block);
			base.PaintBlock(pb, color);
		}

		public override List<object> GetPolygons() {
			return new List<object> { border, block };
		}
	}

	class Line {
		private Point point1;
		private Point point2;
		
		public Line(int x1, int y1, int x2, int y2) {
			point1 = new Point(x1, y1);
			point2 = new Point(x2, y2);
		}

		public Point GetPoint1() {
			return point1;
		}

		public Point GetPoint2() {
			return point2;
		}
	}
}
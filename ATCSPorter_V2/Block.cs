using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	public class Block {
		internal Bitmap bmp = null;
		internal Graphics g = null;
		internal SolidBrush brush = null;
		readonly PictureBox board;
		readonly Mutex paintMutex = new Mutex();

		public Block(PictureBox pb) {
			board = pb;
		}

		public virtual void PaintBlock(Color color, params string[] routeToPaint) {
			paintMutex.WaitOne();
			if (g == null) {
				bmp = new Bitmap(board.Image);
				g = Graphics.FromImage(bmp);
				brush = new SolidBrush(color);
			} else {
				brush.Dispose();
				brush = null;
				board.Image = bmp;
				board.Invalidate();
				g.Dispose();
				g = null;
			}
			paintMutex.ReleaseMutex();
		}

		public virtual List<object> GetPolygons() {
			return null;
		}

		public bool PaintOnInitValid(bool[] paintOnInit) {
			return paintOnInit.Length > 0 && paintOnInit[0];
		}
	}

	class LinearBlock : Block {
		RectangleF westBorder;
		RectangleF block;
		RectangleF eastBorder;

		public LinearBlock(PictureBox pb, int westX, int westY, int blockLength, params bool[] paintInit):base(pb) {
			westBorder = new RectangleF(westX, westY, 1, 12);
			block = new RectangleF(westX + 1, westY + 4, blockLength, 4);
			eastBorder = new RectangleF(westX + blockLength + 1, westY, 1, 12);
			if (PaintOnInitValid(paintInit)) {
				PaintBlock(Color.White);
			}
		}

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			base.PaintBlock(color);
			g.FillRectangle(brush, westBorder);
			g.FillRectangle(brush, block);
			g.FillRectangle(brush, eastBorder);
			base.PaintBlock(color);
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
		readonly string direction;
		readonly Point[] points;

		public ArrowHead(PictureBox pb, int x, int y, string pointingDirection, params bool[] paintInit):base(pb) {
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
			if (PaintOnInitValid(paintInit)) {
				PaintBlock(Color.White);
			}
		}

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			base.PaintBlock(color);
			g.FillPolygon(brush, points);
			base.PaintBlock(color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				points[0], points[1], points[2]
			};
		}
	}

	class Turnout : Block {
		readonly List<RectangleF> rectangles;

		public Turnout(PictureBox pb, int x, int y, string dir):base(pb) {
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

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			base.PaintBlock(color);
			foreach (RectangleF rectangle in rectangles) {
				g.FillRectangle(brush, rectangle);
			}
			base.PaintBlock(color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				rectangles[0], rectangles[1], rectangles[2], rectangles[3]
			};
		}
	}

	class DiagonalBlock : Block {
		readonly List<RectangleF> rectangles;

		public DiagonalBlock(PictureBox pb, int x, int y, char dir):base(pb) {
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

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			base.PaintBlock(color);
			foreach (RectangleF rectangle in rectangles) {
				g.FillRectangle(brush, rectangle);
			}
			base.PaintBlock(color);
		}

		public override List<object> GetPolygons() {
			return new List<object> {
				rectangles[0], rectangles[1], rectangles[2], rectangles[3], rectangles[4], rectangles[5], rectangles[6]
			};
		}
	}

	class BlockConfiguration : Block {
		readonly List<Block> Backdrop;
		readonly Dictionary<string, List<Block>> routes;

		public BlockConfiguration(PictureBox pb, Dictionary<string, List<Block>> blocks, List<Block> backdrop):base(pb) {
			Backdrop = new List<Block>();
			Backdrop.AddRange(backdrop);
			foreach (Block block in Backdrop) {
				block.PaintBlock(Color.White);
			}

			routes = blocks;
		}

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			List<Block> blocks;
			try {
				blocks = routes[routeToPaint[0]];
			} catch (KeyNotFoundException) {
				throw new ArgumentException($"Route {routeToPaint[0]} does not exist for this block");
			}
			foreach (Block block in blocks) {
				block.PaintBlock(color);
			}
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
		public PartialLinearBlock(PictureBox pb, int x, int y, int width, char borderSide, params bool[] paintInit):base(pb) {
			block = new RectangleF(x, y, width, 4);
			if (borderSide == 'W') {
				border = new RectangleF(x - 1, y - 4, 1, 12);
			} else if (borderSide == 'E') {
				border = new RectangleF(x + width, y - 4, 1, 12);
			} else if (borderSide == '0') { } else {
				throw new ArgumentException("Invalid border side declaration");
			}
			if (PaintOnInitValid(paintInit)) {
				PaintBlock(Color.White);
			}
		}

		public override void PaintBlock(Color color, params string[] routeToPaint) {
			base.PaintBlock(color);
			g.FillRectangle(brush, border);
			g.FillRectangle(brush, block);
			base.PaintBlock(color);
		}

		public override List<object> GetPolygons() {
			return new List<object> { border, block };
		}
	}
}
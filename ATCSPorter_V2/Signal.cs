using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	class Signal {
		internal RectangleF[] foundation;
		internal char direction;

		internal PictureBox board;
		internal Bitmap bmp = null;
		internal Graphics g = null;

		public Signal(PictureBox pb, int x, int y, char directionPointingIn) {
			board = pb;
			direction = directionPointingIn;

			if (direction == 'W') {
				foundation = new RectangleF[] {
					new RectangleF(x - 1, y - 3, 2, 8),
					new RectangleF(x - 3, y - 1, 2, 4)
				};
			} else if (direction == 'E') {
				foundation = new RectangleF[] {
					new RectangleF(x    , y - 3, 2, 8),
					new RectangleF(x + 2, y - 1, 2, 4)
				};
			} else {
				throw new ArgumentException("Invalid direction for signal");
			}
		}

		// TODO: Modify to accept signal indication
		public virtual void PaintSignal() {
			if (g == null) {
				bmp = new Bitmap(board.Image);
				g = Graphics.FromImage(bmp);
				SolidBrush foundationBrush = new SolidBrush(Color.DarkGray);
				g.FillRectangle(foundationBrush, foundation[0]);
				g.FillRectangle(foundationBrush, foundation[1]);
				foundationBrush.Dispose();
			} else {
				board.Image = bmp;
				board.Invalidate();
				g.Dispose();
				g = null;
			}
		}
	}

	class DwarfSignal : Signal {
		internal RectangleF head;
		internal Rectangle[] lights;

		public DwarfSignal(PictureBox pb, int x, int y, char directionPointingIn):base(pb, x, y, directionPointingIn) {
			int topLeftX = x + (direction == 'W' ? -20 : 5);
			int topLeftY = y - 2;
			head = new RectangleF(topLeftX, topLeftY, 16, 6);
			lights = new Rectangle[3];
			lights[direction == 'W' ? 0 : 2] = new Rectangle(topLeftX + 1, topLeftY + 1, 3, 3);
			lights[1] = new Rectangle(topLeftX + 6, topLeftY + 1, 3, 3);
			lights[direction == 'W' ? 2 : 0] = new Rectangle(topLeftX + 11, topLeftY + 1, 3, 3);
		}

		public override void PaintSignal() {
			base.PaintSignal();
			SolidBrush headBrush = new SolidBrush(Color.DarkGray);
			g.FillRectangle(headBrush, head);
			headBrush.Dispose();
			Pen penForLights = new Pen(Color.Black);
			g.DrawRectangle(penForLights, lights[0]);
			g.DrawRectangle(penForLights, lights[1]);
			g.DrawRectangle(penForLights, lights[2]);
			base.PaintSignal();
		}
	}

	class TwoHeadSignal : Signal {
		internal Tuple<RectangleF, Rectangle[]>[] heads;	// Item1 = target, Item2 = lights

		public TwoHeadSignal(PictureBox pb, int x, int y, char directionPointingIn):base(pb, x, y, directionPointingIn) {
			if (directionPointingIn == 'W') {
				int topLeftX = x - 16;
				int topLeftY = y - 5;
				heads = new Tuple<RectangleF, Rectangle[]>[2];
				heads[0] = new Tuple<RectangleF, Rectangle[]>(
					new RectangleF(topLeftX - 10, topLeftY, 22, 12),
					new Rectangle[] {
						new Rectangle(topLeftX - 6, topLeftY + 4, 3, 3),
						new Rectangle(topLeftX - 1, topLeftY + 4, 3, 3),
						new Rectangle(topLeftX + 4, topLeftY + 4, 3, 3)
					}
				);
				heads[1] = new Tuple<RectangleF, Rectangle[]>(
					new RectangleF(topLeftX - 33, topLeftY, 22, 12),
					new Rectangle[] {
						new Rectangle(topLeftX - 29, topLeftY + 4, 3, 3),
						new Rectangle(topLeftX - 24, topLeftY + 4, 3, 3),
						new Rectangle(topLeftX - 19, topLeftY + 4, 3, 3)
					}
				);
			} else if (directionPointingIn == 'E') {
				//int topLeftX = x + 5;
				//int topLeftY = y - 2;
			}
		}

		private Tuple<Point, Point> Line(Point point1, Point point2) {
			return new Tuple<Point, Point>(point1, point2);
		}

		public override void PaintSignal() {
			base.PaintSignal();
			SolidBrush headBrush = new SolidBrush(Color.DarkGray);
			SolidBrush blackBrush = new SolidBrush(Color.Black);
			Pen blackPen = new Pen(Color.Black);

			foreach (Tuple<RectangleF, Rectangle[]> head in heads) {
				Point targetsTopCorner = new Point((int) head.Item1.X, (int) head.Item1.Y);
				g.FillRectangle(headBrush, head.Item1);
				RectangleF[] corners = new RectangleF[] {
					new RectangleF(targetsTopCorner.X     , targetsTopCorner.Y     , 2, 4),
					new RectangleF(targetsTopCorner.X     , targetsTopCorner.Y     , 4, 2),
					new RectangleF(targetsTopCorner.X     , targetsTopCorner.Y +  8, 2, 4),
					new RectangleF(targetsTopCorner.X     , targetsTopCorner.Y + 10, 4, 2),
					new RectangleF(targetsTopCorner.X + 18, targetsTopCorner.Y + 10, 4, 2),
					new RectangleF(targetsTopCorner.X + 20, targetsTopCorner.Y +  8, 2, 4),
					new RectangleF(targetsTopCorner.X + 20, targetsTopCorner.Y     , 2, 4),
					new RectangleF(targetsTopCorner.X + 18, targetsTopCorner.Y     , 4, 2),
					new RectangleF(targetsTopCorner.X + 3, targetsTopCorner.Y + 3, 1, 1),	// apparently this is how you draw a single pixel on the picture
					new RectangleF(targetsTopCorner.X + 3, targetsTopCorner.Y + 8, 1, 1)	// both of these are needed for the hood
				};
				foreach (RectangleF rectangle in corners) {
					g.FillRectangle(blackBrush, rectangle);
				}
				Tuple<Point, Point>[] hoodLines = new Tuple<Point, Point>[] {
					Line(new Point(targetsTopCorner.X + 4, targetsTopCorner.Y + 2), new Point(targetsTopCorner.X + 16, targetsTopCorner.Y + 2)),
					Line(new Point(targetsTopCorner.X + 2, targetsTopCorner.Y + 4), new Point(targetsTopCorner.X + 2, targetsTopCorner.Y + 7)),
					Line(new Point(targetsTopCorner.X + 4, targetsTopCorner.Y + 9), new Point(targetsTopCorner.X + 16, targetsTopCorner.Y + 9))
				};
				foreach (Tuple<Point, Point> line in hoodLines) {
					g.DrawLine(blackPen, line.Item1, line.Item2);
				}
				foreach (Rectangle light in head.Item2) {
					g.DrawRectangle(blackPen, light);
				}
			}

			headBrush.Dispose();
			blackBrush.Dispose();
			blackPen.Dispose();
			// TODO: Paint signal heads and lights here
			base.PaintSignal();
		}
	}
}

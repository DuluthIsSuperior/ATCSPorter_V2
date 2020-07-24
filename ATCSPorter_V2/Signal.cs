using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	class Signal {
		internal RectangleF[] foundation;
		internal char direction;
		internal string mnemonic;

		internal PictureBox board;
		internal Bitmap bmp = null;
		internal Graphics g = null;

		public Signal(PictureBox pb, string id, int x, int y, char directionPointingIn) {
			board = pb;
			direction = directionPointingIn;
			mnemonic = id;

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

		internal Tuple<Point, Point> Line(Point point1, Point point2) {
			return new Tuple<Point, Point>(point1, point2);
		}
	}

	class DwarfSignal : Signal {
		internal RectangleF head;
		internal Rectangle[] lights;	// 0 = top light, 1 = middle light, 2 = bottom light

		public DwarfSignal(PictureBox pb, string id, int x, int y, char directionPointingIn):base(pb, id, x, y, directionPointingIn) {
			int topLeftX = x + (direction == 'W' ? -20 : 5);
			int topLeftY = y - 2;
			head = new RectangleF(topLeftX, topLeftY, 16, 6);
			lights = new Rectangle[3];
			if (direction == 'W') {
				lights[0] = new Rectangle(topLeftX + 1, topLeftY + 1, 3, 3);
				lights[1] = new Rectangle(topLeftX + 6, topLeftY + 1, 3, 3);
				lights[2] = new Rectangle(topLeftX + 11, topLeftY + 1, 3, 3);
			} else if (direction == 'E') {
				lights[0] = new Rectangle(topLeftX + 11, topLeftY + 1, 3, 3);
				lights[1] = new Rectangle(topLeftX + 6, topLeftY + 1, 3, 3);
				lights[2] = new Rectangle(topLeftX + 1, topLeftY + 1, 3, 3);
			}
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
		internal Tuple<RectangleF, Rectangle[]>[] heads;    // Item1 = target, Item2 = lights
															// lowest number is top-most head/light and highest number is bottom-most head/light

		private int GetTopLeftX(int totalLights, int hW) {
			int subtractBy = (6 - totalLights) * 6;
			return direction == 'W' ? -49 + subtractBy : 28 - subtractBy + (22 - hW);
		}

		private int GetSignalWidth(int lights) {
			int subtractBy = (3 - lights) * 6;
			return 22 - subtractBy;
		}

		/// <summary>
		/// Creates a Full two head mast signal to display on the board
		/// </summary>
		/// <param name="pb"></param>
		/// <param name="id"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="directionPointingIn"></param>
		/// <param name="configuration">Signal configuration to display from top-head to bottom head.
		/// 3-2 will have a three light head on top and a two light head on the bottom.</param>
		public TwoHeadSignal(PictureBox pb, string id, int x, int y, char directionPointingIn, string configuration):base(pb, id, x, y, directionPointingIn) {
			string[] temp = configuration.Split('-');
			int[] config = new int[] { int.Parse(temp[0]), int.Parse(temp[1]) };
			int totalLights = config[0] + config[1];
			heads = new Tuple<RectangleF, Rectangle[]>[2];
			int topLeftX = x + GetTopLeftX(totalLights, GetSignalWidth(config[0]));
			int topLeftY = y - 5;
			for (int i = 0; i < 2; i++) {
				Rectangle[] lights = new Rectangle[config[i]];
				lights[0] = new Rectangle(topLeftX + 4, topLeftY + 4, 3, 3);
				if (lights.Length >= 2) {
					lights[1] = new Rectangle(topLeftX + 9, topLeftY + 4, 3, 3);
				}
				if (lights.Length == 3) {
					lights[2] = new Rectangle(topLeftX + 14, topLeftY + 4, 3, 3);
				}
				int width = GetSignalWidth(config[i]);
				heads[i] = new Tuple<RectangleF, Rectangle[]>(new RectangleF(topLeftX, topLeftY, width, 12), lights);
				if (direction == 'W') {
					topLeftX += width + 1;
				} else {
					if (i != config.Length - 1) {
						topLeftX -= GetSignalWidth(config[i + 1]) + 1;
					}
				}
			}
		}

		public override void PaintSignal() {
			base.PaintSignal();
			SolidBrush headBrush = new SolidBrush(Color.DarkGray);
			SolidBrush blackBrush = new SolidBrush(Color.Black);
			Pen blackPen = new Pen(Color.Black);

			foreach (Tuple<RectangleF, Rectangle[]> head in heads) {
				Point targetsTopCorner = new Point((int) head.Item1.X, (int) head.Item1.Y);
				g.FillRectangle(headBrush, head.Item1);
				Rectangle[] lights = head.Item2;
				RectangleF[] corners = new RectangleF[10];
				corners[0] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y     , 2, 4);
				corners[1] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y     , 4, 2);
				corners[2] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y +  8, 2, 4);
				corners[3] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y + 10, 4, 2);
				int subtractBy = (3 - lights.Length) * 6;
				corners[4] = new RectangleF(targetsTopCorner.X + 18 - subtractBy, targetsTopCorner.Y + 10, 4, 2);
				corners[5] = new RectangleF(targetsTopCorner.X + 20 - subtractBy, targetsTopCorner.Y +  8, 2, 4);
				corners[6] = new RectangleF(targetsTopCorner.X + 20 - subtractBy, targetsTopCorner.Y     , 2, 4);
				corners[7] = new RectangleF(targetsTopCorner.X + 18 - subtractBy, targetsTopCorner.Y     , 4, 2);
				corners[8] = new RectangleF(targetsTopCorner.X + (direction == 'W' ? 3 : 18), targetsTopCorner.Y + 3, 1, 1);	// this is how you draw 1 pixel for the hood
				corners[9] = new RectangleF(targetsTopCorner.X + (direction == 'W' ? 3 : 18), targetsTopCorner.Y + 8, 1, 1);
				foreach (RectangleF rectangle in corners) {
					g.FillRectangle(blackBrush, rectangle);
				}

				int cornerPosition = direction == 'W' ? GetSignalWidth(lights.Length - 1) : GetSignalWidth(lights.Length - 1) + 1;
				Tuple<Point, Point>[] hoodLines = new Tuple<Point, Point>[] {
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  4 :  5), targetsTopCorner.Y + 2),
						 new Point(targetsTopCorner.X + cornerPosition, targetsTopCorner.Y + 2)),
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  2 : 19), targetsTopCorner.Y + 4),
						 new Point(targetsTopCorner.X + (direction == 'W' ?  2 : 19), targetsTopCorner.Y + 7)),
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  4 :  5), targetsTopCorner.Y + 9),
						 new Point(targetsTopCorner.X + cornerPosition, targetsTopCorner.Y + 9))
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
			base.PaintSignal();
		}
	
	}

	class ThreeHeadSignal : Signal {
		internal Tuple<RectangleF, Rectangle[]>[] heads;    // Item1 = target, Item2 = lights
															// lowest number is top-most head/light and highest number is bottom-most head/light

		private int GetTopLeftX(int totalLights, int hW) {
			int subtractBy = (9 - totalLights) * 6;
			return direction == 'W' ? -72 + subtractBy : 51 - subtractBy + (22 - hW);
		}

		private int GetSignalWidth(int lights) {
			int subtractBy = (3 - lights) * 6;
			return 22 - subtractBy;
		}

		/// <summary>
		/// Creates a Full two head mast signal to display on the board
		/// </summary>
		/// <param name="pb"></param>
		/// <param name="id"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="directionPointingIn"></param>
		/// <param name="configuration">Signal configuration to display from top-head to bottom head.
		public ThreeHeadSignal(PictureBox pb, string id, int x, int y, char directionPointingIn, string configuration) : base(pb, id, x, y, directionPointingIn) {
			string[] temp = configuration.Split('-');
			int[] config = new int[temp.Length];
			int totalLights = 0;
			for (int i = 0; i < temp.Length; i++) {
				config[i] = int.Parse(temp[i]);
				totalLights += config[i];
			}
			int topLeftX = x + GetTopLeftX(totalLights, GetSignalWidth(config[0]));
			int topLeftY = y - 5;
			heads = new Tuple<RectangleF, Rectangle[]>[config.Length];
			for (int i = 0; i < config.Length; i++) {
				Rectangle[] lights = new Rectangle[config[i]];
				if (direction == 'W') {
					lights[0] = new Rectangle(topLeftX + 4, topLeftY + 4, 3, 3);
					if (lights.Length >= 2) {
						lights[1] = new Rectangle(topLeftX + 9, topLeftY + 4, 3, 3);
					}
					if (lights.Length == 3) {
						lights[2] = new Rectangle(topLeftX + 14, topLeftY + 4, 3, 3);
					}
				} else {
					int w = GetSignalWidth(lights.Length);
					lights[0] = new Rectangle(topLeftX + w - 8, topLeftY + 4, 3, 3);
					if (lights.Length >= 2) {
						lights[1] = new Rectangle(topLeftX + w - 13, topLeftY + 4, 3, 3);
					}
					if (lights.Length == 3) {
						lights[2] = new Rectangle(topLeftX + w - 18, topLeftY + 4, 3, 3);
					}
				}
				int width = GetSignalWidth(config[i]);
				heads[i] = new Tuple<RectangleF, Rectangle[]>(new RectangleF(topLeftX, topLeftY, width, 12), lights);
				if (direction == 'W') {
					topLeftX += width + 1;
				} else {
					if (i != config.Length - 1) {
						topLeftX -= GetSignalWidth(config[i + 1]) + 1;
					}
				}
			}
		}

		public override void PaintSignal() {
			base.PaintSignal();
			SolidBrush headBrush = new SolidBrush(Color.DarkGray);
			SolidBrush blackBrush = new SolidBrush(Color.Black);
			Pen blackPen = new Pen(Color.Black);

			foreach (Tuple<RectangleF, Rectangle[]> head in heads) {
				Point targetsTopCorner = new Point((int) head.Item1.X, (int) head.Item1.Y);
				g.FillRectangle(headBrush, head.Item1);
				Rectangle[] lights = head.Item2;
				RectangleF[] corners = new RectangleF[10];
				corners[0] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y, 2, 4);
				corners[1] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y, 4, 2);
				corners[2] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y + 8, 2, 4);
				corners[3] = new RectangleF(targetsTopCorner.X, targetsTopCorner.Y + 10, 4, 2);
				int subtractBy = (3 - lights.Length) * 6;
				corners[4] = new RectangleF(targetsTopCorner.X + 18 - subtractBy, targetsTopCorner.Y + 10, 4, 2);
				corners[5] = new RectangleF(targetsTopCorner.X + 20 - subtractBy, targetsTopCorner.Y + 8, 2, 4);
				corners[6] = new RectangleF(targetsTopCorner.X + 20 - subtractBy, targetsTopCorner.Y, 2, 4);
				corners[7] = new RectangleF(targetsTopCorner.X + 18 - subtractBy, targetsTopCorner.Y, 4, 2);
				corners[8] = new RectangleF(targetsTopCorner.X + (direction == 'W' ? 3 : 18 - subtractBy), targetsTopCorner.Y + 3, 1, 1);    // this is how you draw 1 pixel for the hood
				corners[9] = new RectangleF(targetsTopCorner.X + (direction == 'W' ? 3 : 18 - subtractBy), targetsTopCorner.Y + 8, 1, 1);
				foreach (RectangleF rectangle in corners) {
					g.FillRectangle(blackBrush, rectangle);
				}

				if (lights.Length == 1) {
					_ = 1;
				}
				int cornerPosition = direction == 'W' ? GetSignalWidth(lights.Length - 1) : GetSignalWidth(lights.Length - 1) + 1;
				Tuple<Point, Point>[] hoodLines = new Tuple<Point, Point>[] {
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  4 :  5), targetsTopCorner.Y + 2),
						 new Point(targetsTopCorner.X + cornerPosition, targetsTopCorner.Y + 2)),
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  2 : 19 - subtractBy), targetsTopCorner.Y + 4),
						 new Point(targetsTopCorner.X + (direction == 'W' ?  2 : 19 - subtractBy), targetsTopCorner.Y + 7)),
					Line(new Point(targetsTopCorner.X + (direction == 'W' ?  4 :  5), targetsTopCorner.Y + 9),
						 new Point(targetsTopCorner.X + cornerPosition, targetsTopCorner.Y + 9))
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
			base.PaintSignal();
		}
	}
}

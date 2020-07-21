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
			// TODO: Modify to work with east pointing signals
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
}

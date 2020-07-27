using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	public partial class MainWindow : Form {
		public MainWindow() {
			InitializeComponent();
			Bitmap newBmp = new Bitmap(board.Width, board.Height);
			Graphics newG = Graphics.FromImage(newBmp);
			SolidBrush newBrush = new SolidBrush(Color.Black);
			newG.FillRectangle(newBrush, new RectangleF(0, 0, board.Width, board.Height));
			newBrush.Dispose();
			board.Image = newBmp;
			newG.Dispose();
		}

		public PictureBox GetBoard() {
			return board;
		}

		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
			Program.CloseThread();
		}
	}
}

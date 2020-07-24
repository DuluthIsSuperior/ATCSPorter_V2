using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	public partial class MainWindow : Form {
		PictureBox boardBox;
		Dictionary<string, Block> blockDrawings = new Dictionary<string, Block>();
		Dictionary<string, Signal> signalDrawings = new Dictionary<string, Signal>();	// key is the NS signal number

		public MainWindow() {
			InitializeComponent();
			boardBox = (PictureBox) Controls.Find("board", true)[0];

			Block block = new ArrowHead(6, 82, "W");
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("485_3T", block);

			block = new LinearBlock(6, 82, 128);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("485_1WA", block);

			block = new LinearBlock(136, 82, 60);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_1EA", block);

			List<object> backdrop = new List<object> {
				new DiagonalBlock(323, 96, '/'),
				new LinearBlock(198, 82, 256),
				new DiagonalBlock(387, 64, '/'),
				new DiagonalBlock(403, 48, '/'),
				new DiagonalBlock(419, 32, '/'),
				new Turnout(429, 22, "/-"),
				new RectangleF(433, 22, 22, 4),
				new Line(455, 18, 455, 29)
			};

			Dictionary<string, List<object>> blocks = new Dictionary<string, List<object>>();
			blocks.Add("2N 3N", new List<object> { new LinearBlock(198, 82, 256) });
			blocks.Add("2R 3N", new List<object> {
				new DiagonalBlock(323, 96, '/'),
				new Turnout(333, 86, "/-"),
				new RectangleF(337, 86, 118, 4),
				new Line(455, 82, 455, 93)
			});
			blocks.Add("2R 3R", new List<object> {
				new Turnout(333, 86, "/-"),
				new RectangleF(337, 86, 28, 4),
				new Turnout(365, 86, "-/"),
				new DiagonalBlock(387, 64, '/'),
				new DiagonalBlock(403, 48, '/'),
				new DiagonalBlock(419, 32, '/'),
				new Turnout(429, 22, "/-"),
				new RectangleF(433, 22, 22, 4),
				new Line(455, 18, 455, 29)
			});
			block = new BlockConfiguration(boardBox, blocks, backdrop);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("482_1T", block);

			block = new LinearBlock(456, 18, 126);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_CT", block);

			blocks = new Dictionary<string, List<object>>();
			blocks.Add("N", new List<object> {
				new Line(584, 18, 584, 29),
				new RectangleF(585, 22, 26, 4),
				new Turnout(607, 22, "-/"),
				new ArrowHead(613, 13, "NE")
			});
			block = new BlockConfiguration(boardBox, blocks, blocks["N"]);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("482_6WA", block);

			backdrop = new List<object> {
				new LinearBlock(456, 82, 62),
				new DiagonalBlock(495, 64, '/'),
				new Turnout(499, 60, "-/"),
				new RectangleF(509, 54, 26, 4),
				new Line(535, 50, 535, 61)
			};
			blocks = new Dictionary<string, List<object>>();
			blocks.Add("5N", new List<object> { new LinearBlock(456, 82, 62) });
			blocks.Add("5R", new List<object> {
				new Line(456, 82, 456, 93),
				new RectangleF(456, 86, 18, 4),
				new Turnout(474, 86, "-/"),
				new DiagonalBlock(495, 64, '/'),
				new Turnout(505, 54, "/-"),
				new RectangleF(509, 54, 26, 4),
				new Line(535, 50, 535, 61)
			});
			block = new BlockConfiguration(boardBox, blocks, backdrop);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("482_BT", block);

			block = new LinearBlock(536, 50, 46);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_DT", block);

			blocks = new Dictionary<string, List<object>>();
			blocks.Add("N", new List<object> {
				new Line(584, 50, 584, 61),
				new RectangleF(585, 54, 26, 4),
				new Turnout(607, 54, "-/"),
				new ArrowHead(613, 45, "NE")

			});
			block = new BlockConfiguration(boardBox, blocks, blocks["N"]);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("482_7WA", block);

			backdrop = new List<object> { 
				new LinearBlock(520, 82, 174),
				new DiagonalBlock(551, 96, '\\'),
				new DiagonalBlock(659, 96, '/')
			};
			blocks = new Dictionary<string, List<object>>();
			blocks.Add("7N 9N", new List<object> {
				new LinearBlock(520, 82, 174)
			});
			blocks.Add("7N 9R", new List<object> {
				new DiagonalBlock(659, 96, '/'),
				new Turnout(669, 86, "/-"),
				new RectangleF(673, 86, 22, 4),
				new Line(695, 82, 695, 93)
			});
			blocks.Add("7R 9N", new List<object> {
				new Line(520, 82, 520, 93),
				new RectangleF(521, 86, 20, 4),
				new Turnout(541, 86, "-\\"),
				new DiagonalBlock(551, 96, '\\')
			});

			block = new BlockConfiguration(boardBox, blocks, backdrop);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("482_2T", block);

			block = new LinearBlock(696, 82, 62);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_AO", block);

			backdrop = new List<object> { 
				new LinearBlock(760, 82, 126),
				new DiagonalBlock(803, 96, '/'),
				new DiagonalBlock(839, 96, '\\')
			};
			blocks = new Dictionary<string, List<object>>();
			blocks.Add("1N 2N", new List<object> {
				new LinearBlock(760, 82, 126)
			});
			blocks.Add("1R 2N", new List<object> {
				new DiagonalBlock(803, 96, '/'),
				new Turnout(813, 86, "/-"),
				new RectangleF(814, 86, 73, 4),
				new Line(887, 82, 887, 93)
			});
			blocks.Add("1N 2R", new List<object> {
				new Line(760, 82, 760, 93),
				new RectangleF(761, 86, 68, 4),
				new Turnout(829, 86, "-\\"),
				new DiagonalBlock(839, 96, '\\')
			});

			block = new BlockConfiguration(boardBox, blocks, backdrop);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("479_1T", block);

			block = new LinearBlock(888, 82, 46);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("479_1WA", block);

			block = new ArrowHead(936, 81, "E");
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("479_AWA", block);

			block = new ArrowHead(6, 114, "W");
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("485_4T", block);

			block = new LinearBlock(6, 114, 46);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("485_BO", block);

			backdrop = new List<object> {
				new LinearBlock(54, 114, 80),
				new DiagonalBlock(87, 128, '\\'),
				new Turnout(109, 150, "\\-"),
				new RectangleF(113, 150, 22, 4),
				new Line(135, 146, 135, 157)
				
			};
			blocks = new Dictionary<string, List<object>>();
			blocks.Add("1N", new List<object> {
				new LinearBlock(54, 114, 80)
			});
			blocks.Add("1R", new List<object> {
				new PartialLinearBlock(55, 118, 22, 'W'),
				new Turnout(77, 118, "-\\"),
				new DiagonalBlock(87, 128, '\\'),
				new Turnout(109, 150, "\\-"),
				new PartialLinearBlock(113, 150, 22, 'E')
			});
			block = new BlockConfiguration(boardBox, blocks, backdrop);
			//block.PaintBlock(boardBox, Color.White);
			blockDrawings.Add("483_2T", block);

			block = new LinearBlock(136, 114, 60);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("483_2O", block);

			block = new LinearBlock(136, 146, 60);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("483_3O", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(198, 114, 286);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_3T", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(486, 114, 208);
			block.PaintBlock(boardBox, Color.Red);
			blockDrawings.Add("482_4T", block);

			block = new LinearBlock(696, 114, 62);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("482_BO", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(760, 114, 126);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_2T", block);

			block = new LinearBlock(888, 114, 46);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_2WA", block);

			block = new ArrowHead(936, 113, "E");
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_BWA", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(198, 146, 142);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_5T", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(342, 146, 144);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_FT", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(488, 146, 206);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_GT", block);

			block = new PartialLinearBlock(697, 150, 32, 'W');
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_5WA", block);

			// TODO: Modify to include arrow and diagonal line
			block = new PartialLinearBlock(177, 182, 20, 'E');
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_5EA", block);

			// TODO: Modify with turnouts
			block = new LinearBlock(198, 178, 80);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_6T", block);

			block = new LinearBlock(280, 178, 60);
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_ET", block);

			// TODO: Modify to include arrow and diagonal line
			block = new PartialLinearBlock(177, 214, 20, 'E');
			block.PaintBlock(board, Color.Red);
			blockDrawings.Add("479_6EA", block);

			Signal signal = new DwarfSignal(board, "2WG", 134, 135, 'W');
			signal.PaintSignal();
			signalDrawings.Add("483_4WD", signal);

			signal = new DwarfSignal(board, "5EG", 199, 167, 'E');
			signal.PaintSignal();
			signalDrawings.Add("482_10E", signal);

			signal = new TwoHeadSignal(board, "2WG", 134, 103, 'W', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("483_4WA", signal);

			signal = new TwoHeadSignal(board, "1EI", 138, 103, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("483_1EI", signal);

			signal = new TwoHeadSignal(board, "2EG", 759, 135, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_4E", signal);

			signal = new TwoHeadSignal(board, "1EG", 759, 71, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_2E", signal);

			signal = new TwoHeadSignal(board, "1WI", 134, 73, 'W', "3-2");
			signal.PaintSignal();
			signalDrawings.Add("483_1WI", signal);

			signal = new TwoHeadSignal(board, "6WG", 694, 135, 'W', "2-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12W", signal);

			signal = new TwoHeadSignal(board, "1WG", 582, 39, 'W', "1-3");
			signal.PaintSignal();
			signalDrawings.Add("482_2WD", signal);

			signal = new ThreeHeadSignal(board, "2WG", 694, 71, 'W', "3-3-3");	// 3-3-3
			signal.PaintSignal();
			signalDrawings.Add("482_4W", signal);

			signal = new ThreeHeadSignal(board, "4WG", 694, 103, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_8W", signal);

			signal = new ThreeHeadSignal(board, "1EG", 199, 103, 'E', "3-3-3");	// 3-3-3
			signal.PaintSignal();
			signalDrawings.Add("482_2E", signal);

			signal = new ThreeHeadSignal(board, "3EG", 199, 135, 'E', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_6E", signal);

			signal = new ThreeHeadSignal(board, "1WG", 886, 71, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_2W", signal);

			signal = new ThreeHeadSignal(board, "2WG", 886, 135, 'W', "3-3-3"); // 3-3-3
			signal.PaintSignal();
			signalDrawings.Add("479_4W", signal);

			signal = new ThreeHeadSignal(board, "2EG", 55, 167, 'E', "3-3-2");
			signal.PaintSignal();
			signalDrawings.Add("483_4E", signal);

			signal = new ThreeHeadSignal(board, "6EG", 199, 199, 'E', "1-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12EA", signal);

			signal = new ThreeHeadSignal(board, "6EG", 199, 231, 'E', "1-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12ED", signal);

			signal = new ThreeHeadSignal(board, "1WG", 582, 7, 'W', "1-1-3");
			signal.PaintSignal();
			signalDrawings.Add("482_2WA", signal);
		}
	}
}

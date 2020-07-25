using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	public partial class MainWindow : Form {
		readonly Dictionary<string,  Block> blockDrawings  = new Dictionary<string,  Block>();
		readonly Dictionary<string, Signal> signalDrawings = new Dictionary<string, Signal>();	// key is the NS signal number

		public MainWindow() {
			InitializeComponent();
			Bitmap newBmp = new Bitmap(board.Width, board.Height);
			Graphics newG = Graphics.FromImage(newBmp);
			SolidBrush newBrush = new SolidBrush(Color.Black);
			newG.FillRectangle(newBrush, new RectangleF(0, 0, board.Width, board.Height));
			newBrush.Dispose();
			board.Image = newBmp;
			newG.Dispose();

			Block block = new ArrowHead(board, 6, 82, "W");
			block.PaintBlock(Color.White);
			blockDrawings.Add("485_3T", block);

			block = new LinearBlock(board, 6, 82, 128);
			block.PaintBlock(Color.White);
			blockDrawings.Add("485_1WA", block);

			block = new LinearBlock(board, 136, 82, 60);
			block.PaintBlock(Color.White);
			blockDrawings.Add("482_1EA", block);

			List<Block> backdrop = new List<Block> {
				new DiagonalBlock(board, 323, 96, '/'),
				new LinearBlock(board, 198, 82, 256),
				new DiagonalBlock(board, 387, 64, '/'),
				new DiagonalBlock(board, 403, 48, '/'),
				new DiagonalBlock(board, 419, 32, '/'),
				new Turnout(board, 429, 22, "/-"),
				new PartialLinearBlock(board, 433, 22, 22, 'E')
			};
			Dictionary<string, List<Block>> blocks = new Dictionary<string, List<Block>> {
				{ "2N 3N", new List<Block> { new LinearBlock(board, 198, 82, 256) } },
				{ "2R 3N",
					new List<Block> {
						new DiagonalBlock(board, 323, 96, '/'),
						new Turnout(board, 333, 86, "/-"),
						new PartialLinearBlock(board, 337, 86, 118, 'E')
					}
				}, { "2R 3R",
					new List<Block> {
						new Turnout(board, 333, 86, "/-"),
						new PartialLinearBlock(board, 337, 86, 28, '0'),
						new Turnout(board, 365, 86, "-/"),
						new DiagonalBlock(board, 387, 64, '/'),
						new DiagonalBlock(board, 403, 48, '/'),
						new DiagonalBlock(board, 419, 32, '/'),
						new Turnout(board, 429, 22, "/-"),
						new PartialLinearBlock(board, 433, 22, 22, 'E')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_1T");
			blockDrawings.Add("482_1T", block);

			block = new LinearBlock(board, 456, 18, 126);
			block.PaintBlock(Color.White);
			blockDrawings.Add("482_CT", block);

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 585, 22, 26, 'W'),
						new Turnout(board, 607, 22, "-/"),
						new ArrowHead(board, 613, 13, "NE")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, blocks["N"], "482_6WA");
			blockDrawings.Add("482_6WA", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 456, 82, 62),
				new DiagonalBlock(board, 495, 64, '/'),
				new Turnout(board, 499, 60, "-/"),
				new PartialLinearBlock(board, 509, 54, 26, 'E')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "5N", new List<Block> { new LinearBlock(board, 456, 82, 62) } },
				{ "5R",
					new List<Block> {
						new PartialLinearBlock(board, 456, 86, 18, 'W'),
						new Turnout(board, 474, 86, "-/"),
						new DiagonalBlock(board, 495, 64, '/'),
						new Turnout(board, 505, 54, "/-"),
						new PartialLinearBlock(board, 509, 54, 26, 'E')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_BT");
			blockDrawings.Add("482_BT", block);

			block = new LinearBlock(board, 536, 50, 46);
			block.PaintBlock(Color.White);
			blockDrawings.Add("482_DT", block);

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 585, 54, 26, 'W'),
						new Turnout(board, 607, 54, "-/"),
						new ArrowHead(board, 613, 45, "NE")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, blocks["N"], "482_7WA");
			blockDrawings.Add("482_7WA", block);

			backdrop = new List<Block> { 
				new LinearBlock(board, 520, 82, 174),
				new DiagonalBlock(board, 551, 96, '\\'),
				new DiagonalBlock(board, 659, 96, '/')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "7N 9N", new List<Block> { new LinearBlock(board, 520, 82, 174) } },
				{ "7N 9R",
					new List<Block> {
						new DiagonalBlock(board, 659, 96, '/'),
						new Turnout(board, 669, 86, "/-"),
						new PartialLinearBlock(board, 673, 86, 22, 'E')
					}
				}, { "7R 9N",
					new List<Block> {
						new PartialLinearBlock(board, 521, 86, 20, 'W'),
						new Turnout(board, 541, 86, "-\\"),
						new DiagonalBlock(board, 551, 96, '\\')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_2T");
			blockDrawings.Add("482_2T", block);

			block = new LinearBlock(board, 696, 82, 62);
			block.PaintBlock(Color.White);
			blockDrawings.Add("482_AO", block);

			backdrop = new List<Block> { 
				new LinearBlock(board, 760, 82, 126),
				new DiagonalBlock(board, 803, 96, '/'),
				new DiagonalBlock(board, 839, 96, '\\')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 760, 82, 126) } },
				{ "1R 2N",
					new List<Block> {
						new DiagonalBlock(board, 803, 96, '/'),
						new Turnout(board, 813, 86, "/-"),
						new PartialLinearBlock(board, 814, 86, 73, 'E')
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 761, 86, 68, 'W'),
						new Turnout(board, 829, 86, "-\\"),
						new DiagonalBlock(board, 839, 96, '\\')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "479_1T");
			blockDrawings.Add("479_1T", block);

			block = new LinearBlock(board, 888, 82, 46);
			block.PaintBlock(Color.White);
			blockDrawings.Add("479_1WA", block);

			block = new ArrowHead(board, 936, 81, "E");
			block.PaintBlock(Color.White);
			blockDrawings.Add("479_AWA", block);

			block = new ArrowHead(board, 6, 114, "W");
			block.PaintBlock(Color.White);
			blockDrawings.Add("485_4T", block);

			//
			block = new LinearBlock(board, 6, 114, 46);
			block.PaintBlock(Color.White);
			blockDrawings.Add("483_BO", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 54, 114, 80),
				new DiagonalBlock(board, 97, 128, '\\'),
				new Turnout(board, 119, 150, "\\-"),
				new PartialLinearBlock(board, 123, 150, 12, 'E')

			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N", new List<Block> { new LinearBlock(board, 54, 114, 80) } },
				{ "1R",
					new List<Block> {
						new PartialLinearBlock(board, 55, 118, 22, 'W'),
						new Turnout(board, 77, 118, "-\\"),
						new DiagonalBlock(board, 87, 128, '\\'),
						new Turnout(board, 109, 150, "\\-"),
						new PartialLinearBlock(board, 113, 150, 22, 'E')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "483_2T");
			blockDrawings.Add("483_2T", block);
			//

			block = new LinearBlock(board, 136, 114, 60);
			block.PaintBlock(Color.White);
			blockDrawings.Add("483_2O", block);

			block = new LinearBlock(board, 136, 146, 60);
			block.PaintBlock(Color.White);
			blockDrawings.Add("483_3O", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 114, 286),
				new DiagonalBlock(board, 259, 128, '/')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 198, 114, 286) } },
				{ "1R 2N",
					new List<Block> {
						new PartialLinearBlock(board, 269, 118, 216, 'E'),
						new Turnout(board, 269, 118, "/-"),
						new DiagonalBlock(board, 259, 128, '/')
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 118, 102, 'W'),
						new Turnout(board, 301, 118, "-/")
					}
				}, { "1R 2R",
					new List<Block> {
						new DiagonalBlock(board, 259, 128, '/'),
						new Turnout(board, 269, 118, "/-"),
						new PartialLinearBlock(board, 270, 118, 32, '0'),
						new Turnout(board, 301, 118, "-/")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_3T");
			blockDrawings.Add("482_3T", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 486, 114, 208),
				new DiagonalBlock(board, 595, 128, '/')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "7N 8N 9N", new List<Block> { new LinearBlock(board, 486, 114, 208) } },
				{ "7R 8N 9N",
					new List<Block> {
						new Turnout(board, 573, 118, "\\-"),
						new PartialLinearBlock(board, 577, 118, 118, 'E')
					}
				}, { "7N 8R 9N",
					new List<Block> {
						new Turnout(board, 605, 118, "/-"),
						new PartialLinearBlock(board, 605, 118, 90, 'E'),
						new DiagonalBlock(board, 595, 128, '/')
					}
				}, { "7N 8N 9R",
					new List<Block> {
						new PartialLinearBlock(board, 487, 118, 150, 'W'),
						new Turnout(board, 637, 118, "-/")
					}
				}, { "7N 8R 9R",
					new List<Block> {
						new Turnout(board, 605, 118, "/-"),
						new PartialLinearBlock(board, 606, 118, 32, '0'),
						new Turnout(board, 637, 118, "-/"),
						new DiagonalBlock(board, 595, 128, '/')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_4T");
			blockDrawings.Add("482_4T", block);

			block = new LinearBlock(board, 696, 114, 62);
			block.PaintBlock(Color.White);
			blockDrawings.Add("482_BO", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 760, 114, 126)
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 760, 114, 126) } },
				{ "1R 2N",
					new List<Block> {
						new PartialLinearBlock(board, 761, 118, 20, 'W'),
						new Turnout(board, 781, 118, "-/")
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 864, 118, 23, 'E'),
						new Turnout(board, 861, 118, "\\-")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "479_2T");
			blockDrawings.Add("479_2T", block);

			block = new LinearBlock(board, 888, 114, 46);
			block.PaintBlock(Color.White);
			blockDrawings.Add("479_2WA", block);

			block = new ArrowHead(board, 936, 113, "E");
			block.PaintBlock(Color.White);
			blockDrawings.Add("479_BWA", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 146, 142)
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N", new List<Block> { new LinearBlock(board, 198, 146, 142) } },
				{ "1R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 150, 38, 'W'),
						new Turnout(board, 237, 150, "-/")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_5T");
			blockDrawings.Add("482_5T", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 342, 146, 144),
				new PartialLinearBlock(board, 343, 182, 70, 'W'),
				new Turnout(board, 413, 182, "-/"),
				new DiagonalBlock(board, 435, 160, '/')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "6N", new List<Block> { new LinearBlock(board, 342, 146, 144) } },
				{ "6R",
					new List<Block> {
						new PartialLinearBlock(board, 343, 182, 70, 'W'),
						new Turnout(board, 413, 182, "-/"),
						new DiagonalBlock(board, 435, 160, '/'),
						new Turnout(board, 445, 150, "/-"),
						new PartialLinearBlock(board, 446, 150, 41, 'E')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_FT");
			blockDrawings.Add("482_FT", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 488, 146, 206)
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "8N", new List<Block> { new LinearBlock(board, 488, 146, 206) } },
				{ "8R",
					new List<Block> {
						new PartialLinearBlock(board, 489, 150, 86, 'W'),
						new Turnout(board, 573, 150, "-/")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_GT");
			blockDrawings.Add("482_GT", block);

			block = new PartialLinearBlock(board, 697, 150, 32, 'W');
			blockDrawings.Add("479_5WA", block);

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 177, 182, 20, 'E'),
						new Turnout(board, 173, 182, "/-"),
						new ArrowHead(board, 164, 188, "SW")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, blocks["N"], "482_5EA");
			blockDrawings.Add("482_5EA", block);

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 178, 80),
				new PartialLinearBlock(board, 199, 214, 22, 'W'),
				new Turnout(board, 221, 214, "-/"),
				new DiagonalBlock(board, 243, 192, '/')
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "4N", new List<Block> { new LinearBlock(board, 198, 178, 80) } },
				{ "4R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 214, 22, 'W'),
						new Turnout(board, 221, 214, "-/"),
						new DiagonalBlock(board, 243, 192, '/'),
						new Turnout(board, 253, 182, "/-"),
						new PartialLinearBlock(board, 255, 182, 24, 'E')
					}
				}
			};
			block = new BlockConfiguration(board, blocks, backdrop, "482_6T");
			blockDrawings.Add("482_6T", block);

			block = new LinearBlock(board, 280, 178, 60);
			block.PaintBlock(Color.White);
			blockDrawings.Add("479_ET", block);

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 177, 214, 20, 'E'),
						new Turnout(board, 173, 214, "/-"),
						new ArrowHead(board, 164, 220, "SW")
					}
				}
			};
			block = new BlockConfiguration(board, blocks, blocks["N"], "482_6EA");
			blockDrawings.Add("482_6EA", block);

			Signal signal = new DwarfSignal(board, "2WG", 134, 135, 'W');
			signal.PaintSignal();
			signalDrawings.Add("483_4WD", signal);

			signal = new DwarfSignal(board, "5EG", 199, 167, 'E');
			signal.PaintSignal();
			signalDrawings.Add("482_10E", signal);

			signal = new MastSignal(board, "2WG", 134, 103, 'W', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("483_4WA", signal);

			signal = new MastSignal(board, "1EI", 138, 103, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("483_1EI", signal);

			signal = new MastSignal(board, "2EG", 759, 135, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_4E", signal);

			signal = new MastSignal(board, "1EG", 759, 71, 'E', "3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_2E", signal);

			signal = new MastSignal(board, "1WI", 134, 73, 'W', "3-2");
			signal.PaintSignal();
			signalDrawings.Add("483_1WI", signal);

			signal = new MastSignal(board, "6WG", 694, 135, 'W', "2-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12W", signal);

			signal = new MastSignal(board, "1WG", 582, 39, 'W', "1-3");
			signal.PaintSignal();
			signalDrawings.Add("482_2WD", signal);

			signal = new MastSignal(board, "2WG", 694, 71, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_4W", signal);

			signal = new MastSignal(board, "4WG", 694, 103, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_8W", signal);

			signal = new MastSignal(board, "1EG", 199, 103, 'E', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_2E", signal);

			signal = new MastSignal(board, "3EG", 199, 135, 'E', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_6E", signal);

			signal = new MastSignal(board, "1WG", 886, 71, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_2W", signal);

			signal = new MastSignal(board, "2WG", 886, 135, 'W', "3-3-3");
			signal.PaintSignal();
			signalDrawings.Add("479_4W", signal);

			signal = new MastSignal(board, "2EG", 25, 135, 'E', "3-3-2"); // 55, 167
			signal.PaintSignal();
			signalDrawings.Add("483_4E", signal);

			signal = new MastSignal(board, "6EG", 199, 199, 'E', "1-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12EA", signal);

			signal = new MastSignal(board, "6EG", 199, 231, 'E', "1-3-3");
			signal.PaintSignal();
			signalDrawings.Add("482_12ED", signal);

			signal = new MastSignal(board, "1WG", 582, 7, 'W', "1-1-3");
			signal.PaintSignal();
			signalDrawings.Add("482_2WA", signal);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ATCSPorter_V2 {
	// CP 466 - 75501590130202
	// CP 479 - 75501590160202
	// CP 482 - 75501580120202
	// CP 483 - 75501580110202
	// CP 485 - 75501580090202
	// CP 487 - 75501580180202

	public class MutexObject<T> {
		public Mutex mutex;
		T value;

		public MutexObject(T initialValue) {
			value = initialValue;
			mutex = new Mutex();
		}

		public void SetValue(T newValue) {
			mutex.WaitOne();
			value = newValue;
			mutex.ReleaseMutex();
		}

		public T GetValue() {
			mutex.WaitOne();
			T temp = value;
			mutex.ReleaseMutex();
			return temp;
		}
	}

	static class Program {
		static MainWindow window;       
		static readonly MutexObject<bool> isClosing = new MutexObject<bool>(false); // this class and its members can be static since there should only be one instance of this program running
		static readonly Dictionary<int, ControlPoint> cp = new Dictionary<int, ControlPoint>();

		public static void CloseThread() {
			isClosing.SetValue(true);
		}

		private static string[] ReadAndClearFile(string path) {
			string[] lines = File.ReadAllLines(path);
			File.WriteAllText(path, "");
			return lines;
		}

		private static readonly Thread logicThread = new Thread(() => {
			while (!isClosing.GetValue()) { 
				
			}
		});

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			window = new MainWindow();
			PictureBox board = window.GetBoard();
			InitalizeCP466(board);
			InitalizeCP479(board);
			InitalizeCP482(board);
			InitalizeCP483(board);
			InitalizeCP485(board);
			InitalizeCP487(board);

			logicThread.Start();
			Application.Run(window);
		}

		static void InitalizeCP466(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();
			cp.Add(466, new ControlPoint(blockDictionary, signalDictionary));
		}
		static void InitalizeCP479(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();

			List<Block> backdrop = new List<Block> {
				new LinearBlock(board, 741, 82, 185),	// 760, , 152
				new DiagonalBlock(board, 803, 96, '/'),
				new DiagonalBlock(board, 839, 96, '\\')
			};
			Dictionary<string, List<Block>> blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 741, 82, 185) } },
				{ "1R 2N",
					new List<Block> {
						new DiagonalBlock(board, 803, 96, '/'),
						new Turnout(board, 813, 86, "/-"),
						new PartialLinearBlock(board, 814, 86, 113, 'E')
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 742, 86, 87, 'W'),	// 761, , 68
						new Turnout(board, 829, 86, "-\\"),
						new DiagonalBlock(board, 839, 96, '\\')
					}
				}
			};
			blockDictionary.Add("1T", new BlockConfiguration(board, blocks, backdrop));

			blockDictionary.Add("1WA", new LinearBlock(board, 928, 82, 46, true));
			blockDictionary.Add("AWA", new ArrowHead(board, 976, 81, "E", true));

			backdrop = new List<Block> {
				new LinearBlock(board, 741, 114, 185)	// 760, , 164
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 741, 114, 185) } },
				{ "1R 2N",
					new List<Block> {
						new PartialLinearBlock(board, 742, 118, 39, 'W'),	// 761, , 20
						new Turnout(board, 781, 118, "-/")
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 864, 118, 63, 'E'),
						new Turnout(board, 861, 118, "\\-")
					}
				}
			};
			blockDictionary.Add("2T", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("2WA", new LinearBlock(board, 928, 114, 46, true));
			blockDictionary.Add("BWA", new ArrowHead(board, 976, 113, "E", true));
			blockDictionary.Add("5WA", new PartialLinearBlock(board, 697, 150, 32, 'W', true));
			blockDictionary.Add("ET", new LinearBlock(board, 288, 178, 52, true));   // 280, , 60

			signalDictionary.Add("4E", new MastSignal(board, "2EG", 741, 135, 'E', "3-3"));    // 759
			signalDictionary.Add("2E", new MastSignal(board, "1EG", 741, 103, 'E', "3-3"));    // 759
			signalDictionary.Add("2W", new MastSignal(board, "1WG", 927, 71, 'W', "3-3-3"));   // 886
			signalDictionary.Add("4W", new MastSignal(board, "2WG", 927, 103, 'W', "3-3-3"));  // 886

			cp.Add(479, new ControlPoint(blockDictionary, signalDictionary));
		}
		static void InitalizeCP482(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();

			blockDictionary.Add("1EA", new LinearBlock(board, 136, 82, 60, true));

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
			blockDictionary.Add("1T", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("CT", new LinearBlock(board, 456, 18, 126, true));

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 585, 22, 26, 'W'),
						new Turnout(board, 607, 22, "-/"),
						new ArrowHead(board, 613, 13, "NE")
					}
				}
			};
			blockDictionary.Add("6WA", new BlockConfiguration(board, blocks, blocks["N"]));

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
			blockDictionary.Add("BT", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("DT", new LinearBlock(board, 536, 50, 46, true));

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 585, 54, 26, 'W'),
						new Turnout(board, 607, 54, "-/"),
						new ArrowHead(board, 613, 45, "NE")
					}
				}
			};
			blockDictionary.Add("7WA", new BlockConfiguration(board, blocks, blocks["N"]));

			backdrop = new List<Block> {
				new LinearBlock(board, 520, 82, 174),
				new DiagonalBlock(board, 551, 96, '\\'),
				new DiagonalBlock(board, 618, 96, '/')	// 659
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "7N 9N", new List<Block> { new LinearBlock(board, 520, 82, 174) } },
				{ "7N 9R",
					new List<Block> {
						new DiagonalBlock(board, 618, 96, '/'),
						new Turnout(board, 628, 86, "/-"),	// 669
						new PartialLinearBlock(board, 632, 86, 63, 'E')	// 673, , 22
					}
				}, { "7R 9N",
					new List<Block> {
						new PartialLinearBlock(board, 521, 86, 20, 'W'),
						new Turnout(board, 541, 86, "-\\"),
						new DiagonalBlock(board, 551, 96, '\\')
					}
				}
			};
			blockDictionary.Add("2T", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("AO", new LinearBlock(board, 696, 82, 43, true));    // ,62

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 177, 214, 20, 'E'),
						new Turnout(board, 173, 214, "/-"),
						new ArrowHead(board, 164, 220, "SW")
					}
				}
			};
			blockDictionary.Add("6EA", new BlockConfiguration(board, blocks, blocks["N"]));

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 114, 286),
				new DiagonalBlock(board, 284, 128, '/')	// 259
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N 2N", new List<Block> { new LinearBlock(board, 198, 114, 286) } },
				{ "1R 2N",
					new List<Block> {
						new PartialLinearBlock(board, 295, 118, 191, 'E'),	// 270, , 216
						new Turnout(board, 294, 118, "/-"),	// 269
						new DiagonalBlock(board, 284, 128, '/')	// 259
					}
				}, { "1N 2R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 118, 102, 'W'),
						new Turnout(board, 301, 118, "-/")
					}
				}, { "1R 2R",
					new List<Block> {
						new DiagonalBlock(board, 284, 128, '/'),	// 259,
						new Turnout(board, 294, 118, "/-"),	// 269
						new PartialLinearBlock(board, 295, 118, 7, '0'),
						new Turnout(board, 301, 118, "-/")
					}
				}
			};
			blockDictionary.Add("3T", new BlockConfiguration(board, blocks, backdrop));

			backdrop = new List<Block> {
				new LinearBlock(board, 486, 114, 208),
				new DiagonalBlock(board, 575, 128, '/')	// 595
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
						new DiagonalBlock(board, 575, 128, '/'),
						new Turnout(board, 585, 118, "/-"),	// 605
						new PartialLinearBlock(board, 585, 118, 110, 'E')	// 605, , 90
					}
				}, { "7N 8N 9R",
					new List<Block> {
						new PartialLinearBlock(board, 487, 118, 110, 'W'),
						new Turnout(board, 596, 118, "-/")	// 637
					}
				}, { "7N 8R 9R",
					new List<Block> {
						new DiagonalBlock(board, 575, 128, '/'),
						new Turnout(board, 585, 118, "/-"),
						new PartialLinearBlock(board, 585, 118, 11, '0'),	// 606, , 32
						new Turnout(board, 596, 118, "-/"),	// 637
					}
				}
			};
			blockDictionary.Add("4T", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("BO", new LinearBlock(board, 696, 114, 43, true));   // , 62

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 146, 142)
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "1N", new List<Block> { new LinearBlock(board, 198, 146, 142) } },
				{ "1R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 150, 63, 'W'),	// , 38
						new Turnout(board, 262, 150, "-/")	// 237
					}
				}
			};
			blockDictionary.Add("5T", new BlockConfiguration(board, blocks, backdrop));

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
			blockDictionary.Add("FT", new BlockConfiguration(board, blocks, backdrop));

			backdrop = new List<Block> { new LinearBlock(board, 488, 146, 206) };
			blocks = new Dictionary<string, List<Block>> {
				{ "8N", new List<Block> { new LinearBlock(board, 488, 146, 206) } },
				{ "8R",
					new List<Block> {
						new PartialLinearBlock(board, 489, 150, 86, 'W'),
						new Turnout(board, 573, 150, "-/")
					}
				}
			};
			blockDictionary.Add("GT", new BlockConfiguration(board, blocks, backdrop));

			blocks = new Dictionary<string, List<Block>> {
				{ "N",
					new List<Block> {
						new PartialLinearBlock(board, 177, 182, 20, 'E'),
						new Turnout(board, 173, 182, "/-"),
						new ArrowHead(board, 164, 188, "SW")
					}
				}
			};
			blockDictionary.Add("5EA", new BlockConfiguration(board, blocks, blocks["N"]));

			backdrop = new List<Block> {
				new LinearBlock(board, 198, 178, 88),	// ,80
				new PartialLinearBlock(board, 199, 214, 51, 'W'),	//, 22, 
				new Turnout(board, 250, 214, "-/"),	// 221, 214
				new DiagonalBlock(board, 272, 192, '/')	// 243
			};
			blocks = new Dictionary<string, List<Block>> {
				{ "4N", new List<Block> { new LinearBlock(board, 198, 178, 88) } },
				{ "4R",
					new List<Block> {
						new PartialLinearBlock(board, 199, 214, 51, 'W'),	//, 22, 
						new Turnout(board, 250, 214, "-/"),	// 221, 214
						new DiagonalBlock(board, 272, 192, '/'),	// 243
						new Turnout(board, 282, 182, "/-"),
						new PartialLinearBlock(board, 283, 182, 4, 'E')
					}
				}
			};
			blockDictionary.Add("6T", new BlockConfiguration(board, blocks, backdrop));

			signalDictionary.Add("10E", new DwarfSignal(board, "5EG", 199, 167, 'E'));
			signalDictionary.Add("12W", new MastSignal(board, "6WG", 694, 135, 'W', "2-3"));
			signalDictionary.Add("2WD", new MastSignal(board, "1WG", 582, 39, 'W', "1-3"));
			signalDictionary.Add("4W", new MastSignal(board, "2WG", 694, 71, 'W', "3-3-3"));
			signalDictionary.Add("8W", new MastSignal(board, "4WG", 694, 103, 'W', "3-3-3"));
			signalDictionary.Add("2E", new MastSignal(board, "1EG", 199, 103, 'E', "3-3-3"));
			signalDictionary.Add("6E", new MastSignal(board, "3EG", 199, 135, 'E', "3-3-3"));
			signalDictionary.Add("12EA", new MastSignal(board, "6EG", 199, 199, 'E', "1-3-3"));
			signalDictionary.Add("12ED", new MastSignal(board, "6EG", 199, 231, 'E', "1-3-3"));
			signalDictionary.Add("2WA", new MastSignal(board, "1WG", 582, 7, 'W', "1-1-3"));

			cp.Add(482, new ControlPoint(blockDictionary, signalDictionary));
		}
		static void InitalizeCP483(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();
			blockDictionary.Add("BO", new LinearBlock(board, 6, 114, 21));
			List<Block> backdrop = new List<Block> {
				new LinearBlock(board, 29, 114, 105),	// 54, 
				new DiagonalBlock(board, 97, 128, '\\'),
				new Turnout(board, 119, 150, "\\-"),
				new PartialLinearBlock(board, 123, 150, 12, 'E')

			};
			Dictionary<string, List<Block>> blocks = new Dictionary<string, List<Block>> {
				{ "1N", new List<Block> { new LinearBlock(board, 29, 114, 105) } },	// 55
				{ "1R",
					new List<Block> {
						new PartialLinearBlock(board, 30, 118, 57, 'W'),	// 55
						new Turnout(board, 87, 118, "-\\"),
						new DiagonalBlock(board, 97, 128, '\\'),
						new Turnout(board, 119, 150, "\\-"),
						new PartialLinearBlock(board, 123, 150, 12, 'E')
					}
				}
			};
			blockDictionary.Add("2T", new BlockConfiguration(board, blocks, backdrop));
			blockDictionary.Add("2O", new LinearBlock(board, 136, 114, 60, true));
			blockDictionary.Add("3O", new LinearBlock(board, 136, 146, 60, true));

			signalDictionary.Add("4WD", new DwarfSignal(board, "2WG", 134, 135, 'W'));
			signalDictionary.Add("4WA", new MastSignal(board, "2WG", 134, 103, 'W', "3-3"));
			signalDictionary.Add("1EI", new MastSignal(board, "1EI", 138, 103, 'E', "3-3"));
			signalDictionary.Add("1WI", new MastSignal(board, "1WI", 134, 73, 'W', "3-2"));
			signalDictionary.Add("4E", new MastSignal(board, "2EG", 30, 135, 'E', "3-3-2")); // 55, 167

			cp.Add(483, new ControlPoint(blockDictionary, signalDictionary));
		}
		static void InitalizeCP485(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();

			blockDictionary.Add("3T", new ArrowHead(board, 6, 82, "W", true));
			blockDictionary.Add("1WA", new LinearBlock(board, 6, 82, 128, true));
			blockDictionary.Add("4T", new ArrowHead(board, 6, 114, "W", true));
			cp.Add(485, new ControlPoint(blockDictionary, signalDictionary));
		}
		static void InitalizeCP487(PictureBox board) {
			Dictionary<string, Block> blockDictionary = new Dictionary<string, Block>();
			Dictionary<string, Signal> signalDictionary = new Dictionary<string, Signal>();
			cp.Add(487, new ControlPoint(blockDictionary, signalDictionary));
		}
	}
}

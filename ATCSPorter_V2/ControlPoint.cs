using System;
using System.Collections.Generic;

namespace ATCSPorter_V2 {
	/// <summary>
	/// Class used to keep track of the information for a CP
	/// </summary>
	class ControlPoint {
		readonly Dictionary<string,   Block> blocks = new Dictionary<string,  Block>();
		readonly Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
		DateTime? fileLastAccessed = null;

		public ControlPoint(Dictionary<string, Block> listOfBlocks, Dictionary<string, Signal> listOfSignals) {
			blocks = listOfBlocks;
			signals = listOfSignals;
		}
	}
}

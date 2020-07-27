namespace ATCSPorter_V2 {
	partial class MainWindow {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.board = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.board)).BeginInit();
			this.SuspendLayout();
			// 
			// board
			// 
			this.board.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.board.Location = new System.Drawing.Point(171, 54);
			this.board.Name = "board";
			this.board.Size = new System.Drawing.Size(983, 242);
			this.board.TabIndex = 0;
			this.board.TabStop = false;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(1430, 450);
			this.Controls.Add(this.board);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainWindow";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.board)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox board;
	}
}


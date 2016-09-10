using BrawlBuilder.Properties;

namespace BrawlBuilder
{
	partial class BrawlBuilder
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrawlBuilder));
			this.exit = new System.Windows.Forms.PictureBox();
			this.modFolder = new System.Windows.Forms.TextBox();
			this.modFolderBrowse = new System.Windows.Forms.Button();
			this.modFolder_lbl = new System.Windows.Forms.LinkLabel();
			this.brawlIso_lbl = new System.Windows.Forms.LinkLabel();
			this.brawlIsoBrowse = new System.Windows.Forms.Button();
			this.brawlIso = new System.Windows.Forms.TextBox();
			this.gctFile_lbl = new System.Windows.Forms.LinkLabel();
			this.gctFileBrowse = new System.Windows.Forms.Button();
			this.gctFile = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.bannerBrowse = new System.Windows.Forms.Button();
			this.banner = new System.Windows.Forms.TextBox();
			this.gameTitle = new System.Windows.Forms.TextBox();
			this.gameID = new System.Windows.Forms.TextBox();
			this.customBanner = new System.Windows.Forms.CheckBox();
			this.cutomTitle = new System.Windows.Forms.CheckBox();
			this.customID = new System.Windows.Forms.CheckBox();
			this.removeSubspace = new System.Windows.Forms.CheckBox();
			this.build = new System.Windows.Forms.Button();
			this.buildWorker = new System.ComponentModel.BackgroundWorker();
			this.blinker = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.exit)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// exit
			// 
			this.exit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exit.BackgroundImage")));
			this.exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.exit.Location = new System.Drawing.Point(312, 4);
			this.exit.Name = "exit";
			this.exit.Size = new System.Drawing.Size(18, 18);
			this.exit.TabIndex = 0;
			this.exit.TabStop = false;
			this.exit.Click += new System.EventHandler(this.exit_Click);
			// 
			// modFolder
			// 
			this.modFolder.Location = new System.Drawing.Point(116, 261);
			this.modFolder.Name = "modFolder";
			this.modFolder.Size = new System.Drawing.Size(125, 20);
			this.modFolder.TabIndex = 5;
			// 
			// modFolderBrowse
			// 
			this.modFolderBrowse.Location = new System.Drawing.Point(247, 260);
			this.modFolderBrowse.Name = "modFolderBrowse";
			this.modFolderBrowse.Size = new System.Drawing.Size(75, 22);
			this.modFolderBrowse.TabIndex = 6;
			this.modFolderBrowse.Text = "Browse...";
			this.modFolderBrowse.UseVisualStyleBackColor = true;
			this.modFolderBrowse.Click += new System.EventHandler(this.modFolderBrowse_Click);
			// 
			// modFolder_lbl
			// 
			this.modFolder_lbl.ActiveLinkColor = System.Drawing.Color.Fuchsia;
			this.modFolder_lbl.AutoSize = true;
			this.modFolder_lbl.BackColor = System.Drawing.Color.Transparent;
			this.modFolder_lbl.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.modFolder_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.modFolder_lbl.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.modFolder_lbl.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.modFolder_lbl.Location = new System.Drawing.Point(12, 262);
			this.modFolder_lbl.Name = "modFolder_lbl";
			this.modFolder_lbl.Size = new System.Drawing.Size(98, 16);
			this.modFolder_lbl.TabIndex = 4;
			this.modFolder_lbl.TabStop = true;
			this.modFolder_lbl.Text = "Mod Folder (?):";
			this.modFolder_lbl.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.modFolder_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.modFolder_lbl_LinkClicked);
			// 
			// brawlIso_lbl
			// 
			this.brawlIso_lbl.ActiveLinkColor = System.Drawing.Color.Fuchsia;
			this.brawlIso_lbl.AutoSize = true;
			this.brawlIso_lbl.BackColor = System.Drawing.Color.Transparent;
			this.brawlIso_lbl.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.brawlIso_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.brawlIso_lbl.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.brawlIso_lbl.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.brawlIso_lbl.Location = new System.Drawing.Point(23, 227);
			this.brawlIso_lbl.Name = "brawlIso_lbl";
			this.brawlIso_lbl.Size = new System.Drawing.Size(87, 16);
			this.brawlIso_lbl.TabIndex = 1;
			this.brawlIso_lbl.TabStop = true;
			this.brawlIso_lbl.Text = "Brawl ISO (?):";
			this.brawlIso_lbl.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.brawlIso_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.brawlIso_lbl_LinkClicked);
			// 
			// brawlIsoBrowse
			// 
			this.brawlIsoBrowse.Location = new System.Drawing.Point(247, 225);
			this.brawlIsoBrowse.Name = "brawlIsoBrowse";
			this.brawlIsoBrowse.Size = new System.Drawing.Size(75, 22);
			this.brawlIsoBrowse.TabIndex = 3;
			this.brawlIsoBrowse.Text = "Browse...";
			this.brawlIsoBrowse.UseVisualStyleBackColor = true;
			this.brawlIsoBrowse.Click += new System.EventHandler(this.brawlIsoBrowse_Click);
			// 
			// brawlIso
			// 
			this.brawlIso.Location = new System.Drawing.Point(116, 226);
			this.brawlIso.Name = "brawlIso";
			this.brawlIso.Size = new System.Drawing.Size(125, 20);
			this.brawlIso.TabIndex = 2;
			// 
			// gctFile_lbl
			// 
			this.gctFile_lbl.ActiveLinkColor = System.Drawing.Color.Fuchsia;
			this.gctFile_lbl.AutoSize = true;
			this.gctFile_lbl.BackColor = System.Drawing.Color.Transparent;
			this.gctFile_lbl.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.gctFile_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gctFile_lbl.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.gctFile_lbl.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.gctFile_lbl.Location = new System.Drawing.Point(29, 297);
			this.gctFile_lbl.Name = "gctFile_lbl";
			this.gctFile_lbl.Size = new System.Drawing.Size(82, 16);
			this.gctFile_lbl.TabIndex = 7;
			this.gctFile_lbl.TabStop = true;
			this.gctFile_lbl.Text = "GCT File (?):";
			this.gctFile_lbl.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.gctFile_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gctFile_lbl_LinkClicked);
			// 
			// gctFileBrowse
			// 
			this.gctFileBrowse.Location = new System.Drawing.Point(247, 295);
			this.gctFileBrowse.Name = "gctFileBrowse";
			this.gctFileBrowse.Size = new System.Drawing.Size(75, 22);
			this.gctFileBrowse.TabIndex = 9;
			this.gctFileBrowse.Text = "Browse...";
			this.gctFileBrowse.UseVisualStyleBackColor = true;
			this.gctFileBrowse.Click += new System.EventHandler(this.gctFileBrowse_Click);
			// 
			// gctFile
			// 
			this.gctFile.Location = new System.Drawing.Point(116, 296);
			this.gctFile.Name = "gctFile";
			this.gctFile.Size = new System.Drawing.Size(125, 20);
			this.gctFile.TabIndex = 8;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.bannerBrowse);
			this.groupBox1.Controls.Add(this.banner);
			this.groupBox1.Controls.Add(this.gameTitle);
			this.groupBox1.Controls.Add(this.gameID);
			this.groupBox1.Controls.Add(this.customBanner);
			this.groupBox1.Controls.Add(this.cutomTitle);
			this.groupBox1.Controls.Add(this.customID);
			this.groupBox1.Controls.Add(this.removeSubspace);
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.groupBox1.Location = new System.Drawing.Point(12, 325);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(310, 112);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Optional Extras";
			// 
			// bannerBrowse
			// 
			this.bannerBrowse.Enabled = false;
			this.bannerBrowse.ForeColor = System.Drawing.SystemColors.ControlText;
			this.bannerBrowse.Location = new System.Drawing.Point(227, 85);
			this.bannerBrowse.Name = "bannerBrowse";
			this.bannerBrowse.Size = new System.Drawing.Size(75, 22);
			this.bannerBrowse.TabIndex = 7;
			this.bannerBrowse.Text = "Browse...";
			this.bannerBrowse.UseVisualStyleBackColor = true;
			this.bannerBrowse.Click += new System.EventHandler(this.bannerBrowse_Click);
			// 
			// banner
			// 
			this.banner.Enabled = false;
			this.banner.Location = new System.Drawing.Point(121, 86);
			this.banner.Name = "banner";
			this.banner.Size = new System.Drawing.Size(104, 20);
			this.banner.TabIndex = 6;
			// 
			// gameTitle
			// 
			this.gameTitle.Enabled = false;
			this.gameTitle.Location = new System.Drawing.Point(121, 63);
			this.gameTitle.MaxLength = 63;
			this.gameTitle.Name = "gameTitle";
			this.gameTitle.Size = new System.Drawing.Size(181, 20);
			this.gameTitle.TabIndex = 4;
			// 
			// gameID
			// 
			this.gameID.Enabled = false;
			this.gameID.Location = new System.Drawing.Point(121, 40);
			this.gameID.MaxLength = 6;
			this.gameID.Name = "gameID";
			this.gameID.Size = new System.Drawing.Size(73, 20);
			this.gameID.TabIndex = 2;
			// 
			// customBanner
			// 
			this.customBanner.AutoSize = true;
			this.customBanner.Location = new System.Drawing.Point(6, 88);
			this.customBanner.Name = "customBanner";
			this.customBanner.Size = new System.Drawing.Size(100, 17);
			this.customBanner.TabIndex = 5;
			this.customBanner.Text = "Custom banner:";
			this.customBanner.UseVisualStyleBackColor = true;
			this.customBanner.CheckedChanged += new System.EventHandler(this.customBanner_CheckedChanged);
			// 
			// cutomTitle
			// 
			this.cutomTitle.AutoSize = true;
			this.cutomTitle.Location = new System.Drawing.Point(6, 65);
			this.cutomTitle.Name = "cutomTitle";
			this.cutomTitle.Size = new System.Drawing.Size(114, 17);
			this.cutomTitle.TabIndex = 3;
			this.cutomTitle.Text = "Change game title:";
			this.cutomTitle.UseVisualStyleBackColor = true;
			this.cutomTitle.CheckedChanged += new System.EventHandler(this.customTitle_CheckedChanged);
			// 
			// customID
			// 
			this.customID.AutoSize = true;
			this.customID.Location = new System.Drawing.Point(6, 42);
			this.customID.Name = "customID";
			this.customID.Size = new System.Drawing.Size(109, 17);
			this.customID.TabIndex = 1;
			this.customID.Text = "Change game ID:";
			this.customID.UseVisualStyleBackColor = true;
			this.customID.CheckedChanged += new System.EventHandler(this.customID_CheckedChanged);
			// 
			// removeSubspace
			// 
			this.removeSubspace.AutoSize = true;
			this.removeSubspace.Location = new System.Drawing.Point(6, 19);
			this.removeSubspace.Name = "removeSubspace";
			this.removeSubspace.Size = new System.Drawing.Size(296, 17);
			this.removeSubspace.TabIndex = 0;
			this.removeSubspace.Text = "Remove Subspace Emissary (greatly reduces output size)";
			this.removeSubspace.UseVisualStyleBackColor = true;
			// 
			// build
			// 
			this.build.BackColor = System.Drawing.Color.Transparent;
			this.build.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.build.FlatAppearance.BorderSize = 2;
			this.build.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(15)))), ((int)(((byte)(33)))));
			this.build.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(23)))), ((int)(((byte)(49)))));
			this.build.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.build.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.build.ForeColor = System.Drawing.Color.White;
			this.build.Location = new System.Drawing.Point(12, 449);
			this.build.Name = "build";
			this.build.Size = new System.Drawing.Size(310, 81);
			this.build.TabIndex = 11;
			this.build.Text = "Build ISO";
			this.build.UseVisualStyleBackColor = false;
			this.build.Click += new System.EventHandler(this.build_Click);
			this.build.MouseEnter += new System.EventHandler(this.build_MouseEnter);
			this.build.MouseLeave += new System.EventHandler(this.build_MouseLeave);
			// 
			// buildWorker
			// 
			this.buildWorker.WorkerSupportsCancellation = true;
			this.buildWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.buildWorker_DoWork);
			this.buildWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.buildWorker_RunWorkerCompleted);
			// 
			// blinker
			// 
			this.blinker.WorkerSupportsCancellation = true;
			this.blinker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.blinker_DoWork);
			// 
			// BrawlBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(334, 542);
			this.Controls.Add(this.build);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gctFile_lbl);
			this.Controls.Add(this.gctFileBrowse);
			this.Controls.Add(this.gctFile);
			this.Controls.Add(this.brawlIso_lbl);
			this.Controls.Add(this.brawlIsoBrowse);
			this.Controls.Add(this.brawlIso);
			this.Controls.Add(this.modFolder_lbl);
			this.Controls.Add(this.modFolderBrowse);
			this.Controls.Add(this.modFolder);
			this.Controls.Add(this.exit);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = Resources.icon;
			this.MaximumSize = new System.Drawing.Size(334, 542);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(334, 542);
			this.Name = "BrawlBuilder";
			this.Opacity = 0D;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BrawlBuilder - The Ultimate Brawl ISO Builder";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrawlBuilder_FormClosing);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.exit)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox exit;
		private System.Windows.Forms.TextBox modFolder;
		private System.Windows.Forms.Button modFolderBrowse;
		private System.Windows.Forms.LinkLabel modFolder_lbl;
		private System.Windows.Forms.LinkLabel brawlIso_lbl;
		private System.Windows.Forms.Button brawlIsoBrowse;
		private System.Windows.Forms.TextBox brawlIso;
		private System.Windows.Forms.LinkLabel gctFile_lbl;
		private System.Windows.Forms.Button gctFileBrowse;
		private System.Windows.Forms.TextBox gctFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cutomTitle;
		private System.Windows.Forms.CheckBox customID;
		private System.Windows.Forms.CheckBox removeSubspace;
		private System.Windows.Forms.CheckBox customBanner;
		private System.Windows.Forms.Button bannerBrowse;
		private System.Windows.Forms.TextBox banner;
		private System.Windows.Forms.TextBox gameTitle;
		private System.Windows.Forms.TextBox gameID;
		private System.Windows.Forms.Button build;
		private System.ComponentModel.BackgroundWorker buildWorker;
		private System.ComponentModel.BackgroundWorker blinker;
	}
}


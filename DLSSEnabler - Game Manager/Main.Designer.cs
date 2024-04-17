namespace DLSSEnabler___Game_Manager
{
    partial class Main
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.enableDLSS = new System.Windows.Forms.Button();
            this.GPUlabel = new System.Windows.Forms.Label();
            this.GPUplaceholder = new System.Windows.Forms.Label();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.disableDLSS = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.editIni = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleDocToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nexusPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dLSSEnablerLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buyCoffee = new System.Windows.Forms.PictureBox();
            this.sortBox = new System.Windows.Forms.ComboBox();
            this.dlssEnVer = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buyCoffee)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 27);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(624, 414);
            this.listView1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.listView1, "Double click to open game path");
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // enableDLSS
            // 
            this.enableDLSS.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.enableDLSS.Location = new System.Drawing.Point(18, 578);
            this.enableDLSS.Name = "enableDLSS";
            this.enableDLSS.Size = new System.Drawing.Size(120, 43);
            this.enableDLSS.TabIndex = 1;
            this.enableDLSS.Text = "Enable DLSS";
            this.enableDLSS.UseVisualStyleBackColor = false;
            this.enableDLSS.Click += new System.EventHandler(this.button1_Click);
            // 
            // GPUlabel
            // 
            this.GPUlabel.AutoSize = true;
            this.GPUlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUlabel.Location = new System.Drawing.Point(260, 5);
            this.GPUlabel.Name = "GPUlabel";
            this.GPUlabel.Size = new System.Drawing.Size(33, 15);
            this.GPUlabel.TabIndex = 2;
            this.GPUlabel.Text = "GPU";
            // 
            // GPUplaceholder
            // 
            this.GPUplaceholder.AutoSize = true;
            this.GPUplaceholder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUplaceholder.Location = new System.Drawing.Point(227, 5);
            this.GPUplaceholder.Name = "GPUplaceholder";
            this.GPUplaceholder.Size = new System.Drawing.Size(36, 15);
            this.GPUplaceholder.TabIndex = 3;
            this.GPUplaceholder.Text = "GPU";
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(12, 462);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(624, 20);
            this.searchBox.TabIndex = 4;
            // 
            // disableDLSS
            // 
            this.disableDLSS.BackColor = System.Drawing.Color.Salmon;
            this.disableDLSS.Location = new System.Drawing.Point(144, 578);
            this.disableDLSS.Name = "disableDLSS";
            this.disableDLSS.Size = new System.Drawing.Size(120, 43);
            this.disableDLSS.TabIndex = 5;
            this.disableDLSS.Text = "Disable DLSS";
            this.disableDLSS.UseVisualStyleBackColor = false;
            this.disableDLSS.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 444);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Search a game";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(290, 518);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Can\'t find a game? Add it manually";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(507, 505);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(120, 43);
            this.browse.TabIndex = 8;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // editIni
            // 
            this.editIni.Location = new System.Drawing.Point(270, 578);
            this.editIni.Name = "editIni";
            this.editIni.Size = new System.Drawing.Size(120, 43);
            this.editIni.TabIndex = 9;
            this.editIni.Text = "Customize DLSS";
            this.editIni.UseVisualStyleBackColor = true;
            this.editIni.Click += new System.EventHandler(this.editIni_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(507, 578);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(120, 43);
            this.exit.TabIndex = 10;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.logsToolStripMenuItem,
            this.extraToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(648, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.googleDocToolStripMenuItem,
            this.discordToolStripMenuItem,
            this.nexusPageToolStripMenuItem,
            this.githubToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // googleDocToolStripMenuItem
            // 
            this.googleDocToolStripMenuItem.Image = global::DLSSEnabler___Game_Manager.Properties.Resources.icons_google_sheets;
            this.googleDocToolStripMenuItem.Name = "googleDocToolStripMenuItem";
            this.googleDocToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.googleDocToolStripMenuItem.Text = "Compatibility doc";
            this.googleDocToolStripMenuItem.Click += new System.EventHandler(this.googleDocToolStripMenuItem_Click);
            // 
            // discordToolStripMenuItem
            // 
            this.discordToolStripMenuItem.Image = global::DLSSEnabler___Game_Manager.Properties.Resources.icon_discord;
            this.discordToolStripMenuItem.Name = "discordToolStripMenuItem";
            this.discordToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.discordToolStripMenuItem.Text = "Discord";
            this.discordToolStripMenuItem.Click += new System.EventHandler(this.discordToolStripMenuItem_Click);
            // 
            // nexusPageToolStripMenuItem
            // 
            this.nexusPageToolStripMenuItem.Image = global::DLSSEnabler___Game_Manager.Properties.Resources.icons_nexus;
            this.nexusPageToolStripMenuItem.Name = "nexusPageToolStripMenuItem";
            this.nexusPageToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.nexusPageToolStripMenuItem.Text = "Nexus Mods";
            this.nexusPageToolStripMenuItem.Click += new System.EventHandler(this.nexusPageToolStripMenuItem_Click);
            // 
            // githubToolStripMenuItem
            // 
            this.githubToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("githubToolStripMenuItem.Image")));
            this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
            this.githubToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.githubToolStripMenuItem.Text = "Github";
            this.githubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem_Click);
            // 
            // logsToolStripMenuItem
            // 
            this.logsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dLSSEnablerLogToolStripMenuItem});
            this.logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            this.logsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.logsToolStripMenuItem.Text = "Logs";
            // 
            // dLSSEnablerLogToolStripMenuItem
            // 
            this.dLSSEnablerLogToolStripMenuItem.Name = "dLSSEnablerLogToolStripMenuItem";
            this.dLSSEnablerLogToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.dLSSEnablerLogToolStripMenuItem.Text = "DLSS Enabler Log";
            this.dLSSEnablerLogToolStripMenuItem.Click += new System.EventHandler(this.dLSSEnablerLogToolStripMenuItem_Click);
            // 
            // extraToolStripMenuItem
            // 
            this.extraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem});
            this.extraToolStripMenuItem.Name = "extraToolStripMenuItem";
            this.extraToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.extraToolStripMenuItem.Text = "Extra";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // buyCoffee
            // 
            this.buyCoffee.Image = global::DLSSEnabler___Game_Manager.Properties.Resources.buy_coffee;
            this.buyCoffee.Location = new System.Drawing.Point(21, 501);
            this.buyCoffee.Name = "buyCoffee";
            this.buyCoffee.Size = new System.Drawing.Size(243, 50);
            this.buyCoffee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.buyCoffee.TabIndex = 12;
            this.buyCoffee.TabStop = false;
            this.toolTip1.SetToolTip(this.buyCoffee, "If you like my mod and what I do, offer me a coffe :)");
            this.buyCoffee.Click += new System.EventHandler(this.buyCoffee_Click);
            // 
            // sortBox
            // 
            this.sortBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortBox.FormattingEnabled = true;
            this.sortBox.Items.AddRange(new object[] {
            "Sort A - Z",
            "Sort Z - A"});
            this.sortBox.Location = new System.Drawing.Point(547, 3);
            this.sortBox.Name = "sortBox";
            this.sortBox.Size = new System.Drawing.Size(89, 21);
            this.sortBox.TabIndex = 13;
            this.sortBox.SelectedIndexChanged += new System.EventHandler(this.sortBox_SelectedIndexChanged);
            // 
            // dlssEnVer
            // 
            this.dlssEnVer.AutoSize = true;
            this.dlssEnVer.Location = new System.Drawing.Point(3, 630);
            this.dlssEnVer.Name = "dlssEnVer";
            this.dlssEnVer.Size = new System.Drawing.Size(293, 13);
            this.dlssEnVer.TabIndex = 14;
            this.dlssEnVer.Text = "version not found - place the manager near the README file";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 645);
            this.Controls.Add(this.dlssEnVer);
            this.Controls.Add(this.sortBox);
            this.Controls.Add(this.buyCoffee);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.editIni);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.disableDLSS);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.GPUplaceholder);
            this.Controls.Add(this.GPUlabel);
            this.Controls.Add(this.enableDLSS);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "DLSSEnabler - Game Manager";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buyCoffee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button enableDLSS;
        private System.Windows.Forms.Label GPUlabel;
        private System.Windows.Forms.Label GPUplaceholder;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button disableDLSS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Button editIni;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleDocToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nexusPageToolStripMenuItem;
        private System.Windows.Forms.PictureBox buyCoffee;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem logsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dLSSEnablerLogToolStripMenuItem;
        private System.Windows.Forms.ComboBox sortBox;
        private System.Windows.Forms.Label dlssEnVer;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
    }
}


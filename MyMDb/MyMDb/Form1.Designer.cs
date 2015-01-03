namespace MyMDb
{
    partial class Form1
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.gbConnectionError = new System.Windows.Forms.GroupBox();
			this.txtConnectionError = new System.Windows.Forms.TextBox();
			this.bntTestSqlServer = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbIntegratedSecurity = new System.Windows.Forms.CheckBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtBuildDB = new System.Windows.Forms.RichTextBox();
			this.bntBuildDB = new System.Windows.Forms.Button();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.cbRemCompress = new System.Windows.Forms.CheckBox();
			this.cbFtp_Acs = new System.Windows.Forms.CheckBox();
			this.pb_Acs = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Acs = new System.Windows.Forms.Label();
			this.cbFtp_Act = new System.Windows.Forms.CheckBox();
			this.pb_Act = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Act = new System.Windows.Forms.Label();
			this.cbFtp_Aka = new System.Windows.Forms.CheckBox();
			this.pb_Aka = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Aka = new System.Windows.Forms.Label();
			this.cbFtp_Cou = new System.Windows.Forms.CheckBox();
			this.pb_Cou = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Cou = new System.Windows.Forms.Label();
			this.cbFtp_Tag = new System.Windows.Forms.CheckBox();
			this.pb_Tag = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Tag = new System.Windows.Forms.Label();
			this.cbFtp_Plo = new System.Windows.Forms.CheckBox();
			this.pb_Plo = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Plo = new System.Windows.Forms.Label();
			this.cbFtp_Gen = new System.Windows.Forms.CheckBox();
			this.pb_Gen = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Gen = new System.Windows.Forms.Label();
			this.cbFtp_Mov = new System.Windows.Forms.CheckBox();
			this.pb_Mov = new System.Windows.Forms.ProgressBar();
			this.lblFtpDLSizeSpeed_Mov = new System.Windows.Forms.Label();
			this.bntDownloadData = new System.Windows.Forms.Button();
			this.bntBrowseLocalFolder = new System.Windows.Forms.Button();
			this.txtBrowseLocalFolder = new System.Windows.Forms.TextBox();
			this.lblLocalFolder = new System.Windows.Forms.Label();
			this.lblFTPFrom = new System.Windows.Forms.Label();
			this.cbIMDBInterfaces = new System.Windows.Forms.ComboBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.lblProcessFTPData = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chklbSyncFiles = new System.Windows.Forms.CheckedListBox();
			this.bntBullk = new System.Windows.Forms.Button();
			this.pbProcessFTPData = new System.Windows.Forms.ProgressBar();
			this.txtProcessData = new System.Windows.Forms.RichTextBox();
			this.checkBoxDistributors = new System.Windows.Forms.CheckBox();
			this.progressBarDistributors = new System.Windows.Forms.ProgressBar();
			this.labelFtpDlSizeDistributors = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.gbConnectionError.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(771, 441);
			this.tabControl1.TabIndex = 5;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.gbConnectionError);
			this.tabPage1.Controls.Add(this.bntTestSqlServer);
			this.tabPage1.Controls.Add(this.txtPassword);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.cbIntegratedSecurity);
			this.tabPage1.Controls.Add(this.txtUsername);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.txtServer);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(763, 415);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "SQL Server";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// gbConnectionError
			// 
			this.gbConnectionError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbConnectionError.Controls.Add(this.txtConnectionError);
			this.gbConnectionError.Location = new System.Drawing.Point(30, 143);
			this.gbConnectionError.Name = "gbConnectionError";
			this.gbConnectionError.Size = new System.Drawing.Size(701, 185);
			this.gbConnectionError.TabIndex = 8;
			this.gbConnectionError.TabStop = false;
			this.gbConnectionError.Text = "Connection Error";
			this.gbConnectionError.Visible = false;
			// 
			// txtConnectionError
			// 
			this.txtConnectionError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtConnectionError.Location = new System.Drawing.Point(3, 16);
			this.txtConnectionError.Multiline = true;
			this.txtConnectionError.Name = "txtConnectionError";
			this.txtConnectionError.ReadOnly = true;
			this.txtConnectionError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtConnectionError.Size = new System.Drawing.Size(695, 166);
			this.txtConnectionError.TabIndex = 0;
			this.txtConnectionError.WordWrap = false;
			// 
			// bntTestSqlServer
			// 
			this.bntTestSqlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bntTestSqlServer.Location = new System.Drawing.Point(30, 359);
			this.bntTestSqlServer.Margin = new System.Windows.Forms.Padding(30, 3, 30, 30);
			this.bntTestSqlServer.Name = "bntTestSqlServer";
			this.bntTestSqlServer.Size = new System.Drawing.Size(703, 23);
			this.bntTestSqlServer.TabIndex = 7;
			this.bntTestSqlServer.Text = "Test Connection";
			this.bntTestSqlServer.UseVisualStyleBackColor = true;
			this.bntTestSqlServer.Click += new System.EventHandler(this.bntTestSqlServer_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(98, 117);
			this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(632, 20);
			this.txtPassword.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(30, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Password";
			// 
			// cbIntegratedSecurity
			// 
			this.cbIntegratedSecurity.AutoSize = true;
			this.cbIntegratedSecurity.Checked = true;
			this.cbIntegratedSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbIntegratedSecurity.Location = new System.Drawing.Point(60, 60);
			this.cbIntegratedSecurity.Name = "cbIntegratedSecurity";
			this.cbIntegratedSecurity.Size = new System.Drawing.Size(260, 17);
			this.cbIntegratedSecurity.TabIndex = 2;
			this.cbIntegratedSecurity.Text = "Use Integrated Security (Windows authentication)";
			this.cbIntegratedSecurity.UseVisualStyleBackColor = true;
			// 
			// txtUsername
			// 
			this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsername.Location = new System.Drawing.Point(98, 87);
			this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(632, 20);
			this.txtUsername.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 90);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Username";
			// 
			// txtServer
			// 
			this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtServer.Location = new System.Drawing.Point(98, 27);
			this.txtServer.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(632, 20);
			this.txtServer.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(30, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "SQL Server";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtBuildDB);
			this.tabPage2.Controls.Add(this.bntBuildDB);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(763, 415);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Local Database";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// txtBuildDB
			// 
			this.txtBuildDB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBuildDB.Location = new System.Drawing.Point(6, 35);
			this.txtBuildDB.Name = "txtBuildDB";
			this.txtBuildDB.ReadOnly = true;
			this.txtBuildDB.Size = new System.Drawing.Size(751, 374);
			this.txtBuildDB.TabIndex = 1;
			this.txtBuildDB.Text = "";
			// 
			// bntBuildDB
			// 
			this.bntBuildDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bntBuildDB.Location = new System.Drawing.Point(6, 6);
			this.bntBuildDB.Name = "bntBuildDB";
			this.bntBuildDB.Size = new System.Drawing.Size(752, 23);
			this.bntBuildDB.TabIndex = 0;
			this.bntBuildDB.Text = "Build Database on my Server";
			this.bntBuildDB.UseVisualStyleBackColor = true;
			this.bntBuildDB.Click += new System.EventHandler(this.bntBuildDB_Click);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.checkBoxDistributors);
			this.tabPage3.Controls.Add(this.progressBarDistributors);
			this.tabPage3.Controls.Add(this.labelFtpDlSizeDistributors);
			this.tabPage3.Controls.Add(this.cbRemCompress);
			this.tabPage3.Controls.Add(this.cbFtp_Acs);
			this.tabPage3.Controls.Add(this.pb_Acs);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Acs);
			this.tabPage3.Controls.Add(this.cbFtp_Act);
			this.tabPage3.Controls.Add(this.pb_Act);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Act);
			this.tabPage3.Controls.Add(this.cbFtp_Aka);
			this.tabPage3.Controls.Add(this.pb_Aka);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Aka);
			this.tabPage3.Controls.Add(this.cbFtp_Cou);
			this.tabPage3.Controls.Add(this.pb_Cou);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Cou);
			this.tabPage3.Controls.Add(this.cbFtp_Tag);
			this.tabPage3.Controls.Add(this.pb_Tag);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Tag);
			this.tabPage3.Controls.Add(this.cbFtp_Plo);
			this.tabPage3.Controls.Add(this.pb_Plo);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Plo);
			this.tabPage3.Controls.Add(this.cbFtp_Gen);
			this.tabPage3.Controls.Add(this.pb_Gen);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Gen);
			this.tabPage3.Controls.Add(this.cbFtp_Mov);
			this.tabPage3.Controls.Add(this.pb_Mov);
			this.tabPage3.Controls.Add(this.lblFtpDLSizeSpeed_Mov);
			this.tabPage3.Controls.Add(this.bntDownloadData);
			this.tabPage3.Controls.Add(this.bntBrowseLocalFolder);
			this.tabPage3.Controls.Add(this.txtBrowseLocalFolder);
			this.tabPage3.Controls.Add(this.lblLocalFolder);
			this.tabPage3.Controls.Add(this.lblFTPFrom);
			this.tabPage3.Controls.Add(this.cbIMDBInterfaces);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(763, 415);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "FTP Server";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// cbRemCompress
			// 
			this.cbRemCompress.AutoSize = true;
			this.cbRemCompress.Location = new System.Drawing.Point(6, 297);
			this.cbRemCompress.Name = "cbRemCompress";
			this.cbRemCompress.Size = new System.Drawing.Size(316, 17);
			this.cbRemCompress.TabIndex = 56;
			this.cbRemCompress.Text = "Remove Compressed files after decompressing to save space";
			this.cbRemCompress.UseVisualStyleBackColor = true;
			// 
			// cbFtp_Acs
			// 
			this.cbFtp_Acs.AutoSize = true;
			this.cbFtp_Acs.Enabled = false;
			this.cbFtp_Acs.Location = new System.Drawing.Point(6, 222);
			this.cbFtp_Acs.Name = "cbFtp_Acs";
			this.cbFtp_Acs.Size = new System.Drawing.Size(80, 17);
			this.cbFtp_Acs.TabIndex = 55;
			this.cbFtp_Acs.Text = "Actress List";
			this.cbFtp_Acs.UseVisualStyleBackColor = true;
			this.cbFtp_Acs.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Acs
			// 
			this.pb_Acs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Acs.Location = new System.Drawing.Point(117, 222);
			this.pb_Acs.Name = "pb_Acs";
			this.pb_Acs.Size = new System.Drawing.Size(396, 17);
			this.pb_Acs.TabIndex = 54;
			// 
			// lblFtpDLSizeSpeed_Acs
			// 
			this.lblFtpDLSizeSpeed_Acs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Acs.AutoSize = true;
			this.lblFtpDLSizeSpeed_Acs.Location = new System.Drawing.Point(519, 223);
			this.lblFtpDLSizeSpeed_Acs.Name = "lblFtpDLSizeSpeed_Acs";
			this.lblFtpDLSizeSpeed_Acs.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Acs.TabIndex = 53;
			this.lblFtpDLSizeSpeed_Acs.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Act
			// 
			this.cbFtp_Act.AutoSize = true;
			this.cbFtp_Act.Enabled = false;
			this.cbFtp_Act.Location = new System.Drawing.Point(6, 199);
			this.cbFtp_Act.Name = "cbFtp_Act";
			this.cbFtp_Act.Size = new System.Drawing.Size(70, 17);
			this.cbFtp_Act.TabIndex = 52;
			this.cbFtp_Act.Text = "Actor List";
			this.cbFtp_Act.UseVisualStyleBackColor = true;
			this.cbFtp_Act.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Act
			// 
			this.pb_Act.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Act.Location = new System.Drawing.Point(117, 199);
			this.pb_Act.Name = "pb_Act";
			this.pb_Act.Size = new System.Drawing.Size(396, 17);
			this.pb_Act.TabIndex = 51;
			// 
			// lblFtpDLSizeSpeed_Act
			// 
			this.lblFtpDLSizeSpeed_Act.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Act.AutoSize = true;
			this.lblFtpDLSizeSpeed_Act.Location = new System.Drawing.Point(519, 200);
			this.lblFtpDLSizeSpeed_Act.Name = "lblFtpDLSizeSpeed_Act";
			this.lblFtpDLSizeSpeed_Act.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Act.TabIndex = 50;
			this.lblFtpDLSizeSpeed_Act.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Aka
			// 
			this.cbFtp_Aka.AutoSize = true;
			this.cbFtp_Aka.Enabled = false;
			this.cbFtp_Aka.Location = new System.Drawing.Point(6, 176);
			this.cbFtp_Aka.Name = "cbFtp_Aka";
			this.cbFtp_Aka.Size = new System.Drawing.Size(89, 17);
			this.cbFtp_Aka.TabIndex = 49;
			this.cbFtp_Aka.Text = "AKA Title List";
			this.cbFtp_Aka.UseVisualStyleBackColor = true;
			this.cbFtp_Aka.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Aka
			// 
			this.pb_Aka.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Aka.Location = new System.Drawing.Point(117, 176);
			this.pb_Aka.Name = "pb_Aka";
			this.pb_Aka.Size = new System.Drawing.Size(396, 17);
			this.pb_Aka.TabIndex = 48;
			// 
			// lblFtpDLSizeSpeed_Aka
			// 
			this.lblFtpDLSizeSpeed_Aka.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Aka.AutoSize = true;
			this.lblFtpDLSizeSpeed_Aka.Location = new System.Drawing.Point(519, 177);
			this.lblFtpDLSizeSpeed_Aka.Name = "lblFtpDLSizeSpeed_Aka";
			this.lblFtpDLSizeSpeed_Aka.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Aka.TabIndex = 47;
			this.lblFtpDLSizeSpeed_Aka.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Cou
			// 
			this.cbFtp_Cou.AutoSize = true;
			this.cbFtp_Cou.Enabled = false;
			this.cbFtp_Cou.Location = new System.Drawing.Point(6, 153);
			this.cbFtp_Cou.Name = "cbFtp_Cou";
			this.cbFtp_Cou.Size = new System.Drawing.Size(81, 17);
			this.cbFtp_Cou.TabIndex = 46;
			this.cbFtp_Cou.Text = "Country List";
			this.cbFtp_Cou.UseVisualStyleBackColor = true;
			this.cbFtp_Cou.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Cou
			// 
			this.pb_Cou.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Cou.Location = new System.Drawing.Point(117, 153);
			this.pb_Cou.Name = "pb_Cou";
			this.pb_Cou.Size = new System.Drawing.Size(396, 17);
			this.pb_Cou.TabIndex = 45;
			// 
			// lblFtpDLSizeSpeed_Cou
			// 
			this.lblFtpDLSizeSpeed_Cou.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Cou.AutoSize = true;
			this.lblFtpDLSizeSpeed_Cou.Location = new System.Drawing.Point(519, 154);
			this.lblFtpDLSizeSpeed_Cou.Name = "lblFtpDLSizeSpeed_Cou";
			this.lblFtpDLSizeSpeed_Cou.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Cou.TabIndex = 44;
			this.lblFtpDLSizeSpeed_Cou.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Tag
			// 
			this.cbFtp_Tag.AutoSize = true;
			this.cbFtp_Tag.Enabled = false;
			this.cbFtp_Tag.Location = new System.Drawing.Point(6, 130);
			this.cbFtp_Tag.Name = "cbFtp_Tag";
			this.cbFtp_Tag.Size = new System.Drawing.Size(87, 17);
			this.cbFtp_Tag.TabIndex = 43;
			this.cbFtp_Tag.Text = "Tag Line List";
			this.cbFtp_Tag.UseVisualStyleBackColor = true;
			this.cbFtp_Tag.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Tag
			// 
			this.pb_Tag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Tag.Location = new System.Drawing.Point(117, 130);
			this.pb_Tag.Name = "pb_Tag";
			this.pb_Tag.Size = new System.Drawing.Size(396, 17);
			this.pb_Tag.TabIndex = 42;
			// 
			// lblFtpDLSizeSpeed_Tag
			// 
			this.lblFtpDLSizeSpeed_Tag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Tag.AutoSize = true;
			this.lblFtpDLSizeSpeed_Tag.Location = new System.Drawing.Point(519, 131);
			this.lblFtpDLSizeSpeed_Tag.Name = "lblFtpDLSizeSpeed_Tag";
			this.lblFtpDLSizeSpeed_Tag.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Tag.TabIndex = 41;
			this.lblFtpDLSizeSpeed_Tag.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Plo
			// 
			this.cbFtp_Plo.AutoSize = true;
			this.cbFtp_Plo.Enabled = false;
			this.cbFtp_Plo.Location = new System.Drawing.Point(6, 107);
			this.cbFtp_Plo.Name = "cbFtp_Plo";
			this.cbFtp_Plo.Size = new System.Drawing.Size(63, 17);
			this.cbFtp_Plo.TabIndex = 40;
			this.cbFtp_Plo.Text = "Plot List";
			this.cbFtp_Plo.UseVisualStyleBackColor = true;
			this.cbFtp_Plo.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Plo
			// 
			this.pb_Plo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Plo.Location = new System.Drawing.Point(117, 107);
			this.pb_Plo.Name = "pb_Plo";
			this.pb_Plo.Size = new System.Drawing.Size(396, 17);
			this.pb_Plo.TabIndex = 39;
			// 
			// lblFtpDLSizeSpeed_Plo
			// 
			this.lblFtpDLSizeSpeed_Plo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Plo.AutoSize = true;
			this.lblFtpDLSizeSpeed_Plo.Location = new System.Drawing.Point(519, 108);
			this.lblFtpDLSizeSpeed_Plo.Name = "lblFtpDLSizeSpeed_Plo";
			this.lblFtpDLSizeSpeed_Plo.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Plo.TabIndex = 38;
			this.lblFtpDLSizeSpeed_Plo.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Gen
			// 
			this.cbFtp_Gen.AutoSize = true;
			this.cbFtp_Gen.Enabled = false;
			this.cbFtp_Gen.Location = new System.Drawing.Point(6, 84);
			this.cbFtp_Gen.Name = "cbFtp_Gen";
			this.cbFtp_Gen.Size = new System.Drawing.Size(74, 17);
			this.cbFtp_Gen.TabIndex = 37;
			this.cbFtp_Gen.Text = "Genre List";
			this.cbFtp_Gen.UseVisualStyleBackColor = true;
			this.cbFtp_Gen.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Gen
			// 
			this.pb_Gen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Gen.Location = new System.Drawing.Point(117, 84);
			this.pb_Gen.Name = "pb_Gen";
			this.pb_Gen.Size = new System.Drawing.Size(396, 17);
			this.pb_Gen.TabIndex = 36;
			// 
			// lblFtpDLSizeSpeed_Gen
			// 
			this.lblFtpDLSizeSpeed_Gen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Gen.AutoSize = true;
			this.lblFtpDLSizeSpeed_Gen.Location = new System.Drawing.Point(519, 85);
			this.lblFtpDLSizeSpeed_Gen.Name = "lblFtpDLSizeSpeed_Gen";
			this.lblFtpDLSizeSpeed_Gen.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Gen.TabIndex = 35;
			this.lblFtpDLSizeSpeed_Gen.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// cbFtp_Mov
			// 
			this.cbFtp_Mov.AutoSize = true;
			this.cbFtp_Mov.Checked = true;
			this.cbFtp_Mov.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbFtp_Mov.Enabled = false;
			this.cbFtp_Mov.Location = new System.Drawing.Point(6, 61);
			this.cbFtp_Mov.Name = "cbFtp_Mov";
			this.cbFtp_Mov.Size = new System.Drawing.Size(74, 17);
			this.cbFtp_Mov.TabIndex = 34;
			this.cbFtp_Mov.Text = "Movie List";
			this.cbFtp_Mov.UseVisualStyleBackColor = true;
			this.cbFtp_Mov.CheckedChanged += new System.EventHandler(this.cbFtp_XXX_CheckedChanged);
			// 
			// pb_Mov
			// 
			this.pb_Mov.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pb_Mov.Location = new System.Drawing.Point(117, 61);
			this.pb_Mov.Name = "pb_Mov";
			this.pb_Mov.Size = new System.Drawing.Size(396, 17);
			this.pb_Mov.TabIndex = 12;
			// 
			// lblFtpDLSizeSpeed_Mov
			// 
			this.lblFtpDLSizeSpeed_Mov.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFtpDLSizeSpeed_Mov.AutoSize = true;
			this.lblFtpDLSizeSpeed_Mov.Location = new System.Drawing.Point(519, 62);
			this.lblFtpDLSizeSpeed_Mov.Name = "lblFtpDLSizeSpeed_Mov";
			this.lblFtpDLSizeSpeed_Mov.Size = new System.Drawing.Size(157, 13);
			this.lblFtpDLSizeSpeed_Mov.TabIndex = 11;
			this.lblFtpDLSizeSpeed_Mov.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// bntDownloadData
			// 
			this.bntDownloadData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bntDownloadData.Enabled = false;
			this.bntDownloadData.Location = new System.Drawing.Point(6, 268);
			this.bntDownloadData.Name = "bntDownloadData";
			this.bntDownloadData.Size = new System.Drawing.Size(752, 23);
			this.bntDownloadData.TabIndex = 9;
			this.bntDownloadData.Text = "Download Data";
			this.bntDownloadData.UseVisualStyleBackColor = true;
			this.bntDownloadData.Click += new System.EventHandler(this.bntDownloadData_Click);
			// 
			// bntBrowseLocalFolder
			// 
			this.bntBrowseLocalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bntBrowseLocalFolder.Location = new System.Drawing.Point(731, 33);
			this.bntBrowseLocalFolder.Name = "bntBrowseLocalFolder";
			this.bntBrowseLocalFolder.Size = new System.Drawing.Size(27, 23);
			this.bntBrowseLocalFolder.TabIndex = 8;
			this.bntBrowseLocalFolder.Text = "...";
			this.bntBrowseLocalFolder.UseVisualStyleBackColor = true;
			this.bntBrowseLocalFolder.Click += new System.EventHandler(this.bntBrowseLocalFolder_Click);
			// 
			// txtBrowseLocalFolder
			// 
			this.txtBrowseLocalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBrowseLocalFolder.Location = new System.Drawing.Point(117, 35);
			this.txtBrowseLocalFolder.Name = "txtBrowseLocalFolder";
			this.txtBrowseLocalFolder.Size = new System.Drawing.Size(608, 20);
			this.txtBrowseLocalFolder.TabIndex = 7;
			this.txtBrowseLocalFolder.TextChanged += new System.EventHandler(this.txtBrowseLocalFolder_TextChanged);
			// 
			// lblLocalFolder
			// 
			this.lblLocalFolder.AutoSize = true;
			this.lblLocalFolder.Location = new System.Drawing.Point(6, 38);
			this.lblLocalFolder.Name = "lblLocalFolder";
			this.lblLocalFolder.Size = new System.Drawing.Size(95, 13);
			this.lblLocalFolder.TabIndex = 6;
			this.lblLocalFolder.Text = "Working Directory:";
			// 
			// lblFTPFrom
			// 
			this.lblFTPFrom.AutoSize = true;
			this.lblFTPFrom.Location = new System.Drawing.Point(6, 9);
			this.lblFTPFrom.Name = "lblFTPFrom";
			this.lblFTPFrom.Size = new System.Drawing.Size(105, 13);
			this.lblFTPFrom.TabIndex = 1;
			this.lblFTPFrom.Text = "Download data from:";
			// 
			// cbIMDBInterfaces
			// 
			this.cbIMDBInterfaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbIMDBInterfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbIMDBInterfaces.FormattingEnabled = true;
			this.cbIMDBInterfaces.Items.AddRange(new object[] {
            "Sweden",
            "Finland",
            "Germany"});
			this.cbIMDBInterfaces.Location = new System.Drawing.Point(117, 6);
			this.cbIMDBInterfaces.Name = "cbIMDBInterfaces";
			this.cbIMDBInterfaces.Size = new System.Drawing.Size(640, 21);
			this.cbIMDBInterfaces.TabIndex = 0;
			this.cbIMDBInterfaces.SelectedIndexChanged += new System.EventHandler(this.cbIMDBInterfaces_SelectedIndexChanged);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.lblProcessFTPData);
			this.tabPage4.Controls.Add(this.groupBox1);
			this.tabPage4.Controls.Add(this.bntBullk);
			this.tabPage4.Controls.Add(this.pbProcessFTPData);
			this.tabPage4.Controls.Add(this.txtProcessData);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(763, 415);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Synchronize My Database";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// lblProcessFTPData
			// 
			this.lblProcessFTPData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblProcessFTPData.Location = new System.Drawing.Point(625, 32);
			this.lblProcessFTPData.Name = "lblProcessFTPData";
			this.lblProcessFTPData.Size = new System.Drawing.Size(133, 13);
			this.lblProcessFTPData.TabIndex = 6;
			this.lblProcessFTPData.Text = "0 b";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.chklbSyncFiles);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(162, 404);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data to Sync (Process)";
			// 
			// chklbSyncFiles
			// 
			this.chklbSyncFiles.CheckOnClick = true;
			this.chklbSyncFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chklbSyncFiles.FormattingEnabled = true;
			this.chklbSyncFiles.Location = new System.Drawing.Point(3, 16);
			this.chklbSyncFiles.Name = "chklbSyncFiles";
			this.chklbSyncFiles.Size = new System.Drawing.Size(156, 385);
			this.chklbSyncFiles.TabIndex = 0;
			// 
			// bntBullk
			// 
			this.bntBullk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bntBullk.Location = new System.Drawing.Point(171, 3);
			this.bntBullk.Name = "bntBullk";
			this.bntBullk.Size = new System.Drawing.Size(587, 23);
			this.bntBullk.TabIndex = 4;
			this.bntBullk.Text = "Start Processing";
			this.bntBullk.UseVisualStyleBackColor = true;
			this.bntBullk.Click += new System.EventHandler(this.bntBullk_Click);
			// 
			// pbProcessFTPData
			// 
			this.pbProcessFTPData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbProcessFTPData.Location = new System.Drawing.Point(171, 32);
			this.pbProcessFTPData.Name = "pbProcessFTPData";
			this.pbProcessFTPData.Size = new System.Drawing.Size(448, 13);
			this.pbProcessFTPData.TabIndex = 3;
			// 
			// txtProcessData
			// 
			this.txtProcessData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessData.Location = new System.Drawing.Point(171, 51);
			this.txtProcessData.Name = "txtProcessData";
			this.txtProcessData.ReadOnly = true;
			this.txtProcessData.Size = new System.Drawing.Size(586, 358);
			this.txtProcessData.TabIndex = 2;
			this.txtProcessData.Text = "";
			// 
			// checkBoxDistributors
			// 
			this.checkBoxDistributors.AutoSize = true;
			this.checkBoxDistributors.Enabled = false;
			this.checkBoxDistributors.Location = new System.Drawing.Point(6, 245);
			this.checkBoxDistributors.Name = "checkBoxDistributors";
			this.checkBoxDistributors.Size = new System.Drawing.Size(92, 17);
			this.checkBoxDistributors.TabIndex = 59;
			this.checkBoxDistributors.Text = "Distributor List";
			this.checkBoxDistributors.UseVisualStyleBackColor = true;
			// 
			// progressBarDistributors
			// 
			this.progressBarDistributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarDistributors.Location = new System.Drawing.Point(117, 245);
			this.progressBarDistributors.Name = "progressBarDistributors";
			this.progressBarDistributors.Size = new System.Drawing.Size(396, 17);
			this.progressBarDistributors.TabIndex = 58;
			// 
			// labelFtpDlSizeDistributors
			// 
			this.labelFtpDlSizeDistributors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFtpDlSizeDistributors.AutoSize = true;
			this.labelFtpDlSizeDistributors.Location = new System.Drawing.Point(519, 246);
			this.labelFtpDlSizeDistributors.Name = "labelFtpDlSizeDistributors";
			this.labelFtpDlSizeDistributors.Size = new System.Drawing.Size(157, 13);
			this.labelFtpDlSizeDistributors.TabIndex = 57;
			this.labelFtpDlSizeDistributors.Text = "000 Mb of 000 Mb | 00.00 Mb/s";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(771, 441);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "MyMDb (by: Paw Jershauge)";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.gbConnectionError.ResumeLayout(false);
			this.gbConnectionError.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button bntTestSqlServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbIntegratedSecurity;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox gbConnectionError;
        private System.Windows.Forms.TextBox txtConnectionError;
        private System.Windows.Forms.Button bntBuildDB;
        private System.Windows.Forms.RichTextBox txtBuildDB;
        private System.Windows.Forms.Label lblFTPFrom;
        private System.Windows.Forms.ComboBox cbIMDBInterfaces;
        private System.Windows.Forms.RichTextBox txtProcessData;
        private System.Windows.Forms.Button bntBrowseLocalFolder;
        private System.Windows.Forms.TextBox txtBrowseLocalFolder;
        private System.Windows.Forms.Label lblLocalFolder;
        private System.Windows.Forms.Button bntDownloadData;
        private System.Windows.Forms.ProgressBar pb_Mov;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Mov;
        private System.Windows.Forms.CheckBox cbFtp_Mov;
        private System.Windows.Forms.CheckBox cbFtp_Acs;
        private System.Windows.Forms.ProgressBar pb_Acs;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Acs;
        private System.Windows.Forms.CheckBox cbFtp_Act;
        private System.Windows.Forms.ProgressBar pb_Act;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Act;
        private System.Windows.Forms.CheckBox cbFtp_Aka;
        private System.Windows.Forms.ProgressBar pb_Aka;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Aka;
        private System.Windows.Forms.CheckBox cbFtp_Cou;
        private System.Windows.Forms.ProgressBar pb_Cou;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Cou;
        private System.Windows.Forms.CheckBox cbFtp_Tag;
        private System.Windows.Forms.ProgressBar pb_Tag;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Tag;
        private System.Windows.Forms.CheckBox cbFtp_Plo;
        private System.Windows.Forms.ProgressBar pb_Plo;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Plo;
        private System.Windows.Forms.CheckBox cbFtp_Gen;
        private System.Windows.Forms.ProgressBar pb_Gen;
        private System.Windows.Forms.Label lblFtpDLSizeSpeed_Gen;
        private System.Windows.Forms.CheckBox cbRemCompress;
        private System.Windows.Forms.Button bntBullk;
        private System.Windows.Forms.ProgressBar pbProcessFTPData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chklbSyncFiles;
        private System.Windows.Forms.Label lblProcessFTPData;
		private System.Windows.Forms.CheckBox checkBoxDistributors;
		private System.Windows.Forms.ProgressBar progressBarDistributors;
		private System.Windows.Forms.Label labelFtpDlSizeDistributors;
    }
}


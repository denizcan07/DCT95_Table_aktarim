
namespace test
{
    partial class Login
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.txtPassw = new DevExpress.XtraEditors.TextEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.lblctrlUserName = new DevExpress.XtraEditors.LabelControl();
            this.lblctrlPassword = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEdit1 = new DevExpress.XtraEditors.LookUpEdit();
            this.lblDbName = new DevExpress.XtraEditors.LabelControl();
            this.txtServer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.userLoginEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userLoginEntityBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.BackgroundImage = global::DCT95.Properties.Resources.OK24;
            this.simpleButton1.ImageOptions.Image = global::DCT95.Properties.Resources.OK24;
            this.simpleButton1.Location = new System.Drawing.Point(150, 273);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(32, 23);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(93, 170);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(112, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // txtPassw
            // 
            this.txtPassw.Location = new System.Drawing.Point(93, 196);
            this.txtPassw.Name = "txtPassw";
            this.txtPassw.Properties.UseSystemPasswordChar = true;
            this.txtPassw.Size = new System.Drawing.Size(112, 20);
            this.txtPassw.TabIndex = 2;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(193, 138);
            this.pictureEdit1.TabIndex = 3;
            // 
            // lblctrlUserName
            // 
            this.lblctrlUserName.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblctrlUserName.Appearance.Options.UseBackColor = true;
            this.lblctrlUserName.Location = new System.Drawing.Point(28, 173);
            this.lblctrlUserName.Name = "lblctrlUserName";
            this.lblctrlUserName.Size = new System.Drawing.Size(59, 13);
            this.lblctrlUserName.TabIndex = 4;
            this.lblctrlUserName.Text = "Kullanıcı Adı:";
            // 
            // lblctrlPassword
            // 
            this.lblctrlPassword.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblctrlPassword.Appearance.Options.UseBackColor = true;
            this.lblctrlPassword.Location = new System.Drawing.Point(58, 199);
            this.lblctrlPassword.Name = "lblctrlPassword";
            this.lblctrlPassword.Size = new System.Drawing.Size(29, 13);
            this.lblctrlPassword.TabIndex = 5;
            this.lblctrlPassword.Text = "Şifre :";
            // 
            // lookUpEdit1
            // 
            this.lookUpEdit1.Location = new System.Drawing.Point(93, 247);
            this.lookUpEdit1.Name = "lookUpEdit1";
            this.lookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit1.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DataBaseName", "DataBase Name")});
            this.lookUpEdit1.Properties.DataSource = this.userLoginEntityBindingSource;
            this.lookUpEdit1.Properties.DisplayMember = "DataBaseName";
            this.lookUpEdit1.Properties.NullText = "";
            this.lookUpEdit1.Properties.ValueMember = "RecId";
            this.lookUpEdit1.Size = new System.Drawing.Size(112, 20);
            this.lookUpEdit1.TabIndex = 6;
            this.lookUpEdit1.EditValueChanged += new System.EventHandler(this.lookUpEdit1_EditValueChanged);
            this.lookUpEdit1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lookUpEdit1_MouseClick);
            // 
            // lblDbName
            // 
            this.lblDbName.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblDbName.Appearance.Options.UseBackColor = true;
            this.lblDbName.Location = new System.Drawing.Point(4, 250);
            this.lblDbName.Name = "lblDbName";
            this.lblDbName.Size = new System.Drawing.Size(83, 13);
            this.lblDbName.TabIndex = 7;
            this.lblDbName.Text = "Database Name :";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(93, 223);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(112, 20);
            this.txtServer.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Location = new System.Drawing.Point(48, 226);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Server :";
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.Image = global::DCT95.Properties.Resources.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(181, 273);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(24, 23);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // userLoginEntityBindingSource
            // 
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(212, 320);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblDbName);
            this.Controls.Add(this.lookUpEdit1);
            this.Controls.Add(this.lblctrlPassword);
            this.Controls.Add(this.lblctrlUserName);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.txtPassw);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.simpleButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userLoginEntityBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.TextEdit txtPassw;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl lblctrlUserName;
        private DevExpress.XtraEditors.LabelControl lblctrlPassword;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit1;
        private DevExpress.XtraEditors.LabelControl lblDbName;
        private System.Windows.Forms.BindingSource userLoginEntityBindingSource;
        private DevExpress.XtraEditors.TextEdit txtServer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}


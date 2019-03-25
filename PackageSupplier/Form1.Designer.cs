namespace PackageSupplier
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
            this.PlcStateText = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.serverStateText = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.MessageListControl = new DevExpress.XtraEditors.ListBoxControl();
            this.CarInfoGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CarCacheGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageListControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarCacheGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlcStateText
            // 
            this.PlcStateText.AutoSize = true;
            this.PlcStateText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlcStateText.Location = new System.Drawing.Point(19, 18);
            this.PlcStateText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlcStateText.Name = "PlcStateText";
            this.PlcStateText.Size = new System.Drawing.Size(108, 21);
            this.PlcStateText.TabIndex = 1;
            this.PlcStateText.Text = "PLC 断开连接";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.serverStateText);
            this.panelControl1.Controls.Add(this.PlcStateText);
            this.panelControl1.Location = new System.Drawing.Point(22, 48);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(552, 90);
            this.panelControl1.TabIndex = 2;
            // 
            // serverStateText
            // 
            this.serverStateText.AutoSize = true;
            this.serverStateText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverStateText.Location = new System.Drawing.Point(19, 54);
            this.serverStateText.Name = "serverStateText";
            this.serverStateText.Size = new System.Drawing.Size(122, 21);
            this.serverStateText.TabIndex = 2;
            this.serverStateText.Text = "服务器断开连接";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.MessageListControl);
            this.groupControl1.Controls.Add(this.CarInfoGrid);
            this.groupControl1.Controls.Add(this.CarCacheGrid);
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Location = new System.Drawing.Point(-1, 1);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1119, 618);
            this.groupControl1.TabIndex = 6;
            // 
            // MessageListControl
            // 
            this.MessageListControl.Location = new System.Drawing.Point(23, 319);
            this.MessageListControl.Name = "MessageListControl";
            this.MessageListControl.Size = new System.Drawing.Size(551, 277);
            this.MessageListControl.TabIndex = 3;
            // 
            // CarInfoGrid
            // 
            this.CarInfoGrid.Location = new System.Drawing.Point(589, 48);
            this.CarInfoGrid.MainView = this.gridView2;
            this.CarInfoGrid.Name = "CarInfoGrid";
            this.CarInfoGrid.Size = new System.Drawing.Size(517, 548);
            this.CarInfoGrid.TabIndex = 7;
            this.CarInfoGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.CarInfoGrid;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // CarCacheGrid
            // 
            this.CarCacheGrid.Location = new System.Drawing.Point(22, 153);
            this.CarCacheGrid.MainView = this.gridView1;
            this.CarCacheGrid.Name = "CarCacheGrid";
            this.CarCacheGrid.Size = new System.Drawing.Size(552, 160);
            this.CarCacheGrid.TabIndex = 6;
            this.CarCacheGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.CarCacheGrid;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // Form1
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 619);
            this.Controls.Add(this.groupControl1);
            this.Font = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "智能供包台";
            this.Load += new System.EventHandler(this.FormLoaded);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MessageListControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarInfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CarCacheGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label PlcStateText;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label serverStateText;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl CarInfoGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.GridControl CarCacheGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.ListBoxControl MessageListControl;
    }
}


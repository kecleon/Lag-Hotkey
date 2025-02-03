namespace LagKey
{
	partial class KeyForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyForm));
			this.txtKey = new System.Windows.Forms.TextBox();
			this.lblKey = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtKey
			// 
			this.txtKey.Location = new System.Drawing.Point(44, 6);
			this.txtKey.Name = "txtKey";
			this.txtKey.ReadOnly = true;
			this.txtKey.Size = new System.Drawing.Size(72, 20);
			this.txtKey.TabIndex = 0;
			this.txtKey.Text = "F4";
			this.txtKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblKey
			// 
			this.lblKey.ForeColor = System.Drawing.Color.White;
			this.lblKey.Location = new System.Drawing.Point(12, 9);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(30, 17);
			this.lblKey.TabIndex = 1;
			this.lblKey.Text = "Key:";
			// 
			// lblStatus
			// 
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.ForeColor = System.Drawing.Color.Red;
			this.lblStatus.Location = new System.Drawing.Point(122, 21);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(36, 17);
			this.lblStatus.TabIndex = 2;
			this.lblStatus.Text = "OFF";
			// 
			// lblPort
			// 
			this.lblPort.ForeColor = System.Drawing.Color.White;
			this.lblPort.Location = new System.Drawing.Point(12, 32);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(30, 19);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port:";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(44, 29);
			this.txtPort.MaxLength = 5;
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(72, 20);
			this.txtPort.TabIndex = 4;
			this.txtPort.Text = KeyForm.Port.ToString();
			this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
			// 
			// KeyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(170, 57);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblKey);
			this.Controls.Add(this.txtKey);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KeyForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lag Hotkey";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;

		private System.Windows.Forms.Label lblStatus;

		private System.Windows.Forms.TextBox txtKey;
		private System.Windows.Forms.Label lblKey;

		#endregion
	}
}
namespace paiza
{
    partial class MainFrame
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.startbutton = new System.Windows.Forms.Button();
            this.singlecompany = new System.Windows.Forms.Button();
            this.dbbutton = new System.Windows.Forms.Button();
            this.CompanyprogressBar = new System.Windows.Forms.ProgressBar();
            this.offerbutton = new System.Windows.Forms.Button();
            this.status_txt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startbutton
            // 
            this.startbutton.Location = new System.Drawing.Point(227, 3);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(218, 50);
            this.startbutton.TabIndex = 1;
            this.startbutton.Text = "2.公司数目检验";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // singlecompany
            // 
            this.singlecompany.Location = new System.Drawing.Point(451, 3);
            this.singlecompany.Name = "singlecompany";
            this.singlecompany.Size = new System.Drawing.Size(218, 50);
            this.singlecompany.TabIndex = 2;
            this.singlecompany.Text = "3.GO";
            this.singlecompany.UseVisualStyleBackColor = true;
            this.singlecompany.Click += new System.EventHandler(this.singlecompany_Click);
            // 
            // dbbutton
            // 
            this.dbbutton.Location = new System.Drawing.Point(3, 3);
            this.dbbutton.Name = "dbbutton";
            this.dbbutton.Size = new System.Drawing.Size(218, 50);
            this.dbbutton.TabIndex = 3;
            this.dbbutton.Text = "1.初始化";
            this.dbbutton.UseVisualStyleBackColor = true;
            this.dbbutton.Click += new System.EventHandler(this.dbbutton_Click);
            // 
            // CompanyprogressBar
            // 
            this.CompanyprogressBar.Location = new System.Drawing.Point(1, 206);
            this.CompanyprogressBar.Name = "CompanyprogressBar";
            this.CompanyprogressBar.Size = new System.Drawing.Size(679, 23);
            this.CompanyprogressBar.TabIndex = 4;
            // 
            // offerbutton
            // 
            this.offerbutton.Location = new System.Drawing.Point(3, 59);
            this.offerbutton.Name = "offerbutton";
            this.offerbutton.Size = new System.Drawing.Size(218, 52);
            this.offerbutton.TabIndex = 5;
            this.offerbutton.Text = "4.更新OFFER";
            this.offerbutton.UseVisualStyleBackColor = true;
            this.offerbutton.Click += new System.EventHandler(this.offerbutton_Click);
            // 
            // status_txt
            // 
            this.status_txt.AutoSize = true;
            this.status_txt.Location = new System.Drawing.Point(3, 190);
            this.status_txt.Name = "status_txt";
            this.status_txt.Size = new System.Drawing.Size(0, 12);
            this.status_txt.TabIndex = 6;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 227);
            this.Controls.Add(this.status_txt);
            this.Controls.Add(this.offerbutton);
            this.Controls.Add(this.CompanyprogressBar);
            this.Controls.Add(this.dbbutton);
            this.Controls.Add(this.singlecompany);
            this.Controls.Add(this.startbutton);
            this.Name = "MainFrame";
            this.Text = "Paizaデータ取得";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbutton;
        private System.Windows.Forms.Button singlecompany;
        private System.Windows.Forms.Button dbbutton;
        private System.Windows.Forms.ProgressBar CompanyprogressBar;
        private System.Windows.Forms.Button offerbutton;
        private System.Windows.Forms.Label status_txt;
    }
}


namespace Panzer_Front_Tool
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.unpTex = new System.Windows.Forms.Button();
            this.expModels = new System.Windows.Forms.Button();
            this.checkPS1 = new System.Windows.Forms.RadioButton();
            this.checkDC = new System.Windows.Forms.RadioButton();
            this.dialogPS1 = new System.Windows.Forms.OpenFileDialog();
            this.dialogDC = new System.Windows.Forms.OpenFileDialog();
            this.dialogPVR = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // unpTex
            // 
            this.unpTex.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.unpTex.Location = new System.Drawing.Point(12, 12);
            this.unpTex.Name = "unpTex";
            this.unpTex.Size = new System.Drawing.Size(204, 40);
            this.unpTex.TabIndex = 0;
            this.unpTex.Text = "Unpack Textures";
            this.unpTex.UseVisualStyleBackColor = true;
            this.unpTex.Click += new System.EventHandler(this.button1_Click);
            // 
            // expModels
            // 
            this.expModels.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.expModels.Location = new System.Drawing.Point(12, 58);
            this.expModels.Name = "expModels";
            this.expModels.Size = new System.Drawing.Size(204, 40);
            this.expModels.TabIndex = 1;
            this.expModels.Text = "Export models as .obj";
            this.expModels.UseVisualStyleBackColor = true;
            this.expModels.Click += new System.EventHandler(this.expModels_Click);
            // 
            // checkPS1
            // 
            this.checkPS1.AutoSize = true;
            this.checkPS1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPS1.Checked = true;
            this.checkPS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkPS1.Location = new System.Drawing.Point(60, 104);
            this.checkPS1.Name = "checkPS1";
            this.checkPS1.Size = new System.Drawing.Size(54, 20);
            this.checkPS1.TabIndex = 2;
            this.checkPS1.TabStop = true;
            this.checkPS1.Text = "PS1";
            this.checkPS1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkPS1.UseVisualStyleBackColor = true;
            // 
            // checkDC
            // 
            this.checkDC.AutoSize = true;
            this.checkDC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkDC.Location = new System.Drawing.Point(120, 104);
            this.checkDC.Name = "checkDC";
            this.checkDC.Size = new System.Drawing.Size(47, 20);
            this.checkDC.TabIndex = 3;
            this.checkDC.Text = "DC";
            this.checkDC.UseVisualStyleBackColor = true;
            // 
            // dialogPS1
            // 
            this.dialogPS1.FileName = "TK.D3";
            this.dialogPS1.Filter = "TK.D3|TK.D3";
            // 
            // dialogDC
            // 
            this.dialogDC.FileName = "*.NTD";
            this.dialogDC.Filter = "DC model|*.NTD";
            // 
            // dialogPVR
            // 
            this.dialogPVR.FileName = "*.pvr";
            this.dialogPVR.Filter = "PVR|*.pvr";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 133);
            this.Controls.Add(this.checkDC);
            this.Controls.Add(this.checkPS1);
            this.Controls.Add(this.expModels);
            this.Controls.Add(this.unpTex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PZ Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button unpTex;
        private System.Windows.Forms.Button expModels;
        private System.Windows.Forms.RadioButton checkPS1;
        private System.Windows.Forms.RadioButton checkDC;
        private System.Windows.Forms.OpenFileDialog dialogPS1;
        private System.Windows.Forms.OpenFileDialog dialogDC;
        private System.Windows.Forms.OpenFileDialog dialogPVR;
    }
}


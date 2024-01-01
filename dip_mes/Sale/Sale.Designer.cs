namespace dip_mes
{
    partial class Sale
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.delRow = new System.Windows.Forms.Button();
            this.addRow = new System.Windows.Forms.Button();
            this.RegButton2 = new System.Windows.Forms.Button();
            this.RegButton1 = new System.Windows.Forms.Button();
            this.CheckButton1 = new System.Windows.Forms.Button();
            this.saledate = new System.Windows.Forms.DateTimePicker();
            this.findNo = new System.Windows.Forms.TextBox();
            this.salecode = new System.Windows.Forms.TextBox();
            this.buyername = new System.Windows.Forms.ComboBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // delRow
            // 
            this.delRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(73)))), ((int)(((byte)(153)))));
            this.delRow.FlatAppearance.BorderSize = 0;
            this.delRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delRow.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delRow.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.delRow.Location = new System.Drawing.Point(1375, 762);
            this.delRow.Name = "delRow";
            this.delRow.Size = new System.Drawing.Size(136, 34);
            this.delRow.TabIndex = 61;
            this.delRow.Text = "행삭제";
            this.delRow.UseVisualStyleBackColor = false;
            this.delRow.Visible = false;
            this.delRow.Click += new System.EventHandler(this.delRow_Click);
            // 
            // addRow
            // 
            this.addRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(73)))), ((int)(((byte)(153)))));
            this.addRow.FlatAppearance.BorderSize = 0;
            this.addRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRow.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addRow.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.addRow.Location = new System.Drawing.Point(1226, 763);
            this.addRow.Name = "addRow";
            this.addRow.Size = new System.Drawing.Size(136, 34);
            this.addRow.TabIndex = 60;
            this.addRow.Text = "행추가";
            this.addRow.UseVisualStyleBackColor = false;
            this.addRow.Visible = false;
            this.addRow.Click += new System.EventHandler(this.addRow_Click);
            // 
            // RegButton2
            // 
            this.RegButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(73)))), ((int)(((byte)(153)))));
            this.RegButton2.FlatAppearance.BorderSize = 0;
            this.RegButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RegButton2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegButton2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.RegButton2.Location = new System.Drawing.Point(1525, 761);
            this.RegButton2.Name = "RegButton2";
            this.RegButton2.Size = new System.Drawing.Size(136, 34);
            this.RegButton2.TabIndex = 59;
            this.RegButton2.Text = "등록";
            this.RegButton2.UseVisualStyleBackColor = false;
            this.RegButton2.Visible = false;
            this.RegButton2.Click += new System.EventHandler(this.RegButton2_Click);
            // 
            // RegButton1
            // 
            this.RegButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(73)))), ((int)(((byte)(153)))));
            this.RegButton1.FlatAppearance.BorderSize = 0;
            this.RegButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RegButton1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegButton1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.RegButton1.Location = new System.Drawing.Point(1478, 33);
            this.RegButton1.Name = "RegButton1";
            this.RegButton1.Size = new System.Drawing.Size(136, 34);
            this.RegButton1.TabIndex = 58;
            this.RegButton1.Text = "등록";
            this.RegButton1.UseVisualStyleBackColor = false;
            this.RegButton1.Click += new System.EventHandler(this.RegButton1_Click);
            // 
            // CheckButton1
            // 
            this.CheckButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(73)))), ((int)(((byte)(153)))));
            this.CheckButton1.FlatAppearance.BorderSize = 0;
            this.CheckButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckButton1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckButton1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CheckButton1.Location = new System.Drawing.Point(1523, 55);
            this.CheckButton1.Name = "CheckButton1";
            this.CheckButton1.Size = new System.Drawing.Size(136, 34);
            this.CheckButton1.TabIndex = 57;
            this.CheckButton1.Text = "조회";
            this.CheckButton1.UseVisualStyleBackColor = false;
            this.CheckButton1.Click += new System.EventHandler(this.CheckButton1_Click);
            // 
            // saledate
            // 
            this.saledate.CustomFormat = "yy-MM-dd HH:mm";
            this.saledate.Font = new System.Drawing.Font("굴림", 16F);
            this.saledate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.saledate.Location = new System.Drawing.Point(44, 36);
            this.saledate.Name = "saledate";
            this.saledate.Size = new System.Drawing.Size(330, 32);
            this.saledate.TabIndex = 56;
            // 
            // findNo
            // 
            this.findNo.Font = new System.Drawing.Font("굴림", 16F);
            this.findNo.Location = new System.Drawing.Point(140, 58);
            this.findNo.Name = "findNo";
            this.findNo.Size = new System.Drawing.Size(250, 32);
            this.findNo.TabIndex = 54;
            this.findNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // salecode
            // 
            this.salecode.Font = new System.Drawing.Font("굴림", 16F);
            this.salecode.Location = new System.Drawing.Point(628, 36);
            this.salecode.Name = "salecode";
            this.salecode.Size = new System.Drawing.Size(250, 32);
            this.salecode.TabIndex = 55;
            // 
            // buyername
            // 
            this.buyername.Font = new System.Drawing.Font("굴림", 16F);
            this.buyername.FormattingEnabled = true;
            this.buyername.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.buyername.ItemHeight = 21;
            this.buyername.Location = new System.Drawing.Point(1132, 36);
            this.buyername.Name = "buyername";
            this.buyername.Size = new System.Drawing.Size(177, 29);
            this.buyername.TabIndex = 53;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeight = 35;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView2.Location = new System.Drawing.Point(16, 561);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1643, 183);
            this.dataGridView2.TabIndex = 52;
            this.dataGridView2.Visible = false;
            this.dataGridView2.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView2_CellValidating);
            this.dataGridView2.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_ColumnHeaderMouseClick);
            this.dataGridView2.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView2_DataError);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(16, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1643, 308);
            this.dataGridView1.TabIndex = 51;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(522, 761);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 24);
            this.label10.TabIndex = 50;
            this.label10.Text = "0";
            this.label10.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(378, 761);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 24);
            this.label9.TabIndex = 49;
            this.label9.Text = "총부가세";
            this.label9.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(190, 761);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 24);
            this.label4.TabIndex = 48;
            this.label4.Text = "0";
            this.label4.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(16, 761);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 47;
            this.label2.Text = "총판매금액";
            this.label2.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 16F);
            this.label6.Location = new System.Drawing.Point(19, 336);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 22);
            this.label6.TabIndex = 46;
            this.label6.Text = "등록일자";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 16F);
            this.label3.Location = new System.Drawing.Point(499, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 22);
            this.label3.TabIndex = 45;
            this.label3.Text = "판매번호";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 16F);
            this.label7.Location = new System.Drawing.Point(1030, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 22);
            this.label7.TabIndex = 44;
            this.label7.Text = "고객명";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 16F);
            this.label8.Location = new System.Drawing.Point(17, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 22);
            this.label8.TabIndex = 43;
            this.label8.Text = "판매번호 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 27);
            this.label1.TabIndex = 42;
            this.label1.Text = "판매관리";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(16, 530);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 24);
            this.label5.TabIndex = 42;
            this.label5.Text = "판매세부사항";
            this.label5.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RegButton1);
            this.groupBox1.Controls.Add(this.saledate);
            this.groupBox1.Controls.Add(this.salecode);
            this.groupBox1.Controls.Add(this.buyername);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(16, 419);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1643, 89);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "등록";
            // 
            // Sale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.delRow);
            this.Controls.Add(this.addRow);
            this.Controls.Add(this.RegButton2);
            this.Controls.Add(this.CheckButton1);
            this.Controls.Add(this.findNo);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Name = "Sale";
            this.Size = new System.Drawing.Size(1673, 826);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button delRow;
        private System.Windows.Forms.Button addRow;
        private System.Windows.Forms.Button RegButton2;
        private System.Windows.Forms.Button RegButton1;
        private System.Windows.Forms.Button CheckButton1;
        private System.Windows.Forms.DateTimePicker saledate;
        private System.Windows.Forms.TextBox findNo;
        private System.Windows.Forms.TextBox salecode;
        private System.Windows.Forms.ComboBox buyername;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

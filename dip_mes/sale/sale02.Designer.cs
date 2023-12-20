namespace dip_mes
{
    partial class sale02
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
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // delRow
            // 
            this.delRow.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.delRow.Location = new System.Drawing.Point(1375, 618);
            this.delRow.Name = "delRow";
            this.delRow.Size = new System.Drawing.Size(134, 35);
            this.delRow.TabIndex = 61;
            this.delRow.Text = "행삭제";
            this.delRow.UseVisualStyleBackColor = true;
            this.delRow.Visible = false;
            this.delRow.Click += new System.EventHandler(this.delRow_Click);
            // 
            // addRow
            // 
            this.addRow.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.addRow.Location = new System.Drawing.Point(1226, 619);
            this.addRow.Name = "addRow";
            this.addRow.Size = new System.Drawing.Size(134, 35);
            this.addRow.TabIndex = 60;
            this.addRow.Text = "행추가";
            this.addRow.UseVisualStyleBackColor = true;
            this.addRow.Visible = false;
            this.addRow.Click += new System.EventHandler(this.addRow_Click);
            // 
            // RegButton2
            // 
            this.RegButton2.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RegButton2.Location = new System.Drawing.Point(1525, 617);
            this.RegButton2.Name = "RegButton2";
            this.RegButton2.Size = new System.Drawing.Size(134, 35);
            this.RegButton2.TabIndex = 59;
            this.RegButton2.Text = "등록";
            this.RegButton2.UseVisualStyleBackColor = true;
            this.RegButton2.Visible = false;
            this.RegButton2.Click += new System.EventHandler(this.RegButton2_Click);
            // 
            // RegButton1
            // 
            this.RegButton1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RegButton1.Location = new System.Drawing.Point(1524, 374);
            this.RegButton1.Name = "RegButton1";
            this.RegButton1.Size = new System.Drawing.Size(134, 35);
            this.RegButton1.TabIndex = 58;
            this.RegButton1.Text = "등록";
            this.RegButton1.UseVisualStyleBackColor = true;
            this.RegButton1.Click += new System.EventHandler(this.RegButton1_Click);
            // 
            // CheckButton1
            // 
            this.CheckButton1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CheckButton1.Location = new System.Drawing.Point(1525, 100);
            this.CheckButton1.Name = "CheckButton1";
            this.CheckButton1.Size = new System.Drawing.Size(134, 35);
            this.CheckButton1.TabIndex = 57;
            this.CheckButton1.Text = "조회";
            this.CheckButton1.UseVisualStyleBackColor = true;
            this.CheckButton1.Click += new System.EventHandler(this.CheckButton1_Click);
            // 
            // saledate
            // 
            this.saledate.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.saledate.Location = new System.Drawing.Point(134, 376);
            this.saledate.Name = "saledate";
            this.saledate.Size = new System.Drawing.Size(330, 35);
            this.saledate.TabIndex = 56;
            // 
            // findNo
            // 
            this.findNo.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.findNo.Location = new System.Drawing.Point(134, 108);
            this.findNo.Name = "findNo";
            this.findNo.Size = new System.Drawing.Size(121, 35);
            this.findNo.TabIndex = 54;
            // 
            // salecode
            // 
            this.salecode.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.salecode.Location = new System.Drawing.Point(711, 376);
            this.salecode.Name = "salecode";
            this.salecode.Size = new System.Drawing.Size(121, 35);
            this.salecode.TabIndex = 55;
            // 
            // buyername
            // 
            this.buyername.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buyername.FormattingEnabled = true;
            this.buyername.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.buyername.ItemHeight = 24;
            this.buyername.Items.AddRange(new object[] {
            "탕수육",
            "짜장면",
            "짬뽕",
            "우육면"});
            this.buyername.Location = new System.Drawing.Point(1183, 377);
            this.buyername.Name = "buyername";
            this.buyername.Size = new System.Drawing.Size(177, 32);
            this.buyername.TabIndex = 53;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(15, 417);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1643, 183);
            this.dataGridView2.TabIndex = 52;
            this.dataGridView2.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1643, 204);
            this.dataGridView1.TabIndex = 51;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(522, 619);
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
            this.label9.Location = new System.Drawing.Point(378, 618);
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
            this.label4.Location = new System.Drawing.Point(190, 619);
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
            this.label2.Location = new System.Drawing.Point(22, 617);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 47;
            this.label2.Text = "총판매금액";
            this.label2.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(22, 380);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 24);
            this.label6.TabIndex = 46;
            this.label6.Text = "등록일자";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(599, 380);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 45;
            this.label3.Text = "판매번호";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(1095, 383);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 24);
            this.label7.TabIndex = 44;
            this.label7.Text = "고객명";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(22, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 24);
            this.label8.TabIndex = 43;
            this.label8.Text = "판매번호";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(22, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 42;
            this.label1.Text = "판매관리";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(1385, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 35);
            this.button1.TabIndex = 57;
            this.button1.Text = "저장";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CheckButton1_Click);
            // 
            // sale02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.delRow);
            this.Controls.Add(this.addRow);
            this.Controls.Add(this.RegButton2);
            this.Controls.Add(this.RegButton1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CheckButton1);
            this.Controls.Add(this.saledate);
            this.Controls.Add(this.findNo);
            this.Controls.Add(this.salecode);
            this.Controls.Add(this.buyername);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Name = "sale02";
            this.Size = new System.Drawing.Size(1673, 826);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.Button button1;
    }
}

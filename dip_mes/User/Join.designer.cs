﻿namespace dip_mes
{
    partial class Join
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
            this.txtbox_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbox_id = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbox_pwd = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.number = new System.Windows.Forms.TextBox();
            this.Department = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtbox_name
            // 
            this.txtbox_name.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtbox_name.Location = new System.Drawing.Point(277, 88);
            this.txtbox_name.Name = "txtbox_name";
            this.txtbox_name.Size = new System.Drawing.Size(218, 29);
            this.txtbox_name.TabIndex = 0;
            this.txtbox_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(205, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "이름 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(227, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "ID :";
            // 
            // txtbox_id
            // 
            this.txtbox_id.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtbox_id.Location = new System.Drawing.Point(277, 150);
            this.txtbox_id.Name = "txtbox_id";
            this.txtbox_id.Size = new System.Drawing.Size(218, 29);
            this.txtbox_id.TabIndex = 3;
            this.txtbox_id.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(213, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "PW :";
            // 
            // txtbox_pwd
            // 
            this.txtbox_pwd.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtbox_pwd.Location = new System.Drawing.Point(277, 215);
            this.txtbox_pwd.Name = "txtbox_pwd";
            this.txtbox_pwd.Size = new System.Drawing.Size(218, 29);
            this.txtbox_pwd.TabIndex = 5;
            this.txtbox_pwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(289, 501);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 42);
            this.button1.TabIndex = 13;
            this.button1.Text = "등록";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(199, 406);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 21);
            this.label4.TabIndex = 19;
            this.label4.Text = "부서 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(178, 342);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 21);
            this.label5.TabIndex = 17;
            this.label5.Text = "이메일 :";
            // 
            // email
            // 
            this.email.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.email.Location = new System.Drawing.Point(277, 338);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(218, 29);
            this.email.TabIndex = 16;
            this.email.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(157, 280);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 21);
            this.label6.TabIndex = 15;
            this.label6.Text = "주민번호 :";
            // 
            // number
            // 
            this.number.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.number.Location = new System.Drawing.Point(277, 276);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(218, 29);
            this.number.TabIndex = 14;
            this.number.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_KeyDown);
            // 
            // Department
            // 
            this.Department.Font = new System.Drawing.Font("굴림", 14.25F);
            this.Department.FormattingEnabled = true;
            this.Department.Items.AddRange(new object[] {
            "영업부",
            "생산부"});
            this.Department.Location = new System.Drawing.Point(232, 404);
            this.Department.Name = "Department";
            this.Department.Size = new System.Drawing.Size(218, 27);
            this.Department.TabIndex = 20;
            // 
            // Join
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 658);
            this.Controls.Add(this.Department);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.email);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.number);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbox_pwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbox_id);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbox_name);
            this.Name = "Join";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbox_id;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbox_pwd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox number;
        private System.Windows.Forms.ComboBox Department;
    }
}
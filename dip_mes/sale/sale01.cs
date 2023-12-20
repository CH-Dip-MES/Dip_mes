﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class sale01 : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        public sale01()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) //Req5-2
        {
            if (ItemStatus.SelectedItem != null || ItemNo.SelectedItem != null || ItemName.SelectedItem != null || 
                ItemAmount.Text != "" || RegistDate.Value == DateTimePicker.MinimumDateTime || Inven.SelectedItem != null)
            {
                int amount = int.Parse(ItemAmount.Text);
                using (MySqlConnection iConn = new MySqlConnection(jConn))
                {
                    iConn.Open();
                    MySqlCommand msc = new MySqlCommand("insert into sale1(ItemStat, ItemNo, ItemName, ItemAmount, RegistDate, Inven) values(@ItemStat, @ItemNo, @ItemName, @ItemAmount, @RegistDate, @Inven)", iConn);
                    msc.Parameters.AddWithValue("@ItemStat", ItemStatus.SelectedItem.ToString());
                    msc.Parameters.AddWithValue("@ItemNo", ItemNo.SelectedItem.ToString());
                    msc.Parameters.AddWithValue("@ItemName", ItemName.SelectedItem.ToString());
                    msc.Parameters.AddWithValue("@ItemAmount", amount);
                    msc.Parameters.AddWithValue("@RegistDate", RegistDate.Value);
                    msc.Parameters.AddWithValue("@Inven", Inven.SelectedItem.ToString());
                    msc.ExecuteNonQuery();
                }
            }
            else 
            {
                MessageBox.Show("기입되지 않은 필수 항목이 있습니다");
            }
        }

        private void button1_Click(object sender, EventArgs e) //Req5-1
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {  
                sConn.Open();
                string searchItemNo = Search.Text.Trim(); // 검색창 텍스트
                string fItem = "select ItemStat,ItemNo,ItemName,ItemAmount,RegistDate,Inven from sale1";
                if (!string.IsNullOrEmpty(searchItemNo)) // 검색창에 입력한 문자 있을 시 활성화 없으면 위의 fItem 문구 그대로
                {
                    fItem += $" WHERE ItemNo = '{searchItemNo}'";
                }
                string camount = "SELECT ItemNo,ItemName, SUM(CASE WHEN ItemStat = '입고' THEN ItemAmount ELSE -ItemAmount END) " +
                    "AS CurrentStock,Inven FROM sale1 GROUP BY ItemNo,ItemName,Inven";
                MySqlCommand cmd = new MySqlCommand(fItem, sConn);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable fManage = new DataTable();
                        adapter.Fill(fManage);

                        // Check if any rows are returned
                        if (fManage.Rows.Count > 0)
                        {
                            // DataGridView에 데이터 설정
                            dataGridView1.DataSource = fManage;

                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView1.Columns["ItemStat"].HeaderText = "상태";
                            dataGridView1.Columns["ItemNo"].HeaderText = "품번";
                            dataGridView1.Columns["ItemName"].HeaderText = "품명";
                            dataGridView1.Columns["ItemAmount"].HeaderText = "수량";
                            dataGridView1.Columns["RegistDate"].HeaderText = "등록일자";
                            dataGridView1.Columns["Inven"].HeaderText = "창고";
                        }
                        else
                        {
                            MessageBox.Show("해당 품번의 데이터가 없습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
                MySqlCommand cur = new MySqlCommand(camount, sConn);
                using (MySqlDataAdapter cadapter = new MySqlDataAdapter(cur))
                {
                    DataTable cManage = new DataTable();
                    cadapter.Fill(cManage);
                    dataGridView2.DataSource = cManage;
                    dataGridView2.Columns["ItemNo"].HeaderText = "품번";
                    dataGridView2.Columns["ItemName"].HeaderText = "품명";
                    dataGridView2.Columns["CurrentStock"].HeaderText = "현재고";
                    dataGridView2.Columns["Inven"].HeaderText = "창고";
                }
            }
        }
    }
}

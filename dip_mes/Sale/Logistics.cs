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
    public partial class Logistics : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";

        private void LoadComboBoxItems() //품번 콤보박스 DB 연결
        {
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT product_code FROM product";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemNo.Items.Add(reader["product_code"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터 로드 중 오류 발생: {ex.Message}");
                }
            }
        }
        public Logistics()
        {
            InitializeComponent();
            LoadComboBoxItems();
        }
        private string GetNameDB(string itemNo) //품명 자동입력을 위한 DB 연결
        {
            string itemName = "";
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                conn.Open();
                string query = "SELECT product_name FROM product WHERE product_code = @ItemNo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemNo", itemNo);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        itemName = reader["product_name"].ToString();
                    }
                }
            }
            return itemName;
        }
        private void ItemNoChanged(object sender, EventArgs e) //품번 대응 품명 자동입력
        {
            if (ItemNo.SelectedItem != null)
            {
                string selectedNo = ItemNo.SelectedItem.ToString();
                string itemName = GetNameDB(selectedNo);
                ItemName.Text = itemName;
            }
        }
        private void button2_Click(object sender, EventArgs e) //Req5-2
        {
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            if (ItemStatus.SelectedItem != null && ItemNo.SelectedItem != null && ItemName.Text != "" && ItemAmount.Text != "" && RegistDate.Value != DateTimePicker.MinimumDateTime && Inven.SelectedItem != null)
            {
                int currentStockAmount = 0; // 현재 재고 수량
                int requestedAmount = int.Parse(ItemAmount.Text); // 사용자가 요청한 수량
                string selectedInven = Inven.SelectedItem.ToString(); // 선택된 창고

                using (MySqlConnection iConn = new MySqlConnection(jConn))
                {
                    iConn.Open();
                    // 해당 품번의 현재 입고된 수량을 창고별로 조회
                    MySqlCommand checkAmountCmd = new MySqlCommand("SELECT SUM(ItemAmount) FROM sale1 WHERE ItemNo = @ItemNo AND ItemStat = '입고' AND Inven = @Inven", iConn);
                    checkAmountCmd.Parameters.AddWithValue("@ItemNo", ItemNo.SelectedItem.ToString());
                    checkAmountCmd.Parameters.AddWithValue("@Inven", selectedInven);
                    object checkre = checkAmountCmd.ExecuteScalar();
                    currentStockAmount = Convert.IsDBNull(checkre) ? 0 : Convert.ToInt32(checkre);
                }
                if (requestedAmount <= currentStockAmount)
                {
                    // 입력된 데이터를 메시지박스에 표시
                    string message = $"상태: {ItemStatus.SelectedItem}\n품번: {ItemNo.SelectedItem}\n품명: {ItemName.Text}\n수량: {ItemAmount.Text}\n등록일자: {RegistDate.Value}\n창고: {Inven.SelectedItem}";
                    string caption = "등록 현황";
                    MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                    DialogResult result = MessageBox.Show(message, caption, buttons);

                    // 확인 버튼을 눌렀을 경우 데이터 등록
                    if (result == DialogResult.OK)
                    {
                        int amount = int.Parse(ItemAmount.Text);
                        using (MySqlConnection iConn = new MySqlConnection(jConn))
                        {
                            iConn.Open();
                            MySqlCommand msc = new MySqlCommand("insert into sale1(ItemStat, ItemNo, ItemName, ItemAmount, RegistDate, Inven) values(@ItemStat, @ItemNo, @ItemName, @ItemAmount, @RegistDate, @Inven)", iConn);
                            msc.Parameters.AddWithValue("@ItemStat", ItemStatus.SelectedItem.ToString());
                            msc.Parameters.AddWithValue("@ItemNo", ItemNo.SelectedItem.ToString());
                            msc.Parameters.AddWithValue("@ItemName", ItemName.Text);
                            msc.Parameters.AddWithValue("@ItemAmount", amount);
                            msc.Parameters.AddWithValue("@RegistDate", RegistDate.Value);
                            msc.Parameters.AddWithValue("@Inven", Inven.SelectedItem.ToString());
                            msc.ExecuteNonQuery();
                            MessageBox.Show("성공적으로 등록되었습니다.");
                            ItemStatus.SelectedItem = null;
                            ItemNo.SelectedItem = null;
                            ItemName.Text = "";
                            ItemAmount.Text = "";
                            RegistDate.Value = DateTimePicker.MinimumDateTime;
                            Inven.SelectedItem = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("등록이 취소되었습니다.");
                    }
                }
                else
                {
                    MessageBox.Show("출고 수량이 현재 창고의 재고량보다 많습니다");
                }
            }
            else
            {
                MessageBox.Show("기입되지 않은 필수 항목이 있습니다");
            }
        }
        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
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
                    fItem += $" WHERE ItemNo LIKE '%{searchItemNo}%'";
                }
                string camount = "SELECT ItemNo,ItemName, SUM(CASE WHEN ItemStat = '입고' THEN ItemAmount ELSE -ItemAmount END) " +
                    "AS CurrentStock,Inven FROM sale1 GROUP BY ItemNo,ItemName, Inven";
                MySqlCommand cmd = new MySqlCommand(fItem, sConn);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable fManage = new DataTable();
                        adapter.Fill(fManage);

                        if (fManage.Rows.Count > 0)
                        {
                            //  데이터 설정
                            dataGridView1.DataSource = fManage;

                            // 컬럼 헤더 텍스트 설정
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

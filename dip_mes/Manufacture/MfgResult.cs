﻿using dip_mes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class MfgResult : UserControl
    {
        private string connectionString = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private DateTime startDate;
        public MfgResult()
        {
            InitializeComponent();
            // dateTimePicker1 초기화
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Checked = false;
            LoadDataToDataGridView(false);
        }
       
        private void MfgResult_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadDataToDataGridView(bool selectedDate)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT OrderNo AS '작업번호', product_name AS '제품명', PlannedQty AS '생산수량', WorkStatus AS '작업상태', Operator AS '작업자', 
                                    StartTime AS '시작일자', FinishTime AS '완료일자', takeTime AS '소요시간', Lot FROM manufacture WHERE WorkStatus = '작업완료'";
                    
                    // 선택한 날짜가 있는 경우에만 날짜 조건을 추가
                    if (selectedDate)
                    {
                        query += " AND DATE(Duration) = @SelectedDate";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // 선택한 날짜가 있는 경우에만 파라미터 추가
                        if (selectedDate)
                        {
                            cmd.Parameters.AddWithValue("@SelectedDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        }

                        // 데이터를 담을 DataTable 생성
                        DataTable dataTable = new DataTable();

                        // MySQLDataAdapter를 사용하여 데이터 가져오기
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }

                        // DataGridView에 데이터 바인딩
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // 선택한 날짜 저장
            startDate = dateTimePicker1.Value;

            // 데이터 조회 및 그리드 갱신
            LoadDataToDataGridView(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 데이터 조회 및 그리드 갱신
            LoadDataToDataGridView(false);
        }
    }
}

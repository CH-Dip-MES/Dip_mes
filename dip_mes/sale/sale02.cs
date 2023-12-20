using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes
{
    public partial class sale02 : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        private Dictionary<int, string> selectedValues = new Dictionary<int, string>();
        public sale02()
        {
            InitializeComponent();
        }

        private void RegButton1_Click(object sender, EventArgs e)
        {
            if (saledate.Value == DateTimePicker.MinimumDateTime || salecode.Text != "" || buyername.SelectedItem != null)
            {
                using (MySqlConnection iConn = new MySqlConnection(jConn))
                {
                    iConn.Open();
                    MySqlCommand msc = new MySqlCommand("insert into sale2(saledate, salecode, buyername) values(@saledate, @salecode, @buyername)", iConn);
                    msc.Parameters.AddWithValue("@saledate", saledate.Value);
                    msc.Parameters.AddWithValue("@salecode", salecode.Text);
                    msc.Parameters.AddWithValue("@buyername", buyername.SelectedItem.ToString());

                    msc.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("기입되지 않은 필수 항목이 있습니다");
            }
        }

        private void CheckButton1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                string fSellNo = findNo.Text.Trim(); // 검색창 텍스트
                string sInfo = "select * from sale2";
                if (!string.IsNullOrEmpty(fSellNo)) // 검색창에 입력한 문자 있을 시 활성화 없으면 위의 fItem 문구 그대로
                {
                    sInfo += $" WHERE salecode = '{fSellNo}'";
                }
                MySqlCommand cmd = new MySqlCommand(sInfo, sConn);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable sManage = new DataTable();
                        adapter.Fill(sManage);

                        // Check if any rows are returned
                        if (sManage.Rows.Count > 0)
                        {
                            DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
                            comboColumn.DataPropertyName = "procstat";
                            comboColumn.HeaderText = "상태";
                            comboColumn.Name = "ItemStat";
                            comboColumn.Items.Add("입고");
                            comboColumn.Items.Add("출고");

                            // DataGridView에 데이터 설정
                            dataGridView1.DataSource = sManage;

                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView1.Columns["saledate"].HeaderText = "등록일자";
                            dataGridView1.Columns["salecode"].HeaderText = "판매번호";
                            dataGridView1.Columns["buyername"].HeaderText = "고객명";
                            dataGridView1.Columns["procstat"].HeaderText = "진행상태";
                            dataGridView1.Columns["delistat"].HeaderText = "납품상태";
                            dataGridView1.Columns["delidate"].HeaderText = "납품일자";
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
            }
        }
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                DataGridViewCell changedCell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (changedCell is DataGridViewComboBoxCell comboCell)
                {
                    // 추가로직: 콤보박스의 값이 "완료"일 때 Inven 열에 현재 날짜를 저장
                    if (comboCell.Value != null && comboCell.Value.ToString() == "완료")
                    {
                        dataGridView.Rows[e.RowIndex].Cells["delidate"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dataGridView.Rows[e.RowIndex].Cells["delidate"].Value = DBNull.Value;
                    }
                }
            }
        }
        
    }
}

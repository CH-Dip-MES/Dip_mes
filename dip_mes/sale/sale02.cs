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

namespace dip_mes.sale
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
                    MySqlCommand msc = new MySqlCommand("insert into sale2(saledate, salecode, buyername, delidate) values(@saledate, @salecode, @buyername)", iConn);
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
                string sInfo = "select saledate,salecode,buyername,procstat,delidate from sale2";

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
                            // DataGridView에 데이터 설정
                            dataGridView1.DataSource = sManage;
                        
                            // DataGridView 컬럼 헤더 텍스트 설정
                            dataGridView1.Columns["saledate"].HeaderText = "등록일자";
                            dataGridView1.Columns["salecode"].HeaderText = "판매번호";
                            dataGridView1.Columns["buyername"].HeaderText = "고객명";
                            dataGridView1.Columns["delidate"].HeaderText = "납품일자";

                            // 기존의 "진행상태" 열을 제거
                            if (dataGridView1.Columns.Contains("procstat"))
                                dataGridView1.Columns.Remove("procstat");

                            // 기존의 "납품상태" 열을 제거
                            if (dataGridView1.Columns.Contains("delistat"))
                                dataGridView1.Columns.Remove("delistat");

                            // "진행상태" 콤보박스 열 추가
                            DataGridViewComboBoxColumn comboColumnProcStat = new DataGridViewComboBoxColumn();
                            comboColumnProcStat.DataPropertyName = "procstat";
                            comboColumnProcStat.HeaderText = "진행상태";
                            comboColumnProcStat.Name = "procstat";
                            comboColumnProcStat.Items.Add("미진행");
                            comboColumnProcStat.Items.Add("진행중");
                            comboColumnProcStat.Items.Add("완료");

                            // "진행상태" 콤보박스 열을 원래 "진행상태" 열의 인덱스에 추가

                            dataGridView1.Columns.Insert(3, comboColumnProcStat);

                            // "납품상태" 콤보박스 열 추가
                            DataGridViewComboBoxColumn comboColumnDeliStat = new DataGridViewComboBoxColumn();
                            comboColumnDeliStat.DataPropertyName = "delistat";
                            comboColumnDeliStat.HeaderText = "납품상태";
                            comboColumnDeliStat.Name = "delistat";
                            comboColumnDeliStat.Items.Add("미납품");
                            comboColumnDeliStat.Items.Add("배송중");
                            comboColumnDeliStat.Items.Add("완료");

                            dataGridView1.Columns.Insert(4, comboColumnDeliStat);

                            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;

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

                if (dataGridView.Columns[e.ColumnIndex].Name == "procstat" || dataGridView.Columns[e.ColumnIndex].Name == "delistat")
                {
                    bool isProcStatComplete = dataGridView.Rows[e.RowIndex].Cells["procstat"].Value != null && dataGridView.Rows[e.RowIndex].Cells["procstat"].Value.ToString() == "완료";
                    bool isDeliStatComplete = dataGridView.Rows[e.RowIndex].Cells["delistat"].Value != null && dataGridView.Rows[e.RowIndex].Cells["delistat"].Value.ToString() == "완료";

                    // 이전 값과 현재 값 가져오기
                    string previousProcStat = dataGridView.Rows[e.RowIndex].Cells["procstat"].Tag?.ToString();
                    string previousDeliStat = dataGridView.Rows[e.RowIndex].Cells["delistat"].Tag?.ToString();
                    string currentProcStat = dataGridView.Rows[e.RowIndex].Cells["procstat"].Value?.ToString();
                    string currentDeliStat = dataGridView.Rows[e.RowIndex].Cells["delistat"].Value?.ToString();

                    // "완료"에서 다른 값으로 변경하면 날짜를 다시 초기화하고 값을 현재 셀의 데이터로 만듭니다.
                    if ((currentProcStat != null && currentProcStat != "완료") || (currentDeliStat != null && currentDeliStat != "완료"))
                    {
                        dataGridView.Rows[e.RowIndex].Cells["delidate"].Value = DBNull.Value;
                        dataGridView.Rows[e.RowIndex].Cells["procstat"].Value = currentProcStat;
                        dataGridView.Rows[e.RowIndex].Cells["delistat"].Value = currentDeliStat;

                        string salecode = dataGridView.Rows[e.RowIndex].Cells["salecode"].Value.ToString();

                        using (MySqlConnection iConn = new MySqlConnection(jConn))
                        {
                            iConn.Open();
                            MySqlCommand msc = new MySqlCommand("UPDATE sale2 SET procstat = @procstat, delistat = @delistat, delidate = @delidateValue WHERE salecode = @salecode", iConn);
                            msc.Parameters.AddWithValue("@procstat", currentProcStat);
                            msc.Parameters.AddWithValue("@delistat", currentDeliStat);
                            msc.Parameters.AddWithValue("@delidateValue", DBNull.Value); // 날짜를 초기화
                            msc.Parameters.AddWithValue("@salecode", salecode);

                            msc.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // 변경된 값을 데이터베이스에 업데이트합니다.
                        string salecode = dataGridView.Rows[e.RowIndex].Cells["salecode"].Value.ToString();

                        using (MySqlConnection iConn = new MySqlConnection(jConn))
                        {
                            iConn.Open();
                            MySqlCommand msc = new MySqlCommand("UPDATE sale2 SET procstat = @procstat, delistat = @delistat, delidate = @delidateValue WHERE salecode = @salecode", iConn);
                            msc.Parameters.AddWithValue("@procstat", currentProcStat);
                            msc.Parameters.AddWithValue("@delistat", currentDeliStat);
                            msc.Parameters.AddWithValue("@delidateValue", DateTime.Now.ToString("yyyy-MM-dd"));
                            msc.Parameters.AddWithValue("@salecode", salecode);

                            msc.ExecuteNonQuery();
                        }
                    }

                    // 이전 값을 현재 값으로 업데이트합니다.
                    dataGridView.Rows[e.RowIndex].Cells["procstat"].Tag = currentProcStat;
                    dataGridView.Rows[e.RowIndex].Cells["delistat"].Tag = currentDeliStat;
                }
            }
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string selectedSaleCode = selectedRow.Cells["salecode"].Value.ToString();

                // DataGridView2에 해당 판매번호에 대한 세부 정보를 표시하는 함수 호출
                LoadSaleDetails(selectedSaleCode);
            }
        }

        private void LoadSaleDetails(string test)
        {
            dataGridView2.Visible = false;
            MessageBox.Show("ok");
        }

    }
}

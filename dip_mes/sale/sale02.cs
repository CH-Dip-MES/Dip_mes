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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dip_mes
{
    public partial class sale02 : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
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

                    MySqlCommand msc3 = new MySqlCommand("insert into sale3(salecode) values(@salecode)", iConn);
                    msc3.Parameters.AddWithValue("@salecode", salecode.Text);
                    msc3.ExecuteNonQuery();
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
                string sInfo = "select saledate,salecode,buyername,procstat,delistat,delidate from sale2";

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

        private string selectedSaleCode;
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Visible = true;
            label2.Visible = true;
            label4.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            addRow.Visible = true;
            delRow.Visible = true;
            RegButton2.Visible = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedSaleCode = selectedRow.Cells["salecode"].Value.ToString();

                // DataGridView2에 해당 판매번호에 대한 세부 정보를 표시하는 함수 호출
                LoadSaleDetails(selectedSaleCode);
            }
        }
        private void LoadSaleDetails(string salecode)
        {
            // TODO: saleCode를 사용하여 해당 판매번호에 대한 세부 정보를 데이터베이스에서 가져와서 DataGridView2에 표시합니다.
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                string sDetail = "select salecode,planQ,itemprice,sellprice,vat from sale3 WHERE salecode = @salecode";
                MySqlCommand cmd = new MySqlCommand(sDetail, sConn);
                cmd.Parameters.AddWithValue("@salecode", salecode);
                try
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable sManage = new DataTable();
                        adapter.Fill(sManage);
                        dataGridView2.DataSource = sManage;

                        dataGridView2.Columns["salecode"].HeaderText = "판매번호";
                        dataGridView2.Columns["planQ"].HeaderText = "계획수량";
                        dataGridView2.Columns["itemprice"].HeaderText = "단가";
                        dataGridView2.Columns["sellprice"].HeaderText = "판매금액";
                        dataGridView2.Columns["vat"].HeaderText = "부가세";

                        // 품번 콤보박스 컬럼 생성 및 추가
                        if (!dataGridView2.Columns.Contains("comboItemNo"))
                        {
                            DataGridViewComboBoxColumn comboItemNo = new DataGridViewComboBoxColumn();
                            comboItemNo.HeaderText = "품번";
                            comboItemNo.Name = "comboItemNo";
                            comboItemNo.Items.Add("SB-123"); // 예시 아이템
                            dataGridView2.Columns.Insert(1, comboItemNo);
                        }

                        // 품명 콤보박스 컬럼 생성 및 추가
                        if (!dataGridView2.Columns.Contains("comboItemName"))
                        {
                            DataGridViewComboBoxColumn comboItemName = new DataGridViewComboBoxColumn();
                            comboItemName.HeaderText = "품명";
                            comboItemName.Name = "comboItemName";
                            comboItemName.Items.Add("전기차배터리"); // 예시 아이템
                            dataGridView2.Columns.Insert(2, comboItemName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
                // DataGridView2에 데이터를 추가하고 업데이트하세요.
            }
        }

        private void addRow_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView2.DataSource as DataTable;
            if (dt != null)
            {
                // 새로운 행을 생성합니다.
                DataRow newRow = dt.NewRow();

                // 현재 활성화된 salecode를 새 행의 salecode에 할당합니다.
                newRow["salecode"] = selectedSaleCode;

                // 새로운 행을 DataTable에 추가합니다.
                dt.Rows.Add(newRow);

                // DataGridView에 변경 사항을 반영합니다.
                dataGridView2.DataSource = dt;
            }
        }

        private void delRow_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView2.DataSource as DataTable;
            if (dt != null)
            {
                // 사용자가 선택한 행의 인덱스를 가져옵니다.
                int rowIndex = dataGridView2.CurrentCell.RowIndex;

                // 선택된 행이 유효한지 확인합니다.
                if (rowIndex > 0 && rowIndex < dataGridView2.Rows.Count)
                {
                    // 선택된 행을 DataTable에서 제거합니다.
                    dt.Rows[rowIndex].Delete();

                    // 변경사항을 반영합니다.
                    dataGridView2.DataSource = dt;
                }
                else if (rowIndex == 0)
                {
                    // 첫 번째 행을 삭제하려고 한 경우 경고 메시지를 표시합니다.
                    MessageBox.Show("첫 번째 행은 삭제할 수 없습니다.");
                }
            }
        }

        private void RegButton2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection iConn = new MySqlConnection(jConn))
            {
                iConn.Open();

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView2.Rows[i];
                    if (!row.IsNewRow) // 새로운 행이 아닌지 확인
                    {
                        // 각 열의 데이터를 변수에 저장
                        string saleCode = row.Cells["salecode"].Value?.ToString();
                        string itemNo = row.Cells["comboItemNo"].Value?.ToString();
                        string itemName = row.Cells["comboItemName"].Value?.ToString();
                        string planQ = row.Cells["planQ"].Value?.ToString();
                        string itemPrice = row.Cells["itemprice"].Value?.ToString();
                        string sellPrice = row.Cells["sellprice"].Value?.ToString();
                        string vat = row.Cells["vat"].Value?.ToString();

                        // 첫 번째 행은 UPDATE, 나머지 행은 INSERT
                        string query;
                        if (i == 0) // 첫 번째 행
                        {
                            query = "UPDATE sale3 SET ItemNo = @ItemNo, ItemName = @ItemName, planQ = @planQ, itemprice = @itemprice, sellprice = @sellprice, vat = @vat WHERE salecode = @salecode";
                        }
                        else // 추가된 행
                        {
                            query = "INSERT INTO sale3 (salecode, ItemNo, ItemName, planQ, itemprice, sellprice, vat) VALUES (@salecode, @ItemNo, @ItemName, @planQ, @itemprice, @sellprice, @vat)";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, iConn))
                        {
                            cmd.Parameters.AddWithValue("@salecode", saleCode);
                            cmd.Parameters.AddWithValue("@ItemNo", itemNo);
                            cmd.Parameters.AddWithValue("@ItemName", itemName);
                            cmd.Parameters.AddWithValue("@planQ", planQ);
                            cmd.Parameters.AddWithValue("@itemprice", itemPrice);
                            cmd.Parameters.AddWithValue("@sellprice", sellPrice);
                            cmd.Parameters.AddWithValue("@vat", vat);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}

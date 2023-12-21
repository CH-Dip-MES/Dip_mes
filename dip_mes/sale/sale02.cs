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

namespace dip_mes.sale
{
    public partial class sale02 : UserControl
    {
        string jConn = "Server=222.108.180.36;Database=mes_2;Uid=EDU_STUDENT;Pwd=1234;";
        public sale02()
        {
            InitializeComponent();
            LoadProductMapping();
            LoadBuyerNames();
        }
        private void LoadBuyerNames()
        {
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT companyname FROM business WHERE division = '고객사'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        buyername.Items.Clear(); // 기존 항목을 지웁니다.

                        while (reader.Read())
                        {
                            string companyName = reader["companyname"].ToString();
                            buyername.Items.Add(companyName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터베이스 오류: {ex.Message}");
                }
            }
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
        private List<string> GetProductData(string columnName)
        {
            List<string> data = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                conn.Open();
                string cbConn = $"SELECT product_code FROM product";
                MySqlCommand cbmd = new MySqlCommand(cbConn, conn);

                try
                {
                    using (MySqlDataReader reader = cbmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(reader[columnName].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터베이스 오류: {ex.Message}");
                }
            }
            return data;
        }
        private void LoadSaleDetails(string salecode)
        {
            using (MySqlConnection sConn = new MySqlConnection(jConn))
            {
                sConn.Open();
                string sDetail = "select salecode, ItemNo, ItemName, planQ, itemprice, sellprice, vat from sale3 WHERE salecode = @salecode";
                MySqlCommand cmd = new MySqlCommand(sDetail, sConn);
                cmd.Parameters.AddWithValue("@salecode", salecode);

                try
                {
                    // product 테이블에서 데이터 가져오기
                    List<string> productCodes = GetProductData("product_code");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable sManage = new DataTable();
                        adapter.Fill(sManage);
                        dataGridView2.DataSource = sManage;
                        // ItemNo 및 ItemName 열을 콤보박스 셀로 변환
                        ConvertColumnToComboBoxCell(dataGridView2, "ItemNo", productCodes);

                        // 기타 열 헤더 설정
                        dataGridView2.Columns["salecode"].HeaderText = "판매번호";
                        dataGridView2.Columns["ItemNo"].HeaderText = "품번";
                        dataGridView2.Columns["ItemName"].HeaderText = "품명";
                        dataGridView2.Columns["planQ"].HeaderText = "계획수량";
                        dataGridView2.Columns["itemprice"].HeaderText = "단가";
                        dataGridView2.Columns["sellprice"].HeaderText = "판매금액";
                        dataGridView2.Columns["vat"].HeaderText = "부가세";
                    }
                    // CellValueChanged 이벤트 핸들러 연결
                    dataGridView2.CellValueChanged -= DataGridView2_CellValueChanged; // 이전 핸들러 제거 (중복 방지)
                    dataGridView2.CellValueChanged += DataGridView2_CellValueChanged; // 새 핸들러 추가
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
            }
        }


        // 특정 열의 모든 셀을 콤보박스 셀로 변경하는 메서드
        private void ConvertColumnToComboBoxCell(DataGridView dataGridView, string columnName, List<string> items)
        {
            int columnIndex = dataGridView.Columns[columnName].Index;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    // 기존 셀의 값을 저장
                    var existingValue = row.Cells[columnIndex].Value;

                    DataGridViewComboBoxCell comboBoxCell;

                    // 이미 콤보박스 셀인 경우 기존 셀을 사용
                    if (row.Cells[columnIndex] is DataGridViewComboBoxCell existingComboBoxCell)
                    {
                        comboBoxCell = existingComboBoxCell;
                        comboBoxCell.Items.Clear();
                    }
                    else
                    {
                        comboBoxCell = new DataGridViewComboBoxCell();
                        row.Cells[columnIndex] = comboBoxCell;
                    }

                    comboBoxCell.Items.AddRange(items.ToArray());

                    // 콤보박스의 선택된 값으로 기존 값을 설정
                    if (existingValue != null && items.Contains(existingValue.ToString()))
                    {
                        comboBoxCell.Value = existingValue;
                    }
                }
            }
        }
        private Dictionary<string, string> productCodeToNameMapping = new Dictionary<string, string>();

        private void LoadProductMapping()
        {
            using (MySqlConnection conn = new MySqlConnection(jConn))
            {
                conn.Open();
                string query = "SELECT product_code, product_name FROM product";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productCode = reader["product_code"].ToString();
                        string productName = reader["product_name"].ToString();
                        productCodeToNameMapping[productCode] = productName;
                    }
                }
            }
        }
        private void DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            var dataGridView = sender as DataGridView;

            // 'ItemNo' 열의 값이 변경될 때, 매핑된 'ItemName'을 설정
            if (e.ColumnIndex == dataGridView.Columns["ItemNo"].Index && e.RowIndex >= 0)
            {
                string selectedProductCode = dataGridView.Rows[e.RowIndex].Cells["ItemNo"].Value?.ToString();

                if (productCodeToNameMapping.TryGetValue(selectedProductCode, out string productName))
                {
                    dataGridView.Rows[e.RowIndex].Cells["ItemName"].Value = productName;
                }
            }
            // 'planQ'와 'itemprice' 값 변경에 대한 처리
            if ((dataGridView.Columns[e.ColumnIndex].Name == "planQ" ||
                 dataGridView.Columns[e.ColumnIndex].Name == "itemprice") && e.RowIndex >= 0)
            {
                // 유효성 체크 및 숫자 변환
                bool isValidPlanQ = int.TryParse(dataGridView.Rows[e.RowIndex].Cells["planQ"].Value?.ToString(), out int planQ);
                bool isValidItemPrice = decimal.TryParse(dataGridView.Rows[e.RowIndex].Cells["itemprice"].Value?.ToString(), out decimal itemPrice);

                if (isValidPlanQ && isValidItemPrice)
                {
                    // '판매금액'과 '부가세' 계산
                    decimal sellPrice = planQ * itemPrice;
                    decimal vat = sellPrice / 10;

                    // 계산된 값 셀에 설정
                    dataGridView.Rows[e.RowIndex].Cells["sellprice"].Value = sellPrice;
                    dataGridView.Rows[e.RowIndex].Cells["vat"].Value = vat;
                }
            }
            // 판매금액과 부가세의 총합 계산 및 레이블에 표시
            UpdateTotalSums();
        }
        private void UpdateTotalSums()
        {
            int totalSellPrice = 0;
            int totalVat = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (int.TryParse(row.Cells["sellprice"].Value?.ToString(), out int sellPrice))
                    {
                        totalSellPrice += sellPrice;
                    }

                    if (int.TryParse(row.Cells["vat"].Value?.ToString(), out int vat))
                    {
                        totalVat += vat;
                    }
                }
            }
            label4.Text = $"{totalSellPrice}";
            label10.Text = $"{totalVat}";
        }
        private void addRow_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView2.DataSource as DataTable;
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                newRow["salecode"] = selectedSaleCode;
                dt.Rows.Add(newRow);

                // 새로 추가된 행의 인덱스를 가져옴
                int newIndex = dataGridView2.Rows.Count - 1; // -2는 새 행 템플릿을 고려

                // DataGridView에 변경 사항을 반영
                dataGridView2.DataSource = dt;

                Console.WriteLine(dataGridView2.Rows.Count);
                // 새 행의 'ItemNo' 및 'ItemName' 콤보박스 셀로 변환
                ConvertCellToComboBoxCell(dataGridView2, "ItemNo", GetProductData("product_code"), newIndex);
            }
        }
        private void ConvertCellToComboBoxCell(DataGridView dataGridView, string columnName, List<string> items, int rowIndex)
        {
            int columnIndex = dataGridView.Columns[columnName].Index;
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();

            comboBoxCell.Items.AddRange(items.ToArray());

            // 콤보박스 셀로 교체
            dataGridView.Rows[rowIndex].Cells[columnIndex] = comboBoxCell;

            // 콤보박스의 선택된 값으로 기존 값을 설정
            if (dataGridView.Rows[rowIndex].Cells[columnIndex].Value != null &&
                items.Contains(dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString()))
            {
                comboBoxCell.Value = dataGridView.Rows[rowIndex].Cells[columnIndex].Value;
            }

            dataGridView.InvalidateColumn(columnIndex); // 해당 열을 다시 그립니다.
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

                // 선택된 salecode에 해당하는 기존 행 삭제
                string dConn = "DELETE FROM sale3 WHERE salecode = @salecode";
                MySqlCommand deleteCmd = new MySqlCommand(dConn, iConn);
                deleteCmd.Parameters.AddWithValue("@salecode", selectedSaleCode); // selectedSaleCode는 현재 선택된 salecode
                deleteCmd.ExecuteNonQuery();

                // DataGridView2의 모든 행을 sale3에 삽입
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string itemNo = row.Cells["ItemNo"].Value?.ToString();
                        string itemName = row.Cells["ItemName"].Value?.ToString();
                        string planQ = row.Cells["planQ"].Value?.ToString();
                        string itemPrice = row.Cells["itemprice"].Value?.ToString();
                        string sellPrice = row.Cells["sellprice"].Value?.ToString();
                        string vat = row.Cells["vat"].Value?.ToString();

                        string insertQuery = "INSERT INTO sale3 (salecode, ItemNo, ItemName, planQ, itemprice, sellprice, vat) VALUES (@salecode, @ItemNo, @ItemName, @planQ, @itemprice, @sellprice, @vat)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, iConn);
                        insertCmd.Parameters.AddWithValue("@salecode", selectedSaleCode);
                        insertCmd.Parameters.AddWithValue("@ItemNo", itemNo);
                        insertCmd.Parameters.AddWithValue("@ItemName", itemName);
                        insertCmd.Parameters.AddWithValue("@planQ", planQ);
                        insertCmd.Parameters.AddWithValue("@itemprice", itemPrice);
                        insertCmd.Parameters.AddWithValue("@sellprice", sellPrice);
                        insertCmd.Parameters.AddWithValue("@vat", vat);

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace dip_mes
{
    public partial class MaterialOrder : UserControl
    {

        private const string connectionString = "Server = 222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;";

        public MaterialOrder()
        {
            InitializeComponent();
            LoadMaterialMapping();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            // 날짜 형식을 MySQL의 DATETIME 형식으로 변환
            string orderDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            string deliveryDate = dateTimePicker2.Value.ToString("yyyyMMdd");

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 중복되지 않는 orderingcode 생성
                string orderingCode = GenerateOrderingCode(connection, orderDate);

                // 텍스트 박스에 표시
                textBox1.Text = orderingCode;

                // buy1 테이블에 데이터 삽입
                string insertQuery = "INSERT INTO buy1 (DeliveryDate, Code, OrderDate, Orderingcode, Companyname, Writer) VALUES (@DeliveryDate, @Code, @OrderDate, @OrderingCode, @Companyname, @Writer)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                    command.Parameters.AddWithValue("@Code", comboBox1.Text);
                    command.Parameters.AddWithValue("@OrderDate", orderDate);
                    command.Parameters.AddWithValue("@OrderingCode", orderingCode);
                    command.Parameters.AddWithValue("@Companyname", textBox2.Text);
                    command.Parameters.AddWithValue("@Writer", MainScreen.label2Text);

                    command.ExecuteNonQuery();
                }

                string insertBuy2Query = "INSERT INTO buy2 (Orderingcode) VALUES (@OrderingCode)";
                using (MySqlCommand command2 = new MySqlCommand(insertBuy2Query, connection))
                {
                    command2.Parameters.AddWithValue("@OrderingCode", orderingCode);
                    command2.ExecuteNonQuery();
                }
                connection.Close();
                MessageBox.Show($"{orderingCode} 발주건 등록 성공");
                LoadDataToDataGridView1();
            }
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
        }

        // 중복되지 않는 orderingcode 생성 메서드
        private string GenerateOrderingCode(MySqlConnection connection, string orderDate)
        {
            // 해당 날짜에 저장된 마지막 orderingcode 가져오기
            int lastCode = GetLastCodeForOrderDate(orderDate, connection);

            // orderingcode 생성
            string orderingCode = $"B{orderDate}{(lastCode + 1).ToString("000")}";

            return orderingCode;
        }

        // 특정 날짜에 대해 마지막 orderingcode의 숫자 부분 가져오기
        private int GetLastCodeForOrderDate(string orderDate, MySqlConnection connection)
        {
            string lastCodeQuery = "SELECT MAX(CAST(SUBSTRING(Orderingcode, 10) AS SIGNED)) FROM buy1 WHERE OrderDate = @OrderDate";
            using (MySqlCommand command = new MySqlCommand(lastCodeQuery, connection))
            {
                command.Parameters.AddWithValue("@OrderDate", orderDate);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }

                return 0;
            }
        }
        private void MaterialOrder_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView1();
            LoadBusinessData();
            // comboBox1의 이벤트 핸들러 등록
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // dataGridView2의 CellValueChanged 이벤트 핸들러 등록
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            // DataGridView 클리어
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            UpdateBuy1NumberColumn(); // 발주코드의 갯수를 buy1의 number 열에 업데이트
            UpdateBuy1TotalAmountColumn(); //발주코드와 일치한 발주금액과 부가세 합

            // 날짜 형식을 MySQL의 DATETIME 형식으로 변환
            string deliveryDate = dateTimePicker3.Value.ToString("yyyyMMdd");

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy1 테이블에서 데이터 조회
                string selectQuery;
                MySqlCommand command;

                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    // TextBox3에 값이 없으면 DeliveryDate만 일치한 행들 조회
                    selectQuery = "SELECT ROW_NUMBER() OVER(ORDER BY nb DESC) as 'N0.', DeliveryDate AS '발주일자', code as '업체코드', Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액', Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1 WHERE DeliveryDate = @DeliveryDate";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                }
                else
                {
                    // TextBox3에 값이 있으면 code와 DeliveryDate가 일치한 행들 조회
                    selectQuery = "SELECT ROW_NUMBER() OVER(ORDER BY nb DESC) as 'N0.', DeliveryDate AS '발주일자', code as '업체코드', Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액', Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1 WHERE DeliveryDate = @DeliveryDate AND Code = @Code";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                    command.Parameters.AddWithValue("@Code", textBox3.Text);
                }

                // 데이터 가져오기
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView에 데이터 바인딩
                    dataGridView1.DataSource = dataTable;
                }

                connection.Close();


            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedCode = dataGridView1.Rows[e.RowIndex].Cells["발주코드"].Value.ToString();
                LoadDataToDataGridView2(selectedCode);

                // 클릭한 행의 데이터를 가져와서 처리
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string codeValue = selectedRow.Cells["발주코드"].Value.ToString();

                // textBox7에 업체코드 표시
                textBox7.Text = codeValue;
            }
        }

        private void LoadDataToDataGridView1()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY nb DESC) as 'N0.', DeliveryDate AS '납기일자', code as '업체코드',
                                    Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액',
                                    Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView1에 데이터 바인딩
                    dataGridView1.DataSource = dataTable;
                }
                connection.Close();
            }

            // DataGridView1에서 첫 번째 행을 선택하도록 설정
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void CalculateTotalOrderAmountForGridView2()
        {
            decimal totalOrderAmount = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["발주금액"].Value != null && decimal.TryParse(row.Cells["발주금액"].Value.ToString(), out decimal orderAmount))
                {
                    totalOrderAmount += orderAmount;
                }
            }

            textBox6.Text = $"{totalOrderAmount:N0}원"; // N0는 숫자를 천 단위로 구분하여 표시
        }

        private List<string> GetMaterialData(string columnName)
        {
            List<string> data = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string cbConn = $"SELECT Material_name FROM material";
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

        private void LoadDataToDataGridView2(string selectedCode)
        {
            List<string> materialName = GetMaterialData("Material_name");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // DataGridView2에 데이터를 가져오는 쿼리
                string selectQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY nb DESC) as 'NO.', Itemnumber as '품번', Itemname as '품명', 
                Weight as '갯수', Unitprice as '단가', Orderamount as '발주금액', Surtax as '부가세' FROM buy2 WHERE Orderingcode = @Orderingcode";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Orderingcode", selectedCode);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView2에 데이터 바인딩
                        dataGridView2.DataSource = dataTable;

                        // 발주금액 컬럼에 대한 형식 지정
                        dataGridView2.Columns["발주금액"].DefaultCellStyle.Format = "Orderamount";
                    }
                    dataGridView2.CellValueChanged -= DataGridView2_CellValueChanged; // 이전 핸들러 제거 (중복 방지)
                    dataGridView2.CellValueChanged += DataGridView2_CellValueChanged; // 새 핸들러 추가
                }

                connection.Close();

                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;
                }

                // DataGridView2에서 첫 번째 행을 선택하도록 설정
                dataGridView2.RowsAdded -= dataGridView2_RowsAdded;
                dataGridView2.RowsAdded += dataGridView2_RowsAdded;

                // 열을 콤보박스 셀로 변환
                ConvertColumnToComboBoxCell(dataGridView2, "품명", materialName);
            }
            // 발주금액 합 다시 계산
            CalculateTotalOrderAmountForGridView2();
            // 부가세 합 계산
            CalculateTotalSurtaxForGridView2();
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[0].DefaultCellStyle.BackColor = Color.Gray;
            dataGridView2.Columns[0].DefaultCellStyle.ForeColor = Color.White;
        }

        private void ConvertColumnToComboBoxCell(DataGridView dataGridView, string columnName, List<string> items)
        {
            if (dataGridView.Columns.Contains(columnName))
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
        }

            private void DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            var dataGridView = sender as DataGridView;

            // '품명' 열의 값이 변경될 때, 매핑된 '품번'을 설정
            if (e.ColumnIndex == dataGridView.Columns["품명"].Index && e.RowIndex >= 0)
            {
                string selectedMaterialName = dataGridView.Rows[e.RowIndex].Cells["품명"].Value?.ToString();

                if (materialNameToNumberMapping.TryGetValue(selectedMaterialName, out string materialCode))
                {
                    dataGridView.Rows[e.RowIndex].Cells["품번"].Value = materialCode;
                }
            }
        }
        private Dictionary<string, string> materialNameToNumberMapping = new Dictionary<string, string>();

        private void LoadMaterialMapping()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Material_code, Material_name FROM material";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string materialCode = reader["Material_code"].ToString();
                        string materialName = reader["Material_name"].ToString();
                        materialNameToNumberMapping[materialName] = materialCode;
                    }
                }
            }
        }

        private void CalculateTotalSurtaxForGridView2()
        {
            decimal totalSurtax = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["부가세"].Value != null && decimal.TryParse(row.Cells["부가세"].Value.ToString(), out decimal surtax))
                {
                    totalSurtax += surtax;
                }
            }

            textBox5.Text = $"{totalSurtax:N0}원";
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView2의 CellEndEdit 이벤트 핸들러
            // 여기서 변경된 값을 가져와서 필요한 처리를 수행합니다.
            // 이 예시에서는 변경된 값을 콘솔에 출력하고 있습니다.
            Console.WriteLine("Cell Edited: " + dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            // 선택된 행의 발주코드 가져오기
            string selectedOrderingCode = textBox7.Text;

            if (string.IsNullOrEmpty(selectedOrderingCode))
            {
                MessageBox.Show("발주코드를 선택하세요.");
                return; // 발주코드가 없으면 함수 종료
            }

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("행을 선택하세요.");
                return; // 선택된 행이 없으면 함수 종료
            }

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    // 빈칸 체크 (단, "nb" 열은 빈 값 허용)
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name != "NO." && (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString())))
                        {
                            MessageBox.Show("데이터를 입력하세요.");
                            return; // 빈칸이 있으면 함수 종료
                        }
                    }

                    // DataGridView2의 선택된 행에 대한 데이터를 DB에 저장
                    string insertQuery = "INSERT INTO buy2 (nb, Itemnumber, Itemname, Weight, Unitprice, Orderamount, Surtax, Orderingcode) VALUES (@nb, @Itemnumber, @Itemname, @Weight, @Unitprice, @Orderamount, @Surtax, @Orderingcode)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nb", row.Cells["NO."].Value);
                        command.Parameters.AddWithValue("@Itemnumber", row.Cells["품번"].Value);
                        command.Parameters.AddWithValue("@Itemname", row.Cells["품명"].Value);
                        command.Parameters.AddWithValue("@Weight", row.Cells["갯수"].Value);

                        // 단가 열 처리
                        try
                        {
                            // 단가 열이 비어있지 않으면 숫자로 변환
                            if (row.Cells["단가"].Value != null)
                            {
                                decimal unitPrice = decimal.Parse(row.Cells["단가"].Value.ToString());
                                command.Parameters.AddWithValue("@Unitprice", unitPrice);
                            }
                            else
                            {
                                // 단가 열이 비어있으면 DBNull.Value로 처리
                                command.Parameters.AddWithValue("@Unitprice", DBNull.Value);
                            }
                        }
                        catch (FormatException)
                        {
                            // 숫자로 변환할 수 없는 경우 메시지 박스로 알림 표시
                            MessageBox.Show("단가에는 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // 발주금액 열 처리
                        try
                        {
                            // 발주금액 열이 비어있지 않으면 숫자로 변환
                            if (row.Cells["발주금액"].Value != null)
                            {
                                decimal orderAmount = decimal.Parse(row.Cells["발주금액"].Value.ToString());
                                command.Parameters.AddWithValue("@Orderamount", orderAmount);
                            }
                            else
                            {
                                // 발주금액 열이 비어있으면 DBNull.Value로 처리
                                command.Parameters.AddWithValue("@Orderamount", DBNull.Value);
                            }
                        }
                        catch (FormatException)
                        {
                            // 숫자로 변환할 수 없는 경우 메시지 박스로 알림 표시
                            MessageBox.Show("발주금액에는 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // 부가세 열 처리
                        try
                        {
                            // 부가세 열이 비어있지 않으면 숫자로 변환
                            if (row.Cells["부가세"].Value != null)
                            {
                                decimal surtax = decimal.Parse(row.Cells["부가세"].Value.ToString());
                                command.Parameters.AddWithValue("@Surtax", surtax);
                            }
                            else
                            {
                                // 부가세 열이 비어있으면 DBNull.Value로 처리
                                command.Parameters.AddWithValue("@Surtax", DBNull.Value);
                            }
                        }
                        catch (FormatException)
                        {
                            // 숫자로 변환할 수 없는 경우 메시지 박스로 알림 표시
                            MessageBox.Show("부가세에는 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        command.Parameters.AddWithValue("@Orderingcode", selectedOrderingCode);

                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }

            MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
            // 등록 후 발주금액 합 다시 계산
            CalculateTotalOrderAmount();
            CalculateTotalSurtaxForGridView2();
            UpdateBuy1TotalAmounts();
            UpdateBuy1TotalAmountColumn();
            UpdateBuy1NumberColumn();
            LoadDataToDataGridView1();
            LoadDataToDataGridView2(selectedOrderingCode);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string selectedOrderingCode = textBox7.Text;
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // 선택된 행의 발주코드 가져오기

                if (string.IsNullOrEmpty(selectedOrderingCode))
                {
                    MessageBox.Show("발주코드를 선택하세요.");
                    return; // 발주코드가 없으면 함수 종료
                }

                // MySQL 연결 및 명령어 생성
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                    {
                        if (!IsRowCommitted(row, selectedOrderingCode))
                        {
                            MessageBox.Show("DB에 존재하지 않는 데이터 입니다.");
                            continue;
                        }
                        // DataGridView2의 선택된 행에 대한 데이터를 DB에서 삭제
                        string deleteQuery = "DELETE FROM buy2 WHERE Itemnumber = @Itemnumber AND Orderingcode = @Orderingcode AND Orderamount = @Orderamount";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Itemnumber", row.Cells["품번"].Value);
                            command.Parameters.AddWithValue("@Orderamount", row.Cells["발주금액"].Value);
                            command.Parameters.AddWithValue("@Orderingcode", selectedOrderingCode);

                            command.ExecuteNonQuery();
                            MessageBox.Show("데이터가 성공적으로 삭제되었습니다.");
                        }

                        // DataGridView2에서 선택된 행 삭제
                        dataGridView2.Rows.Remove(row);
                    }

                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("행을 선택하세요.");
            }
            // 등록 후 발주금액 합 다시 계산
            CalculateTotalOrderAmount();
            CalculateTotalSurtaxForGridView2();
            UpdateBuy1TotalAmounts();
            UpdateBuy1TotalAmountColumn();
            UpdateBuy1NumberColumn();
            LoadDataToDataGridView1();
            LoadDataToDataGridView2(selectedOrderingCode);
        }

        // 행이 데이터베이스에 커밋되었는지 확인하는 로직
        private bool IsRowCommitted(DataGridViewRow row, string selectedOrderingCode)
        {

            // 실제로는 데이터베이스 쿼리를 사용하여 기본 키가 존재하는지 확인해야 합니다.
            // 아래 예제는 가상의 데이터베이스 연결 및 쿼리를 사용하는 것을 나타냅니다.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM buy2 WHERE Itemnumber = @Itemnumber AND Orderingcode = @Orderingcode AND Orderamount = @Orderamount";
                using (MySqlCommand command = new MySqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@Itemnumber", row.Cells["품번"].Value);
                    command.Parameters.AddWithValue("@Orderamount", row.Cells["발주금액"].Value);
                    command.Parameters.AddWithValue("@Orderingcode", selectedOrderingCode);

                    int rowCount = Convert.ToInt32(command.ExecuteScalar());

                    return rowCount > 0; // 기본 키가 존재하면 true를 반환
                }
            }
        }


        private void CalculateTotalOrderAmount()
        {
            // 선택된 행의 발주코드 가져오기
            string selectedOrderingCode = textBox7.Text;

            if (string.IsNullOrEmpty(selectedOrderingCode))
            {
                MessageBox.Show("발주코드를 선택하세요.");
                return; // 발주코드가 없으면 함수 종료
            }

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 발주코드와 일치하는 행들의 발주금액 합을 계산
                string selectQuery = "SELECT SUM(Orderamount) FROM buy2 WHERE Orderingcode = @Orderingcode";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Orderingcode", selectedOrderingCode);

                    // 결과 가져오기
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // 발주금액 합을 textBox6에 표시
                        decimal totalOrderAmount = Convert.ToDecimal(result);
                        textBox6.Text = totalOrderAmount.ToString("#,##0");
                        decimal TotalSurtax = Convert.ToDecimal(result);
                        textBox5.Text = TotalSurtax.ToString("#,##0");
                    }
                    else
                    {
                        // 발주코드에 해당하는 데이터가 없을 경우 0으로 초기화
                        textBox6.Text = "0";
                        textBox5.Text = "0";
                    }
                }

                connection.Close();
            }
        }

        private void UpdateBuy1TotalAmounts()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 발주금액 업데이트
                string updateOrderAmountQuery = @"
            UPDATE buy1 b1
            LEFT JOIN (
                SELECT orderingcode, COALESCE(SUM(Orderamount), 0) AS TotalOrderamount
                FROM buy2
                GROUP BY orderingcode
            ) b2 ON b1.Orderingcode = b2.orderingcode
            SET b1.Orderamount = b2.TotalOrderamount;
        ";

                // 부가세 업데이트
                string updateSurtaxQuery = @"
            UPDATE buy1 b1
            LEFT JOIN (
                SELECT orderingcode, COALESCE(SUM(Surtax), 0) AS TotalSurtax
                FROM buy2
                GROUP BY orderingcode
            ) b2 ON b1.Orderingcode = b2.orderingcode
            SET b1.Surtax = b2.TotalSurtax;
        ";

                using (MySqlCommand command1 = new MySqlCommand(updateOrderAmountQuery, connection))
                using (MySqlCommand command2 = new MySqlCommand(updateSurtaxQuery, connection))
                {
                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void UpdateBuy1NumberColumn()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 발주코드의 갯수를 구해서 buy1의 number 열에 업데이트하는 쿼리
                string updateNumberQuery = @"
            UPDATE buy1 b1
            LEFT JOIN (
                SELECT orderingcode, COUNT(*) AS OrderCount
                FROM buy2
                WHERE Itemnumber IS NOT NULL AND Itemname IS NOT NULL AND Unitprice IS NOT NULL AND weight IS NOT NULL AND Surtax IS NOT NULL AND Orderamount
                GROUP BY orderingcode
            ) b2 ON b1.Orderingcode = b2.orderingcode
            SET b1.number = COALESCE(b2.OrderCount, 0);
        ";

                using (MySqlCommand command = new MySqlCommand(updateNumberQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateBuy1TotalAmountColumn()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Totalamount 업데이트
                string updateTotalAmountQuery = @"
                UPDATE buy1 b1
                LEFT JOIN (
                SELECT orderingcode, COALESCE(SUM(Orderamount), 0) + COALESCE(SUM(Surtax), 0) AS TotalAmount
                FROM buy2
                GROUP BY orderingcode
                ) b2 ON b1.Orderingcode = b2.orderingcode
                SET b1.Totalamount = b2.TotalAmount;";

                using (MySqlCommand command = new MySqlCommand(updateTotalAmountQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Login.getAuth < 2)
            {
                MessageBox.Show("권한이 없습니다.");
                return;
            }
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // MySQL 연결 및 명령어 생성
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string selectedOrderingCode = textBox7.Text;

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        // DataGridView2의 선택된 행에 대한 데이터를 DB에서 삭제
                        string deleteQuery = "DELETE FROM buy1 WHERE Orderingcode = @Orderingcode";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Orderingcode", selectedOrderingCode);

                            command.ExecuteNonQuery();
                        }

                        // DataGridView2에서 선택된 행 삭제
                        dataGridView1.Rows.Remove(row);
                    }
                    connection.Close();
                }
                MessageBox.Show("데이터가 성공적으로 삭제되었습니다.");
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
            }
            else
            {
                MessageBox.Show("행을 선택하세요.");
            }
        }
        private void LoadBusinessData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // division이 '협력사'인 행들의 companycode를 선택
                string selectQuery = "SELECT companycode FROM business WHERE division = '협력사'";

                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // comboBox1를 초기화
                        comboBox1.Items.Clear();

                        // 데이터를 comboBox1에 추가
                        while (reader.Read())
                        {
                            // 각 행의 "companycode"를 가져와서 comboBox1에 추가
                            string companyCode = reader["companycode"].ToString();
                            comboBox1.Items.Add(companyCode);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox1에서 선택된 companycode 가져오기
            string selectedCompanyCode = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedCompanyCode))
            {
                // MySQL 연결 및 명령어 생성
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 선택된 companycode에 해당하는 companyname 가져오기
                    string selectQuery = "SELECT companyname FROM business WHERE companycode = @CompanyCode";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CompanyCode", selectedCompanyCode);

                        // 결과 가져오기
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            // companyname을 textBox2에 표시
                            textBox2.Text = result.ToString();
                        }
                        else
                        {
                            // 선택된 companycode에 해당하는 데이터가 없을 경우 textBox2를 초기화
                            textBox2.Text = string.Empty;
                        }
                    }

                    connection.Close();
                }
            }
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 발주금액 열이 변경된 경우
            if (e.ColumnIndex == dataGridView2.Columns["발주금액"].Index && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells["발주금액"];
                DataGridViewCell surtaxCell = dataGridView2.Rows[e.RowIndex].Cells["부가세"];

                // 발주금액이 비어있지 않으면 계산 후 부가세 열에 넣기
                if (cell.Value != null && decimal.TryParse(cell.Value.ToString(), out decimal orderAmount))
                {
                    decimal surtax = Math.Round(orderAmount * 0.1M); // 부가세 계산 (10%) 후 반올림
                    surtaxCell.Value = surtax == (int)surtax ? ((int)surtax).ToString() : surtax.ToString(); // 소수점 이하가 0이면 점 없애기
                }
                else
                {
                    // 발주금액이 비어있거나 숫자로 변환할 수 없으면 부가세 열 초기화
                    surtaxCell.Value = DBNull.Value;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadDataToDataGridView1();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //LoadDataToDataGridView2(textBox7.Text);
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ConvertColumnToComboBoxCell2(dataGridView2, "품명", GetMaterialData("Material_name"), e.RowIndex);
        }

        private void ConvertColumnToComboBoxCell2(DataGridView dataGridView, string columnName, List<string> items, int rowIndex)
        {
            if (dataGridView.Columns.Contains(columnName) && rowIndex >= 0)
            {
                int columnIndex = dataGridView.Columns[columnName].Index;
                DataGridViewRow row = dataGridView.Rows[rowIndex];

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
}

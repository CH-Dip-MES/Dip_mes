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


namespace dip_mes.buy
{
    public partial class buy01 : UserControl
    {

        private const string connectionString = "Server = 222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;";

        public buy01()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                string insertQuery = "INSERT INTO buy1 (DeliveryDate, Code, OrderDate, Orderingcode, Companyname) VALUES (@DeliveryDate, @Code, @OrderDate, @OrderingCode, @Companyname)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                    command.Parameters.AddWithValue("@Code", textBox4.Text);
                    command.Parameters.AddWithValue("@OrderDate", orderDate);
                    command.Parameters.AddWithValue("@OrderingCode", orderingCode);
                    command.Parameters.AddWithValue("@Companyname", textBox2.Text);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
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
        private void buy01_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView1();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // DataGridView 클리어
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

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
                    selectQuery = "SELECT nb AS 'N0.', orderdate AS '발주일자', code as '업체코드', Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액', Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1 WHERE DeliveryDate = @DeliveryDate";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                }
                else
                {
                    // TextBox3에 값이 있으면 code와 DeliveryDate가 일치한 행들 조회
                    selectQuery = "SELECT nb AS 'N0.', orderdate AS '발주일자', code as '업체코드', Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액', Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1 WHERE DeliveryDate = @DeliveryDate AND Code = @Code";
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
                string codeValue = selectedRow.Cells["업체코드"].Value.ToString();

                // textBox7에 업체코드 표시
                textBox7.Text = codeValue;
            }
        }

        private void LoadDataToDataGridView1()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT nb AS 'N0.', orderdate AS '발주일자', code as '업체코드', Companyname AS '업체명', number AS '건수', Orderamount AS '발주금액', Surtax AS '부가세', Totalamount AS '합계금액', Writer AS '작성자', Orderingcode AS '발주코드' FROM buy1";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                connection.Close();
            }
        }
        private void LoadDataToDataGridView2(string selectedCode)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT nb as 'NO.', Itemnumber as '품번', Itemname as '품명', Weight as '중량', Unitprice as '단가', Orderamount as '발주금액', Surtax as '부가세' FROM buy1 WHERE OrderingCode = @OrderingCode";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@OrderingCode", selectedCode);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView2.DataSource = dataTable;
                    }
                }
                connection.Close();
            }
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
            // 선택된 행이 있는지 확인
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // 선택된 DataGridView2 행의 값 가져오기
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                string nb = selectedRow.Cells["NO."].Value.ToString();
                string itemNumber = selectedRow.Cells["품번"].Value.ToString();
                string itemName = selectedRow.Cells["품명"].Value.ToString();
                string weight = selectedRow.Cells["중량"].Value.ToString();
                string unitPrice = selectedRow.Cells["단가"].Value.ToString();
                string orderAmount = selectedRow.Cells["발주금액"].Value.ToString();
                string surtax = selectedRow.Cells["부가세"].Value.ToString();

                // MySQL 연결 및 명령어 생성
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // DataGridView2의 특정 행에 대한 값을 MySQL 데이터베이스에 추가하는 코드
                    string insertQuery = "INSERT INTO buy1 (nb, Itemnumber, Itemname, Weight, Unitprice, Orderamount, Surtax) " +
                                        "VALUES (@nb, @Itemnumber, @Itemname, @Weight, @Unitprice, @Orderamount, @Surtax)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        // 각 매개변수에 값을 할당
                        command.Parameters.AddWithValue("@nb", nb);
                        command.Parameters.AddWithValue("@Itemnumber", itemNumber);
                        command.Parameters.AddWithValue("@Itemname", itemName);
                        command.Parameters.AddWithValue("@Weight", weight);
                        command.Parameters.AddWithValue("@Unitprice", unitPrice);
                        command.Parameters.AddWithValue("@Orderamount", orderAmount);
                        command.Parameters.AddWithValue("@Surtax", surtax);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
            }
            else
            {
                MessageBox.Show("행을 선택하세요.");
            }
        }
    }
}

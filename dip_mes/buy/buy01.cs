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
    }
}

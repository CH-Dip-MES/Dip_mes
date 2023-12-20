using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dip_mes.goods
{
    public partial class standard : UserControl
    {
        private MySqlConnection connection;
        private string connectionString = "Server=222.108.180.36; Database=mes_2; User ID=EDU_STUDENT; Password=1234;";

        public standard()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadDataIntoComboBox();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("MySQL Connection Opened");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void LoadDataIntoComboBox()
        {
            string query = "SELECT process_name FROM process";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("process_name"));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string selectedValue = comboBox1.SelectedItem?.ToString();
            string inputValue = txtInput.Text;

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(inputValue))
            {
                string insertQuery = "INSERT INTO product_process (process_name, process_time) VALUES (@SelectedValue, @InputValue)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@SelectedValue", selectedValue);
                    cmd.Parameters.AddWithValue("@InputValue", inputValue);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("데이터가 성공적으로 등록되었습니다.");

                        // 데이터 이동 부분 추가
                        MoveDataBetweenTables();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        MessageBox.Show("데이터 등록 중 오류가 발생했습니다.");
                    }
                }
            }
            else
            {
                MessageBox.Show("콤보박스와 텍스트 박스에 값을 입력하세요.");
            }
        }

        private void MoveDataBetweenTables()
        {
            try
            {
                // 읽어올 테이블
                string sourceTableName = "sourceTable";
                string selectQuery = $"SELECT * FROM {sourceTableName}";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);

                MySqlDataReader dataReader = selectCmd.ExecuteReader();

                // 삽입할 테이블
                string destinationTableName = "destinationTable";
                string insertQuery = $"INSERT INTO {destinationTableName} (column1, column2, column3) VALUES (@Value1, @Value2, @Value3)";

                while (dataReader.Read())
                {
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                    {
                        // 예시: 컬럼 이름 및 데이터 타입에 맞게 수정
                        insertCmd.Parameters.AddWithValue("@Value1", dataReader.GetString("sourceColumn1"));
                        insertCmd.Parameters.AddWithValue("@Value2", dataReader.GetInt32("sourceColumn2"));
                        insertCmd.Parameters.AddWithValue("@Value3", dataReader.GetDateTime("sourceColumn3"));

                        insertCmd.ExecuteNonQuery();
                    }
                }

                dataReader.Close();
                MessageBox.Show("데이터 이동이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("데이터 이동 중 오류가 발생했습니다.");
            }
        }
    }
}

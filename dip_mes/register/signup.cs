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

namespace dip_mes.register
{

    
    public partial class signup : UserControl

    {
        
        private MySqlConnection connection;
        private string ConnectionString = "server=222.108.180.36; Uid=EDU_STUDENT; password=1234; database=mes_2;";

        public signup()
        {
            InitializeComponent();
        }

        private void serch_btn_Click(object sender, EventArgs e)
        {
            string nameToSearch = textBox_name.Text.Trim();
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();

            // MySQL 쿼리 작성
            string query = $"SELECT name, id, pwd, number, email, department FROM test WHERE name = '{nameToSearch}'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", nameToSearch);

            // 데이터 어댑터 및 데이터테이블 사용
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // 데이터그리드에 데이터 바인딩
            dataGridView1.DataSource = dataTable;




            Console.WriteLine(query);
            
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "선택";
            checkBoxColumn.Name = "checkBoxColumn";
            dataGridView1.Columns.Insert(0, checkBoxColumn);


            foreach (DataRow row in dataTable.Rows)
            {
                // DataGridViewRow를 생성하여 데이터 입력
                DataGridViewRow dgvRow = new DataGridViewRow();
                dgvRow.CreateCells(dataGridView1);

                // 각 컬럼에 데이터 입력
                dgvRow.Cells[0].Value = row["name"];  // 여기서 0은 DataGridView의 첫 번째 컬럼을 나타냅니다.
                dgvRow.Cells[1].Value = row["id"];
                dgvRow.Cells[2].Value = row["pwd"];
                dgvRow.Cells[3].Value = row["number"];
                dgvRow.Cells[4].Value = row["email"];
                dgvRow.Cells[5].Value = row["department"];
                // ...
                // DataGridViewCheckBoxCell을 만들어서 첫 번째 열에 추가
                DataGridViewCheckBoxCell checkBoxCell = new DataGridViewCheckBoxCell();
                checkBoxCell.Value = false; // 체크 여부 초기화
                dgvRow.Cells.Insert(0, checkBoxCell);



                // DataGridView에 행 추가
                //dataGridView1.Rows.Add(dgvRow);
                


            }
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns["name"].HeaderText = "이름";
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["pwd"].HeaderText = "PW";
            dataGridView1.Columns["number"].HeaderText = "주민등록번호";
            dataGridView1.Columns["email"].HeaderText = "이메일";
            dataGridView1.Columns["department"].HeaderText = "부서";
           
        }
        

        
    }
}


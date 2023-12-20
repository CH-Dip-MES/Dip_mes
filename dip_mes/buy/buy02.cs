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

    public partial class buy02 : UserControl
    {
        private const string connectionString = "Server = 222.108.180.36; Database=mes_2; Uid=EDU_STUDENT;Pwd=1234;";

        public buy02()
        {
            InitializeComponent();
        }

        private void buy02_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView3();
            InitializeNumericUpDown();
            LoadDataToDataGridView1((int)numericUpDown1.Value);
        }

        

        private void LoadDataToDataGridView3()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy3 테이블에서 데이터 조회
                string selectQuery = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY nb DESC) as 'No.', 
                companycode AS '업체코드', 
                itemname AS '품명', 
                itemnumber AS '품번', 
                orderweight AS '발주중량', 
                incomingweight AS '입고중량', 
                orderingcode AS '발주코드'
            FROM buy3
        ";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView3에 데이터 바인딩
                    dataGridView3.DataSource = dataTable;
                }

                connection.Close();
            }

            // DataGridView3에서 첫 번째 행을 선택하도록 설정
            if (dataGridView3.Rows.Count > 0)
            {
                dataGridView3.Rows[0].Selected = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // DataGridView 클리어
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();

            // 날짜 형식을 MySQL의 DATETIME 형식으로 변환
            string deliveryDate = dateTimePicker1.Value.ToString("yyyyMMdd");

            // MySQL 연결 및 명령어 생성
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy1 테이블에서 데이터 조회
                string selectQuery;
                MySqlCommand command;

                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    // TextBox3에 값이 없으면 DeliveryDate만 일치한 행들 조회
                    selectQuery = @"
                SELECT 
                ROW_NUMBER() OVER (ORDER BY nb DESC) as 'No.', 
                companycode AS '업체코드', 
                itemname AS '품명', 
                itemnumber AS '품번', 
                orderweight AS '발주중량', 
                incomingweight AS '입고중량', 
                orderingcode AS '발주코드'
            FROM buy3
                WHERE DeliveryDate = @DeliveryDate";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                }
                else
                {
                    // TextBox3에 값이 있으면 code와 DeliveryDate가 일치한 행들 조회
                    selectQuery = @"
                SELECT 
                ROW_NUMBER() OVER (ORDER BY nb DESC) as 'No.', 
                companycode AS '업체코드', 
                itemname AS '품명', 
                itemnumber AS '품번', 
                orderweight AS '발주중량', 
                incomingweight AS '입고중량', 
                orderingcode AS '발주코드'
            FROM buy3
                WHERE DeliveryDate = @DeliveryDate AND companycode = @companycode";
                    command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                    command.Parameters.AddWithValue("@companycode", textBox1.Text);
                }

                // 데이터 가져오기
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // DataGridView에 데이터 바인딩
                    dataGridView3.DataSource = dataTable;
                }

                connection.Close();
            }
        }
        private void InitializeNumericUpDown()
        {
            // 년도 범위 설정 (예: 2000년부터 현재 년도까지)
            numericUpDown1.Minimum = 2000;
            numericUpDown1.Maximum = DateTime.Now.Year;

            // 현재 년도로 초기화
            numericUpDown1.Value = DateTime.Now.Year;
        }

        private void LoadDataToDataGridView1(int selectedYear)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // buy4 테이블에서 데이터 조회
                string selectQuery = @"
            SELECT 
                year, 1Enterquantity, 1Deliveryquantity, 2Enterquantity, 2Deliveryquantity,
                3Deliveryquantity, 3Enterquantity, 4Deliveryquantity, 4Enterquantity,
                5Deliveryquantity, 5Enterquantity, 6Deliveryquantity, 6Enterquantity,
                7Deliveryquantity, 7Enterquantity, 8Deliveryquantity, 8Enterquantity,
                9Deliveryquantity, 9Enterquantity, 10Deliveryquantity, 10Enterquantity,
                11Deliveryquantity, 11Enterquantity, 12Deliveryquantity, 12Enterquantity
            FROM buy4
            WHERE year = @Year
        ";

                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Year", selectedYear);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView1 초기화
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();

                        // 1행 추가 (월 표시)
                        dataGridView1.Columns.Add("년도", "년도");
                        for (int month = 1; month <= 12; month++)
                        {
                            dataGridView1.Columns.Add(month + "월", month + "월");
                            dataGridView1.Columns.Add(month + "월", month + "월");
                        }

                        // 2행 추가 (DB 칼럼명)
                        object[] columnRowData = new object[37];
                        columnRowData[0] = "year";
                        for (int month = 1; month <= 12; month++)
                        {
                            columnRowData[month] = month + "Enterquantity";
                            columnRowData[month + 12] = month + "Deliveryquantity";
                        }
                        dataGridView1.Rows.Add(columnRowData);

                        // 3행 추가 (DB 데이터)
                        if (dataTable.Rows.Count > 0)
                        {
                            object[] rowData = new object[37];
                            rowData[0] = dataTable.Rows[0]["year"].ToString(); // "년도" 열에 년도 데이터 설정

                            for (int month = 1; month <= 12; month++)
                            {
                                string enterColumnName = month + "Enterquantity";
                                string deliveryColumnName = month + "Deliveryquantity";

                                rowData[2 * month - 1] = dataTable.Rows[0][enterColumnName];
                                rowData[2 * month] = dataTable.Rows[0][deliveryColumnName];
                            }

                            dataGridView1.Rows.Add(rowData);
                        }
                    }
                }

                connection.Close();
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // NumericUpDown의 값이 변경될 때마다 데이터 다시 불러오기
            LoadDataToDataGridView1((int)numericUpDown1.Value);
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r1 = dataGridView1.GetCellDisplayRectangle(1, -1, true);
            int w2 = dataGridView1.GetCellDisplayRectangle(2, -1, true).Width;
            r1.X += 1;
            r1.Y += 1;
            r1.Width = r1.Width + w2 - 2;
            r1.Height = r1.Height / 2 - 2;
            e.Graphics.FillRectangle(new SolidBrush(dataGridView1.ColumnHeadersDefaultCellStyle.BackColor), r1);

            StringFormat format = new StringFormat();

            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("Merged Text", dataGridView1.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

        }
        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rtHeader = dataGridView1.DisplayRectangle;
            rtHeader.Height = dataGridView1.ColumnHeadersHeight / 2;
            dataGridView1.Invalidate(rtHeader);
        }
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            Rectangle rtHeader = dataGridView1.DisplayRectangle;
            rtHeader.Height = dataGridView1.ColumnHeadersHeight / 2;
            dataGridView1.Invalidate(rtHeader);

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }

        }

        private void dataGridView1_ColumnHeadersHeightChanged(object sender, EventArgs e)
        {

        }
    }
    }

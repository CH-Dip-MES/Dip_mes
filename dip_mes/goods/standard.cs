using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace dip_mes.goods
{
    public partial class standard : UserControl
    {
        public standard()
        {
            InitializeComponent();

            // UserControl 로드 시에 열 및 RowHeadersVisible 설정을 할 수 있도록 이벤트 핸들러 등록
            this.Load += Standard_Load;
        }

        private void Standard_Load(object sender, EventArgs e)
        {
            // 데이터그리드뷰에 열 추가
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "체크";
            dataGridView2.Columns.Add(checkBoxColumn); // 체크박스 열을 추가

            // DataGridView에 행 번호를 표시하는 방법
            dataGridView2.RowPostPaint += DataGridView2_RowPostPaint;

            dataGridView2.Columns.Add("ProcessColumn", "공정");
            dataGridView2.Columns.Add("LeadTimeColumn", "리드타임");





        }

        private void DataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 각 행의 행 번호를 표시
            using (SolidBrush b = new SolidBrush(dataGridView2.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 텍스트박스에서 체크, 공정, 리드타임을 가져옴
            bool isChecked = dataGridView2.Rows.Count > 0 &&
                             dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells["체크"].Value != null &&
                             (bool)dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells["체크"].Value;
            string process = txtProcess.Text;
            string leadTime = txtLeadTime.Text;

            // 데이터그리드뷰에 행 추가
            dataGridView2.Rows.Add(isChecked, process, leadTime);

            // 텍스트박스 비우기
            txtProcess.Clear();
            txtLeadTime.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 체크된 행을 찾아서 삭제할 목록 생성
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // 체크된 행인지 확인
                bool isChecked = Convert.ToBoolean(row.Cells["체크"].Value);
                if (isChecked)
                {
                    // 삭제할 행을 목록에 추가
                    rowsToRemove.Add(row);
                }
            }

            // 목록에 있는 행을 삭제
            foreach (DataGridViewRow row in rowsToRemove)
            {
                dataGridView2.Rows.Remove(row);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
    }
}

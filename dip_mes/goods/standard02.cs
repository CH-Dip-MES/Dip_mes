using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_mes.goods
{
    public partial class standard02 : UserControl
    {
        public standard02()
        {
            InitializeComponent();
        }

        private void standard02_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // bnss_registration 폼 인스턴스 생성
            bnss_registration registrationForm = new bnss_registration();

            // ShowDialog를 사용하여 팝업 창으로 표시
            registrationForm.ShowDialog();
        }
    }
}

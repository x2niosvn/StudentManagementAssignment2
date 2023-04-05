using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_StudentManagement
{
    public partial class LoginForm : Form
    {
        // Đánh dấu trạng thái đăng nhập
        private bool isLoggedIn = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click_1(object sender, EventArgs e)
        {
            isLoggedIn = true;
            label1.ForeColor = Color.RoyalBlue;
            label2.ForeColor = Color.RoyalBlue;
            


            if (txtUsername.Text == "")
            {
                label1.ForeColor = Color.Red;

            }
            else if (txtPassword.Text == "")
            {
                label2.ForeColor = Color.Red;

            }
            else
            {
                if (txtUsername.Text == "admin" && txtPassword.Text == "admin")
                {
                    // Đặt cờ là true khi đăng nhập thành công
                    isLoggedIn = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or Password does not match!");
                }

            }

        }

        // Trả về trạng thái đăng nhập
        public bool IsLoggedIn()
        {
            return isLoggedIn;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm_StudentManagement
{
    public partial class Form1 : Form
    {
        private LoginForm loginForm;
        List<Student> students = new List<Student>(); // List Data 

        List<ListViewItem> list = new List<ListViewItem>(); // List Row Item 

        public Form1()
        {
            InitializeComponent();
            string directoryPath = "C:\\StudentManagementData";
            string filePath = Path.Combine(directoryPath, "student_data.txt");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            Lv_Student.FullRowSelect = true;
            Lv_Student.GridLines = true;
            Lv_Student.Columns.Add("StudentCode", -2, HorizontalAlignment.Left);
            Lv_Student.Columns.Add("StudentName", -2, HorizontalAlignment.Left);
            Lv_Student.Columns.Add("Address", -2, HorizontalAlignment.Left);
            Lv_Student.Columns.Add("BirthDay", -2, HorizontalAlignment.Center);
            Lv_Student.Columns.Add("Gender", -2, HorizontalAlignment.Center);
            Lv_Student.Columns.Add("MathScore", -2, HorizontalAlignment.Center);
            Lv_Student.View = View.Details; // Phai co lenh nay thi View moi Hien Thi len Giao dien
            rdtMale.Checked = true;
            string[] lines = File.ReadAllLines("C:\\StudentManagementData\\student_data.txt");
            int numLines = lines.Length;

            for (int i = 0; i < numLines; i++)
            {
                string line = lines[i];
                string[] values = line.Split(',');
                Student student = new Student();
                student.StudentCode = values[0];
                student.Name = values[1];
                student.Address = values[2];
                student.BrithDay = values[3];
                student.Gender = (values[4] == "Male");
                student.MathScore = Convert.ToDouble(values[5]);
                students.Add(student);
                RefreshListView(students);
            }
        }

        private void ClickSaveStudent(object sender, EventArgs e)
        {
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tạo mới LoginForm
            LoginForm loginForm = new LoginForm();
            // Ẩn Form1
            this.Hide();

            // Hiển thị LoginForm và đợi người dùng đăng nhập
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Kiểm tra trạng thái đăng nhập
                if (loginForm.IsLoggedIn())
                {
                    this.Show();
                }
                else
                {
                    // Nếu đăng nhập không thành công, đóng ứng dụng
                    Application.Exit();
                }
            }
            else
            {
                // Nếu người dùng đóng LoginForm mà chưa đăng nhập
                Application.Exit();
            }
        }


        string ConvertGender(bool gender)
        {
            return gender ? "Male" : "Female";
        }


        void RefreshListView(List<Student> students)
        {

            list = new List<ListViewItem>();

            for (int i = 0; i < students.Count; i++)
            {

                Student student = students[i];
                ListViewItem studentItem = new ListViewItem(student.StudentCode, i);

                studentItem.SubItems.Add(student.Name);
                studentItem.SubItems.Add(student.Address + "");
                studentItem.SubItems.Add(student.BrithDay + "");
                studentItem.SubItems.Add(ConvertGender(student.Gender));
                studentItem.SubItems.Add(student.MathScore + "");
                list.Add(studentItem);


            }

            Lv_Student.Items.Clear();
            Lv_Student.Items.AddRange(list.ToArray());

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Khởi tạo các biến như bình thường
            String name = txtStudentName.Text;
            String address = txtAddress.Text;
            String math = txtMathScore.Text;
            String code = txtStudentCode.Text;
            String birthdayString = dtpBirthDay.Text;
            bool Gender = true;

            // Xác định giới tính của sinh viên
            if (rdtMale.Checked)
            {
                rdtFemale.Checked = false;
                Gender = true;
            }
            if (rdtFemale.Checked)
            {
                rdtMale.Checked = false;
                Gender = false;
            }

            // Kiểm tra các trường đã nhập có đầy đủ chưa
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(math) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(birthdayString))
            {
                MessageBox.Show("Please complete all fields!");
                return;
            }

            // Khởi tạo đối tượng sinh viên mới
            Student student = new Student();
            student.StudentCode = code;
            student.Name = name;
            student.Address = address;
            student.Gender = Gender;
            student.MathScore = Convert.ToDouble(math);
            student.BrithDay = birthdayString;

            // Thêm sinh viên vào danh sách sinh viên
            students.Add(student);

            // Refresh danh sách hiển thị trên ListView
            RefreshListView(students);

            // Ghi thông tin sinh viên vào tệp tin "student_data.txt"
            using (StreamWriter sw = new StreamWriter("C:\\StudentManagementData\\student_data.txt", true))
            {
                // Ghi thông tin sinh viên dưới dạng chuỗi với các trường ngăn cách nhau bằng dấu phẩy (,)
                sw.WriteLine("{0},{1},{2},{3},{4},{5}", code, name, address, birthdayString, ConvertGender(Gender), math);
            }
        }



        List<Student> listSearch = new List<Student>();
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if(keyword == "")
            {
                listSearch.Clear();
                listSearch = new List<Student>();
                RefreshListView(students);
            }else
            {
                foreach (Student student in students)
                {
                    if (keyword.Equals(student.Name))
                    {
                        listSearch.Add(student);
                    }
                }
                RefreshListView(listSearch);
            }
        }

        private void Lv_Student_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Lv_Student.SelectedItems.Count == 1)
            {

                var firstSelectedItem = Lv_Student.SelectedItems[0].Index;

                Student student = students[firstSelectedItem];


                txtStudentName.Text = student.Name;
                txtAddress.Text = student.Address;
                txtMathScore.Text = student.MathScore + "";
                txtStudentCode.Text = student.StudentCode;
                dtpBirthDay.Text = student.BrithDay;
                bool Gender = student.Gender;

                if (Gender)
                {
                    rdtFemale.Checked = false;
                    rdtMale.Checked = true;

                }

                if (!Gender)
                {
                    rdtFemale.Checked = true;
                    rdtMale.Checked = false;
                }

            }

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Lv_Student.SelectedItems.Count == 1)
            {
                var firstSelectedItem = Lv_Student.SelectedItems[0];
                var index = firstSelectedItem.Index;
                var studentToRemove = students[index];
                students.RemoveAt(index);
                Lv_Student.Items.RemoveAt(index);
                // Xóa thông tin sinh viên khỏi file txt
                string filePath = "C:\\StudentManagementData\\student_data.txt";
                string[] lines = File.ReadAllLines(filePath);
                File.WriteAllText(filePath, string.Empty);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var line in lines)
                    {
                        var student = line.Split(',');
                        if (student[0] != studentToRemove.StudentCode)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }


        private List<int> selectedIndexes = new List<int>();

        private void button2_Click(object sender, EventArgs e)
        {
            if (Lv_Student.SelectedItems.Count > 1 || Lv_Student.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select 1 student to edit!");
                return;
            }

            // Lưu trữ các chỉ mục được chọn vào danh sách tạm thời
            selectedIndexes.Clear();
            foreach (ListViewItem selectedItem in Lv_Student.SelectedItems)
            {
                selectedIndexes.Add(selectedItem.Index);
            }

            // Tiến hành cập nhật thông tin sinh viên
            foreach (int selectedIndex in selectedIndexes)
            {
                Student student = students[selectedIndex];
                student.Name = txtStudentName.Text;
                student.Address = txtAddress.Text;
                student.MathScore = int.Parse(txtMathScore.Text);
                student.StudentCode = txtStudentCode.Text;
                student.BrithDay = dtpBirthDay.Text;
                student.Gender = rdtMale.Checked;

                UpdateListViewItem(selectedIndex, student);

                // Cập nhật thông tin sinh viên vào file txt
                string filePath = "C:\\StudentManagementData\\student_data.txt";
                string[] lines = File.ReadAllLines(filePath);
                File.WriteAllText(filePath, string.Empty);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var line in lines)
                    {
                        var studentFromFile = line.Split(',');
                        if (studentFromFile[0] == student.StudentCode)
                        {
                            writer.WriteLine($"{student.StudentCode},{student.Name},{student.Address},{student.BrithDay},{ConvertGender(student.Gender)},{student.MathScore}");
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }

            MessageBox.Show("Student information has been successfully updated!");
        }


        private void UpdateListViewItem(int index, Student student)
        {
            Lv_Student.Items[index].SubItems[0].Text = student.StudentCode;
            Lv_Student.Items[index].SubItems[1].Text = student.Name;
            Lv_Student.Items[index].SubItems[2].Text = student.Address;
            Lv_Student.Items[index].SubItems[3].Text = student.BrithDay;
            Lv_Student.Items[index].SubItems[4].Text = ConvertGender(student.Gender);
            Lv_Student.Items[index].SubItems[5].Text = student.MathScore.ToString();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtStudentName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}

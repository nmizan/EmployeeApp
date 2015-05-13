using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace EmployeeManagementApp
{
    public partial class EmployeeUI : Form
    {
        public EmployeeUI()
        {
            InitializeComponent();

            ShowAllEmployee();
        }

     string connectionString = ConfigurationManager.ConnectionStrings["EmployeeManagement"].ConnectionString;

     bool isUpdateMode = false;
     private  int employeeId;

        private void saveButton_Click(object sender, EventArgs e)
        {

            string name = nameTextBox.Text;
            string email = emailTextBox.Text;
            float salary = float.Parse(salaryTextBox.Text);

            if (isUpdateMode)
            {
                SqlConnection connection = new SqlConnection(connectionString);


                string query = "UPDATE Employee SET Name='" + name + "',Email='"+email+"', Salary='" + salary + "'WHERE ID = '" + employeeId + "'";


                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();


                if (rowAffected > 0)
                {
                    MessageBox.Show("Infomation is Updated!");

                    saveButton.Text = "Save";
                    employeeId = 0;

                    isUpdateMode = false;
                    ShowAllEmployee();

                }
                else
                {
                    MessageBox.Show("Information is not Updated !");
                }

            }

            else
            {
                if (IsEmailExists(email))
                {
                    MessageBox.Show("Email Exist");
                    return;
                }


                SqlConnection connection = new SqlConnection(connectionString);


                string query = "INSERT INTO Employee values('" + name + "','" + email + "','" + salary + "')";


                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();


                if (rowAffected > 0)
                {
                    MessageBox.Show("Infomation is Inserted!");
                    ShowAllEmployee();

                }
                else
                {
                    MessageBox.Show("Information is not Inserted !");
                }
            
            }


            
                nameTextBox.Clear();
                emailTextBox.Clear();
                salaryTextBox.Clear();

            }
         
          
        public bool IsEmailExists(string email)
        {
           
            SqlConnection connection = new SqlConnection(connectionString);


            string query = "SELECT * FROM Employee WHERE Email= '" + email + "'";
            bool isEmailExists = false;

            SqlCommand command = new SqlCommand(query, connection);
           
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
           
            while(reader.Read())
            {
               isEmailExists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return isEmailExists;
        }


        //public void LoadEmployeeListview(List<Employee> employee)
        //{
            
        //    employeeListView.Items.Clear();
        //    foreach (var Employee in employee)
        //    {
        //        ListViewItem item = new ListViewItem(Employee.ID.ToString());
        //        item.SubItems.Add(Employee.name);
        //        item.SubItems.Add(Employee.email);
        //        item.SubItems.Add(Employee.salary.ToString());

        //        employeeListView.Items.Add(item);
        //    }
        
        //}
   
    
    public void ShowAllEmployee()
    {

        employeeListView.Items.Clear();

     SqlConnection connection = new SqlConnection(connectionString);


      string query =  "SELECT * FROM Employee";


          SqlCommand command = new SqlCommand(query, connection);
           connection.Open();
         SqlDataReader reader = command.ExecuteReader();
         List<Employee> employeeList = new List<Employee>();
            while(reader.Read())
            {
                Employee employee = new Employee();

                employee.ID = int.Parse(reader["ID"].ToString());
                employee.name = reader["name"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = float.Parse(reader["salary"].ToString());

                employeeList.Add(employee);
            }
           
            reader.Close();
            connection.Close();


            foreach (var Employee in employeeList)
            {
                ListViewItem item = new ListViewItem(Employee.ID.ToString());
                item.SubItems.Add(Employee.name);
                item.SubItems.Add(Employee.email);
                item.SubItems.Add(Employee.salary.ToString());

                employeeListView.Items.Add(item);
            }
            
   
    }

    private void employeeListView_DoubleClick(object sender, EventArgs e)
    {
        ListViewItem item = employeeListView.SelectedItems[0];
        int ID = int.Parse(item.Text.ToString());

        
        Employee employee = GetEmployeeByID(ID);

        if (employee != null)
        {
            isUpdateMode = true;
            saveButton.Text = "Update";
           employeeId = employee.ID;

           nameTextBox.Text = employee.name;
           emailTextBox.Text = employee.email;
           salaryTextBox.Text = Convert.ToString(employee.salary);


        }
    }


  public Employee  GetEmployeeByID(int id)
    {
        SqlConnection connection = new SqlConnection(connectionString);
    

        string query = "SELECT * FROM Employee WHERE ID='" + id + "'";


        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        List<Employee> employeeList = new List<Employee>();
        while (reader.Read())
        {
            Employee employee = new Employee();

            employee.ID = int.Parse(reader["ID"].ToString());
            employee.name = reader["name"].ToString();
            employee.email = reader["email"].ToString();
            employee.salary = float.Parse(reader["salary"].ToString());

            employeeList.Add(employee);
        }

        reader.Close();
        connection.Close();
       return  employeeList.FirstOrDefault();
    }
    }


}




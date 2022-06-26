﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollMultithreading
{
    public class EmployeePayrollOperations
    {
        public static string connectionString = @"data source = .; database = payroll; integrated security = true";

        SqlConnection connection = new SqlConnection(connectionString);

        public List<EmployeeDetails> employeePayrollDetailList = new List<EmployeeDetails>();

        public void AddEmployeeToPayroll(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Console.WriteLine("Employee Being added : " + employeeData.EmployeeName);
                this.AddEmployeePayroll(employeeData);
                Console.WriteLine("Employee added : " + employeeData.EmployeeName);
            });
            Console.WriteLine(this.employeePayrollDetailList.ToString());
        }

        public void AddEmployeeToPayrollWithThread(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Employee Being added : " + employeeData.EmployeeName);
                    this.AddEmployeePayroll(employeeData);
                    Console.WriteLine("Employee added : " + employeeData.EmployeeName);
                });
                thread.Start();
            });
        }

        public void AddEmployeePayroll(EmployeeDetails emp)
        {
            employeePayrollDetailList.Add(emp);
        }

        public void AddEmployeeToPayrollDataBase(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Console.WriteLine("Employee being added : " + employeeData.EmployeeName);
                this.AddEmployeePayrollDatabase(employeeData);
                Console.WriteLine("Employee added : " + employeeData.EmployeeName);
            });
        }

        public void AddEmployeePayrollDatabase(EmployeeDetails employeeDetails)
        {
            SqlCommand command = new SqlCommand("SPInsertData", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmployeeId", employeeDetails.EmployeeID);
            command.Parameters.AddWithValue("@EmployeeName", employeeDetails.EmployeeName);
            command.Parameters.AddWithValue("@phoneNumber", employeeDetails.PhoneNumber);
            command.Parameters.AddWithValue("@Address", employeeDetails.Address);
            command.Parameters.AddWithValue("@Department", employeeDetails.Department);
            command.Parameters.AddWithValue("@gender", employeeDetails.Gender);
            command.Parameters.AddWithValue("@basicpay", employeeDetails.BasicPay);
            command.Parameters.AddWithValue("@deductions", employeeDetails.Deductions);
            command.Parameters.AddWithValue("@taxablepay", employeeDetails.TaxablePay);
            command.Parameters.AddWithValue("@tax", employeeDetails.Tax);
            command.Parameters.AddWithValue("@netpay", employeeDetails.NetPay);
            command.Parameters.AddWithValue("@city", employeeDetails.City);
            command.Parameters.AddWithValue("@country", employeeDetails.Country);
            connection.Open();

            var result = command.ExecuteNonQuery();

            Console.WriteLine(result);
            connection.Close();
        }
    }
}

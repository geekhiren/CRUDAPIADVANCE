using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using EMPAPI.Models;
using System.Web;

namespace EMPAPI.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeesController : ApiController
    {
        //connection string
        EMPEntities db;
        public EmployeesController()
        {
            db = new EMPEntities();
        }

        //get employee list

        [Route("GetEmployee")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployee()
        {
            return db.Employees;
        }

        //get employee list base id

        [Route("GetEmployeeByID")]
        [HttpGet]
        public Employee GetEmployeeByID(int id)
        {
            return db.Employees.Where(x => x.EmployeeID.Equals(id)).FirstOrDefault();
        }

        //insert data in employee table

        [Route("AddNewEmployee")]
        [HttpPost]
        public Employee AddNewEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(emp);
                db.SaveChanges();

                return db.Employees.ToList().OrderByDescending(a => a.EmployeeID).FirstOrDefault();
            }
            else
            {
                return new Employee();
            }
        }

        //update data on employee table

        [Route("UpdateEmployeeData")]
        [HttpPost]
        public Employee UpdateEmployeeData(Employee emp)
        {
            var empReco = db.Employees.Where(a => a.EmployeeID.Equals(emp.EmployeeID)).FirstOrDefault();
            if (empReco != null)
            {
                empReco.EmployeeID = emp.EmployeeID;
                empReco.EmployeeName = emp.EmployeeName;
                empReco.Department = emp.Department;
                empReco.DateOfJoining = emp.DateOfJoining;
                empReco.PhotoFile = emp.PhotoFile;

                db.Entry(empReco).State = EntityState.Modified;
                db.SaveChanges();

                return db.Employees.Where(a => a.EmployeeID.Equals(emp.EmployeeID)).FirstOrDefault();
            }
            else
            {
                return new Employee();
            }
        }

        //delete data on employee base on id

        [Route("DeleteEmployeeData")]
        [HttpGet]
        public string DeleteEmployeeData(int id)
        {
            var empReco = db.Employees.Where(a => a.EmployeeID.Equals(id)).FirstOrDefault();
            if (empReco != null)
            {
                db.Employees.Remove(empReco);
                db.SaveChanges();
                return "Record Deleted SuccessFully!";
            }
            else
            {
                return "Some Thing Want Wrong!";
            }
        }

        //get all department name
        [Route("GetAllDepartment")]
        [HttpGet]
        public IQueryable<string> GetAllDepartment()
        {
            var result = db.Departments.Select(x => x.DepartmentName);
            return result;
        }

        //save photos

        [Route("SavePhoto")]
        [HttpPost]
        public string SavePhoto()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var pysicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + fileName);

                postedFile.SaveAs(pysicalPath);
               
                return fileName;
               
            }
            catch(Exception)
            {
                return "Some Thing Want Wrong!";
            }
        }
    }
}
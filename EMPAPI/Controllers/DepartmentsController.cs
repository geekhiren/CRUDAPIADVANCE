using EMPAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace EMPAPI.Controllers
{
    [RoutePrefix("Department")]
    public class DepartmentsController : ApiController
    {
        //connection string
        EMPEntities db;
        public DepartmentsController()
        {
            db = new EMPEntities();
        }

        //get department list

        [Route("GetDepartment")]
        [HttpGet]
        public IEnumerable<Department> GetDepartment()
        {
            return db.Departments.ToList();
        }

        //get department base on id

        [Route("GetDepartmentByID")]
        [HttpGet]
        public Department GetDepartmentByID(int id)
        {
            return db.Departments.Where(a => a.DepartmentID.Equals(id)).FirstOrDefault();
        }

        //insert on data department table

        [Route("AddNewDepartment")]
        [HttpPost]
        public Department AddNewDepartment(Department dep)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(dep);
                db.SaveChanges();

                return db.Departments.ToList().OrderByDescending(a => a.DepartmentID).FirstOrDefault();
            }
            else
            {
                return new Department();
            }

        }

        // update data department table

        [Route("UpdateDepartmentData")]
        [HttpPost]
        public Department UpdateDepartmentData(Department dep)
        {
            var depReco = db.Departments.Where(a => a.DepartmentID.Equals(dep.DepartmentID)).FirstOrDefault();
            if (depReco != null)
            {
                depReco.DepartmentID = dep.DepartmentID;
                depReco.DepartmentName = dep.DepartmentName;

                db.Entry(depReco).State = EntityState.Modified;
                db.SaveChanges();

                return db.Departments.Where(a => a.DepartmentID.Equals(dep.DepartmentID)).FirstOrDefault();
            }
            else
            {
                return new Department();
            }
        }

        //delete department base id

        [Route("DeleteDepartmentData")]
        [HttpGet]
        public string DeleteDepartmentData(int id)
        {
            var depReco = db.Departments.Where(a => a.DepartmentID.Equals(id)).FirstOrDefault();
            if (depReco != null)
            {
                db.Departments.Remove(depReco);
                db.SaveChanges();
                return "Record Deleted SuccessFully!";
            }
            else
            {
                return "Some Thing Want Wrong!";
            }
        }
    }
}
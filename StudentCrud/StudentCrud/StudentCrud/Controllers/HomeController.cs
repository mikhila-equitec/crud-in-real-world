using StudentCrud.Models;
using StudentCrud.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data;
using System.Web.UI;
using System.Net;
using System.Drawing.Printing;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace StudentCrud.Controllers
{
    public class HomeController : Controller
    {

        StudentRepocs std = new StudentRepocs();
        string con = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        //Insert
        public ActionResult Insert()
        {

            using (var conn = new SqlConnection(con))
            {
                var ex = conn.Query<StudentModel>("select DeptId, DeptName from Departments").ToList();
                ViewBag.dept = ex;
            }
            return View();
        }

        //[HttpPost]
        //public ActionResult Insert(StudentModel sm)
        //{
        //    try
        //    {
        //        int count = 0;
        //        using (var conn = new SqlConnection(con))
        //        {
        //            var p = new DynamicParameters();
        //            p.Add("@RollNo", sm.RollNo);
        //            count = conn.ExecuteScalar<int>("Check_Rollnum", p, commandType: CommandType.StoredProcedure);
        //        }
        //        if (count > 0)
        //        {
        //            //throw new Exception("Roll No exists");
        //            TempData["error"] = "Duplicate Roll Number";
        //            return View(sm);
        //        }
        //        else
        //        {

        //            std.insertData(sm);
        //            TempData["success"] = "Added details successfully";
        //            return RedirectToAction("StudentShow");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        std.Error(e);
        //        TempData["error"] = "Error: " + e;
        //        return View(sm);
        //    }

        //}
        [HttpPost]
        public ActionResult Insert(StudentModel sm)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    std.insertData(sm);
                    TempData["success"] = "Added details successfully";
                    return RedirectToAction("StudentShow");
                }
                return RedirectToAction("StudentShow");
            }

            catch (Exception e)
            {
                std.Error(e);
                TempData["error"] = "Duplicate Roll no  "  ;
                return RedirectToAction("Insert");
            }

        }

        //Delete
        public ActionResult Delete(int id)
        {
            try
            {
                std.deleteData(id);
                TempData["delSuccess"] = "Deleted data";
                return RedirectToAction("StudentShow");
            }
            catch(Exception e)
            {
                std.Error(e);
                TempData["delError"] = "Error: " + e;
                return View();
            }
           
        }

        //Deleted
          public ActionResult ShowDeleted(int num = 1, int size = 5)
        {

            using (var conn = new SqlConnection(con))
            {

                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", num);
                parameters.Add("@PageSize", size);
                int count = (num - 1) * size + 1;
                ViewBag.c = count;
                using (var multi = conn.QueryMultiple("DeletePagination", parameters, commandType: CommandType.StoredProcedure))
                {
                    var students = multi.Read<StudentModel>().ToList();
                    var totalRecords = multi.Read<int>().FirstOrDefault();

                    ViewBag.total = (int)Math.Ceiling((double)totalRecords / size);
                    ViewBag.current = num;

                    return View(students);
                }
            }
        }
        //Restore
        public ActionResult Restore(int id)
        {
            try
            {
                std.restoreData(id);
                TempData["resSuccess"] = "Data Restored successfully";
                return RedirectToAction("StudentShow");
            }
            catch (Exception e)
            {
                std.Error(e);
                TempData["resError"] = "Error"+e;
                return View();
            }
        }

        //View single data
        public ActionResult ViewData(int id)
        {

            return View(std.viewData(id));
        }

        public ActionResult ViewDeleted(int id)
        {

            return View(std.viewDeletedData(id));
        }

        //edit
        public ActionResult Edit(int id)
        {

            using (var conn = new SqlConnection(con))
            {
                string sql = "select DeptId, DeptName from Departments ";
                var ex = conn.Query<StudentModel>(sql).ToList();
                ViewBag.dep = ex;
            }
            ViewBag.id = id;
            return View(std.viewData(id));
        }
       
 
        [HttpPost]
        public JsonResult Edit(StudentModel student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    std.updateData(student);
                    TempData["EditSuccess"] = "Edited data successfully";
                    return Json(new { success = true, message = "Updated" });
                }
                return Json(new { success = false, message = "Error"  });
            }
            catch (Exception ex)
            {
                std.Error(ex);
                return Json(new { success = false, message = "Roll Number already exists"});
            }
        }


        public ActionResult StudentShow(int pgnum = 1, int pgSize = 5)
        {
           
                    using (var conn = new SqlConnection(con))
                    {
                        conn.Open();
                        var parameters = new DynamicParameters();
                        parameters.Add("@PageNumber", pgnum);
                        parameters.Add("@PageSize", pgSize);
                        int count = (pgnum - 1) * pgSize + 1;
                        ViewBag.c = count;
                        using (var multi = conn.QueryMultiple("Pagination", parameters, commandType: CommandType.StoredProcedure))
                        {
                            var students = multi.Read<StudentModel>().ToList();
                            var totalRecords = multi.Read<int>().FirstOrDefault();

                            ViewBag.totalPg = (int)Math.Ceiling((double)totalRecords / pgSize);
                            ViewBag.curPg = pgnum;

                            return View(students);
                        }
                    }
    }


    }
}
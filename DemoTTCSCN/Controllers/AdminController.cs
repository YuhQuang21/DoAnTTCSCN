using DemoTTCSCN.DAO;
using DemoTTCSCN.Dto;
using DemoTTCSCN.Models;
using DemoTTCSCN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoTTCSCN.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            return View();
        }
        public async Task<ActionResult> GetListStudent()
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            var students = await StudentDAO.Instance.GetList();
            List<StudentDto> model = new List<StudentDto>();
            foreach (var item in students)
            {
                model.Add(new StudentDto
                {
                    IdStudent = item.IDSinhVien,
                    Name = item.HoTen
                });

            }
            return View(model);
        }
        public async Task<ActionResult> GetStudentById (string Id)
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null || Id == null)
            {
                return View("Error");
            }
            var detail = await StudentDAO.Instance.GetStudentByID(Id);
            return View(detail);
        }
        public async Task<ActionResult> UpdateStudent(FormCollection fields)
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            var model = new Student
            {
                IDSinhVien = fields["idSinhVien"],
                HoTen = fields["hoTen"],
                NgaySinh = Convert.ToDateTime(fields["ngaySinh"]),
                GioiTinh = fields["gioiTinh"],
                QueQuan = fields["queQuan"],
                DiaChiHT = fields["diaChi"],
                KhoaDKi = fields["khoaDangKy"],
                SoTCDaDki = Convert.ToInt32(fields["soTinChiDaDangKy"]),
                SoTCDaDat = Convert.ToInt32(fields["soTinChiDaDat"]),
                DiemTichLuy = Convert.ToDouble(fields["diemTichLuy"]),
                IDLop = fields["idClass"]
            };
            Object obj = model.IDSinhVien;
            var result = await StudentDAO.Instance.Update(model);
            if (result == 1)
            {
                SetAlert("Sửa thành công", 1);
            }
            else SetAlert("Sửa thất bại", 1);
            return RedirectToRoute("Detail", new { Id = model.IDSinhVien }); 
        }
        protected void SetAlert(string message, int type)
        {
            TempData["AlertMessage"] = message;
            if (type == 1)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == 2)
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == 3)
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
    }
}
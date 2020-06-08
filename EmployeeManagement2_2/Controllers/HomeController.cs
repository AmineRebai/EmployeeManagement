using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement2_2.Models;
using EmployeeManagement2_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace EmployeeManagement2_2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private IHostingEnvironment hostingEnvironment;
        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View(_employeeRepository.GetEmployees());
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            //throw new Exception("Error in details view");
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            return View(employee);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateVM model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessuploadedFile(model);
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Create(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Email = employee.Email,
                Name = employee.Name,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string  filePath = Path.Combine(hostingEnvironment.WebRootPath, "Images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                     employee.PhotoPath = ProcessuploadedFile(model);
                }
                
                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View();
        }

        private string ProcessuploadedFile(EmployeeCreateVM model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using(var fileStream=new FileStream(filePath, FileMode.Create))
                  model.Photo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}

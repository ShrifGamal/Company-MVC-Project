using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepositorie _employeeRepositorie;
        //public readonly IDepartmentRepositorie _departmentRepositorie;
        private readonly IMapper _mapper;

        public EmployeesController(
            //IEmployeeRepositorie employeeRepositorie ,
            //IDepartmentRepositorie departmentRepositorie,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_departmentRepositorie = departmentRepositorie;
            _mapper = mapper;
            //_employeeRepositorie = employeeRepositorie;
        }
        public async Task<IActionResult> Index(string InputSearch)
        {
            var employee = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(InputSearch))
            {
                 employee = await _unitOfWork.EmployeeRepositorie.GetAllAsync();

            }
            else
            {
                 employee = await _unitOfWork.EmployeeRepositorie.GetByNameAsync(InputSearch);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employee);

            return View(Result);
        }

        public async Task<IActionResult> Create()
        {
            var department = await _unitOfWork.DepartmentRepositorie.GetAllAsync();
            ViewData["department"] = department;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model) 
        {
            
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSetting.Upload(model.Image, "Images");
                }
                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepositorie.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id )
        {
            if (id is null) return BadRequest();

            var employee = await _unitOfWork.EmployeeRepositorie.GetAsync(id.Value);

            if (employee is null) return NotFound();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View( employeeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var department = await _unitOfWork.DepartmentRepositorie.GetAllAsync();
            ViewData["department"] = department;
            if (id is null) return BadRequest();

            var employee = await _unitOfWork.EmployeeRepositorie.GetAsync(id.Value);

            if (employee is null) return NotFound();
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id , EmployeeViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid) 
                {
                    if(model.ImageName is not null)
                    {
                        DocumentSetting.Delete(model.ImageName, "Images");
                    }

                    if(model.Image is not null)
                    {
                        model.ImageName = DocumentSetting.Upload(model.Image, "Images");
                    }

                    var employee = _mapper.Map<Employee>(model);
                    _unitOfWork.EmployeeRepositorie.Update(employee);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            { 
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var employee = await _unitOfWork.EmployeeRepositorie.GetAsync(id.Value);

            if (employee is null) return NotFound();
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel); ;
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var employee = _mapper.Map<Employee>(model);
                    _unitOfWork.EmployeeRepositorie.Delete(employee);
                    var count = await _unitOfWork.CompleteAsync();
                    if(count > 0)
                    {
                        if (model.ImageName is not null)
                        {
                            DocumentSetting.Delete(model.ImageName, "Images");
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty , ex.Message);
            }
            return View(model);
        }


    }
}

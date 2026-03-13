using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepositorie _departmentRepositorie;

        public DepartmentsController( IUnitOfWork unitOfWork) //IDepartmentRepositorie departmentRepositorie)
        {
            //_departmentRepositorie = departmentRepositorie;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var department =await _unitOfWork.DepartmentRepositorie.GetAllAsync();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepositorie.AddAsync(model);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id )
        {
            if(id is null) return BadRequest();
            var department = await _unitOfWork.DepartmentRepositorie.GetAsync(id.Value);
            if (department is null) return NotFound();
            return View(department );
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var department = await  _unitOfWork.DepartmentRepositorie.GetAsync(id.Value);
            if(department is null) return NotFound();
            return View(department);
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int? id , Department model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepositorie.Update(model);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
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

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id is null) return BadRequest();
            var department = await _unitOfWork.DepartmentRepositorie.GetAsync(id.Value);
            if(department is null) return NotFound();
            return View(department);
            //return Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? Id, Department model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepositorie.Delete(model);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError (string.Empty , ex.Message);
            }
            return View(model);
        }
    }
}

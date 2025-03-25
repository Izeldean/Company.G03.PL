using AutoMapper;
using Company.G03.BLL;
using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repersitorties;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Company.G03.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        //public IDepartmentRepository _departmentRepository { get; }
        private readonly IUnitWork _unitOfWork;
        public EmployeeController(IUnitWork unitOfWork ,
            IMapper mapper)
        {
            //_employeeRepository = Employee;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
           _mapper = mapper;
        }


        [HttpGet]
        /* Search method is with in the index */
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                 employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            
            
            
            }
            // Dicttionary:
            // ViewData : Transfer Extra Information From Controller (Action) To view
            ViewData["Message"] = "Hello From ViewData";
            // ViewBag
            ViewBag.Message = new { Message = "Hello From ViewBag" };
            // TempData 

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
           var departments= await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();

        }
        [HttpPost]
        public async Task< IActionResult> Create(CreateEmployeedto model)
        {
            if (ModelState.IsValid)
            {
                //try
                //{

                    if (model.Image is not null)
                    {
                      model.ImageName =   DocumentSetting.UploadFile(model.Image, "images");
                    }

                    //Manual Mapping 
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Address = model.Address,
                    //    Age = model.Age,
                    //    CreateAt = model.CreateAt,
                    //    HiringDate = model.HiringDate,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary, 
                    //    DepartmentId=model.DepartmentId
                    //};
                    //-----------------------------------------------

                    //Automatic Mapping
                    // Need Dependency injection
                    var employee=  _mapper.Map<Employee>(model);

                 await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {

                    

                         TempData["Message"] = "Employee is created";
                        return RedirectToAction("Index");
                    }
                }
                //catch (Exception ex)
                //{

                //    ModelState.AddModelError("", ex.Message);
                //}

            //}
            return View(model);


        }

        //  if(id is null) return BadRequest("Invalid ID");
        //var department = _UnitOfWork.DepartmentRepository.Get(id.Value);
        //if(department is null) return NotFound(new { statusCode=404, message=$"Department with Id {id} is not found"});
        //    return View(viewName, department);

    [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) { return BadRequest("Invalid Id"); }

         
            var employees = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employees == null) { return NotFound(new { statusCode = 404, message = $"Emplpyee with Id :{id} is not found" }); }
        
         
            return View(viewName, employees);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
          
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department {id} not found" });
            var employeeDto = new CreateEmployeedto()
            {

                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Phone = employee.Phone,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId

            };
            var dto = _mapper.Map<CreateEmployeedto>(employee);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeedto model, string viewName="Edit")
        {
            if (ModelState.IsValid)
            {

                if (model.ImageName is not null  && model.Image is not null) {
                    DocumentSetting.DeleteFile(model.ImageName, "images");
                
                }


                if (model.Image is not null)
                {
                model.ImageName=    DocumentSetting.UploadFile(model.Image, "images");

                }
              

                //if (id != model.Id) return BadRequest();

                //var employee = new Employee()
                //{   Id=id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId

                //};
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(viewName,model);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
          
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeedto model)
        {


            if (ModelState.IsValid)
            {

                ////if (id != model.Id) return BadRequest();
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId= model.DepartmentId


                //};

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    if (model.ImageName is not null )
                    {
                        DocumentSetting.DeleteFile(model.ImageName, "images");

                    }
                    return RedirectToAction(nameof(Index));

                }




            }
            return View(model);
        }


    }
}

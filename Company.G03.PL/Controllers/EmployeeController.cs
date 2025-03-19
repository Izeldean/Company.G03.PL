using Company.G03.BLL.Interfaces;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Microsoft.AspNetCore.Mvc;



namespace Company.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public IDepartmentRepository _departmentRepository { get; }

        public EmployeeController(IEmployeeRepository Employee, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = Employee;
            _departmentRepository = departmentRepository;
        }


        [HttpGet]
        /* Search method is with in the index */
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();

            }
            else
            {
                 employees = _employeeRepository.GetByName(SearchInput);
            
            
            
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
        public IActionResult Create()
        {
           var departments= _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();

        }
        [HttpPost]
        public IActionResult Create(CreateEmployeedto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = new Employee()
                    {
                        Name = model.Name,
                        Address = model.Address,
                        Age = model.Age,
                        CreateAt = model.CreateAt,
                        HiringDate = model.HiringDate,
                        Email = model.Email,
                        IsActive = model.IsActive,
                        IsDeleted = model.IsDeleted,
                        Phone = model.Phone,
                        Salary = model.Salary, 
                        DepartmentId=model.DepartmentId
                    };
                    var count = _employeeRepository.Add(employee);
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is created";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex) { 
                
                ModelState.AddModelError("",ex.Message);
                }

            }
            return View(model);


        }
        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) { return BadRequest("Invalid Id"); }
            var employees = _employeeRepository.Get(id.Value);

            if (employees == null) { return NotFound(new { statusCode = 404, message = $"Emplpyee with Id :{id} is not found" }); }
            return View(viewName, employees);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
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
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeedto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest();

                var employee = new Employee()
                {   Id=id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    DepartmentId = model.DepartmentId

                };

                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeedto model)
        {


            if (ModelState.IsValid)
            {

                //if (id != model.Id) return BadRequest();
                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    DepartmentId= model.DepartmentId
                   

                };
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));

                }




            }
            return View(model);
        }


    }
}

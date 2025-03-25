using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repersitorties;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitWork _UnitOfWork;

        public DepartmentController(IUnitWork unitOfWork) {

            //_departmentRepository = departmentRepository;
            //UnitOfWork = unitOfWork;
            _UnitOfWork = unitOfWork;
        }
        [HttpGet] // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
          
         var department= await _UnitOfWork.DepartmentRepository.GetAllAsync();
            return View(department);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = new Department()
                    {
                        Code = model.Code,
                        Name = model.Name,
                        CreateAt = model.CreateAt
                    };
                  await  _UnitOfWork.DepartmentRepository.AddAsync(department);
                    var count = await _UnitOfWork.CompleteAsync();

                    if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create department.");
                    }
                }
                catch (Exception ex)
                {
                    // Log exception
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName="Details") { 
        if(id is null) return BadRequest("Invalid ID");
         var department=  await _UnitOfWork.DepartmentRepository.GetAsync(id.Value);
        if(department is null) return NotFound(new { statusCode=404, message=$"Department with Id {id} is not found"});
            return View(viewName,department);
        }
        
		[HttpGet]
		public async Task<IActionResult> EditAsync(int? id)
		{
			if (id is null) return BadRequest("Invalid ID");

			var department = await _UnitOfWork.DepartmentRepository.GetAsync(id.Value);
			if (department is null)
				return NotFound(new { statusCode = 404, message = $"Department with Id {id} is not found" });
            var departmentDto = new CreateDepartmentDto()
            {
                Code=department.Code,
                Name=department.Name,
                CreateAt= department.CreateAt
            };
			return View(departmentDto); // Load the Edit view with the department data
		}


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Edit([FromRoute] int id, UpdateDepartmentdto model)
		//{
		//    if (ModelState.IsValid)
		//    {

		//        var department = new Department(){ Id=id, Name=model.Name,
		//        Code= model.Code , CreateAt= model.CreateAt};
		//        var count = _departmentRepository.Update(department);
		//        if (count > 0) { return RedirectToAction(nameof(Index)); }
		//    }



		//    return View(model);

		//}




		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
		{
			if (ModelState.IsValid)
			{
                var department = new Department()
                {   Id=id,
                    Code= model.Code,
                    Name=model.Name,
                    CreateAt=model.CreateAt

                };

                _UnitOfWork.DepartmentRepository.Update(department);
                var count = await _UnitOfWork.CompleteAsync();
				if (count > 0) { return RedirectToAction(nameof(Index)); }
			}

			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id is null) return BadRequest("Invalid ID");

			var department = await _UnitOfWork.DepartmentRepository.GetAsync(id.Value);
			if (department is null)
				return NotFound(new { statusCode = 404, message = $"Department with Id {id} is not found" });

			// Convert Department to CreateDepartmentDto
			var departmentDto = new CreateDepartmentDto()
			{
				Code = department.Code,
				Name = department.Name,
				CreateAt = department.CreateAt
			};

			return View("Delete", departmentDto); // Pass the correct model
		}

		//[HttpGet]
		//      public IActionResult Delete(int? id)
		//      {
		//          //if (id is null) return BadRequest("Invalid ID");

		//          //var department = _departmentRepository.Get(id.Value);
		//          //if (department is null)
		//          //    return NotFound(new { statusCode = 404, message = $"Department with Id {id} is not found" });

		//          return    Details(id, "Delete"); 
		//      }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
				{
					Id = id,
					Code = model.Code,
                    Name=model.Name,
                    CreateAt= model.CreateAt

                };
                _UnitOfWork.DepartmentRepository.Delete(department);
                var count = await _UnitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }




    }
}

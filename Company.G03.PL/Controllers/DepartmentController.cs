using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repersitorties;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository) {

            _departmentRepository = departmentRepository;


        }
        [HttpGet] // GET: /Department/Index
        public IActionResult Index()
        {
          
         var department= _departmentRepository.GetAll();
            return View(department);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        
        }


        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
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

                    var count = _departmentRepository.Add(department);

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
        public IActionResult Details(int? id, string viewName="Details") { 
        if(id is null) return BadRequest("Invalid ID");
         var department=   _departmentRepository.Get(id.Value);
        if(department is null) return NotFound(new { statusCode=404, message=$"Department with Id {id} is not found"});
            return View(viewName,department);
        }
        
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id is null) return BadRequest("Invalid ID");

			var department = _departmentRepository.Get(id.Value);
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
		public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
		{
			if (ModelState.IsValid)
			{
                var department = new Department()
                {   Id=id,
                    Code= model.Code,
                    Name=model.Name,
                    CreateAt=model.CreateAt

                };

                
				var count = _departmentRepository.Update(department);
				if (count > 0) { return RedirectToAction(nameof(Index)); }
			}

			return View(model);
		}
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id is null) return BadRequest("Invalid ID");

			var department = _departmentRepository.Get(id.Value);
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
        public IActionResult Delete(int id, CreateDepartmentDto model)
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
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }




    }
}

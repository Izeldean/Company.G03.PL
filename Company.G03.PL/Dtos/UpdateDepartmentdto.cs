﻿using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.Dtos
{
    public class UpdateDepartmentdto
    {

        [Required(ErrorMessage = "Name is Required !")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code is Required !")]
        public string Code { get; set; }
        [Required(ErrorMessage = "CreatAt is Required !")]
        public DateTime CreateAt { get; set; }
    }
}

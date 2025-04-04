using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.Dtos
{
	public class ResetPassworddto
	{


		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		[Required(ErrorMessage = "Confirmed Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match")]
		public string ConfirmPassword { get; set; }
	}
}

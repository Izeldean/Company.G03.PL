using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.Dtos
{
	public class ForgetPassworddto
	{
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress]
		public string Email { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Templateprj.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = " ")]
		[Display(Name = "Username")]
		[StringLength(16)]
		public string Username { get; set; }

		[Required(ErrorMessage = " ")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[StringLength(16)]
		public string Password { get; set; }
	}
	
	public class VerifyAccountModel
	{
		public string OtpSrc { get; set; }
		public string PhoneEmail { get; set; }

		[Display(Name = "OTP")]
		[Required(ErrorMessage = " ")]
		[StringLength(16)]
		public string OTP { get; set; }
	}

	public class FirstTimeLoginModel
	{

		[Required(ErrorMessage = " ")]
		[Display(Name = "Security Question")]
		public int SelectedSecurityQuestion { get; set; }
		public SelectList SecurityQuestions { get; set; }

		[StringLength(50)]
		[Display(Name = "Answer")]
		[Required(ErrorMessage = " ")]
		public string Answer { get; set; }

		[StringLength(16)]
		[Required(ErrorMessage = " ")]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		[RegularExpression(@"^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])(?=\S*?[!@#$%^&*_+-])\S{8,}$", ErrorMessage = "Your Passwrod is Weak")]
		public string NewPassword { get; set; }

		[StringLength(16)]
		[Required(ErrorMessage = " ")]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Doesn't match with New Password")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

	}

	//For both ForgotPwd and ChangePwd
	public class AuthenticateUserModel
	{
		[Required(ErrorMessage = " ")]
		[Display(Name = "Username")]
		[StringLength(16)]
		public string Username { get; set; }

		[Required(ErrorMessage = " ")]
		[Display(Name = "Security Question")]
		public int SelectedSecurityQuestion { get; set; }
		public SelectList SecurityQuestions { get; set; }

		[Required(ErrorMessage = " ")]
		[Display(Name = "Answer")]
		[StringLength(50)]
		public string Answer { get; set; }

		[Required(ErrorMessage = " ")]
		[Display(Name = "Old Password")]
		[StringLength(16)]
		public string OldPwd { get; set; }
		
	}

	public class ChangePasswordModel
	{
		[StringLength(16)]
		[Display(Name = "OTP")]
		[Required(ErrorMessage = " ")]
		public string OTP { get; set; }


		[StringLength(16)]
		[Required(ErrorMessage = " ")]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		[RegularExpression(@"^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])(?=\S*?[!@#$%^&*_+-])\S{8,}$", ErrorMessage = "Your Passwrod is Weak")]
		public string NewPassword { get; set; }


		[StringLength(16)]
		[Required(ErrorMessage = " ")]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Doesn't match with New Password")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

	}

	public class VerifySecurityQnModel
	{
		[Required(ErrorMessage = " ")]
		[Display(Name = "Security Question")]
		public int SelectedSecurityQuestion { get; set; }

		public SelectList SecurityQuestions { get; set; }

		[Required(ErrorMessage = " ")]
		[Display(Name = "Answer")]
		[StringLength(50)]
		public string Answer { get; set; }

    }

}
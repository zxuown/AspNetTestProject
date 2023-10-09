using System.ComponentModel.DataAnnotations;

namespace RV211AspNet.Models.Forms;

public class RegisterForm
{
	[Required]
	public string Username { get; set; }
	[Required]
	[EmailAddress]
	public string Login { get; set; }
	[Required]
	public string Password { get; set; }
}

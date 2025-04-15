﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace Motosfera.Models.ViewModels
{
	public class LoginViewModel
	{
		[EmailAddress]
		public required string Email { get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }
		
		public bool RememberMe { get; set; }
	}
}

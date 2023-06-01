using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WeatherApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string Name { get; set; } = string.Empty;

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]

	public string City { get; set; } = string.Empty;
	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string Country { get; set; } = string.Empty;
}


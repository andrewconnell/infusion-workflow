using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnonReadSecureWriteWeb {
  public class Customer {
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string FirstName { get; set; }

    [StringLength(100)]
    [Required]
    public string LastName { get; set; }

    [StringLength(200, MinimumLength = 10)]
    [Required]
    public string Email { get; set; }

    [StringLength(5000)]
    public string SpContextToken { get; set; }
  }
}
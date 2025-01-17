using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace StudentCrud.Models
{
    public class StudentModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z'’\- ]*$",ErrorMessage ="Invalid characters in name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "Invalid Roll Number.")]
        public int RollNo { get; set; }

        [Required]
        public string DeptName { get; set; }

        [Required]
        public int DeptId { get; set; }

        [Required]
        public DateTime DOB{ get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNum { get; set; }

        public int Status { get; set; }
    }
}

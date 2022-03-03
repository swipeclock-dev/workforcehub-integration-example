using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// this is not a complete list of all the properties that can be set.
    /// For a complete list refer to the API documentation
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the Employee Code.
        /// </summary>
        [Description("The employee code.")]
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Gets or sets the Employee First Name.
        /// </summary>
        [Description("The employee's first name.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Employee Last Name.
        /// </summary>
        [Description("The employee's last name.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Employee Middle Name.
        /// </summary>
        [Description("The employee's middle name.")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Employee's Designation.
        /// </summary>
        [Description("The employee's designation (e.g. Sr, Jr).")]
        public string Designation { get; set; }

        /// <summary>
        /// Gets or sets the Employee's birth date.
        /// </summary>
        [Description("The employee's birth date. Format is yyyy-MM-dd.")]
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the Employee's Email.
        /// </summary>
        [Description("The employee's email address.")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Employee's Phone Number.
        /// </summary>
        [Description("The employee's phone number. Format is (###) ###-####.")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the Employee's address line 1.
        /// </summary>
        [Description("The employee's address line 1.")]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the Employee's address line 2.
        /// </summary>
        [Description("The employee's address line 2.")]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the Employee's city.
        /// </summary>
        [Description("The employee's city.")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Employee's state.
        /// </summary>
        [Description("The employee's state.")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the Employee's country.
        /// </summary>
        [Description("The employee's country.")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the Employee's zip code.
        /// </summary>
        [Description("The employee's zip code. Format is XXXXX.")]
        public string ZipCode { get; set; }

        public Dictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();
    }
}

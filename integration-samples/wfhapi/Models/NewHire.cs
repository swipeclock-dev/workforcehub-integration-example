using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// WorkforceHub New Hire.
    /// </summary>
    public class NewHire
    {
        public string BirthDate { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string EmploymentStatus { get; set; }

        public string HireDate { get; set; }

        public Address HomeAddress { get; set; }

        public bool IsPartTime { get; set; }

        public string LegalFirstName { get; set; }

        public string LegalLastName { get; set; }

        public string NewHireID { get; set; }

        public string PositionTitle { get; set; }

        public string PrimaryPhoneNumber { get; set; }

        public string PrimaryWorkStateCode { get; set; }

        public string RequestedEmployeeNumber { get; set; }
    }
}

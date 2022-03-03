using System.ComponentModel;

namespace integration_samples.wfhapi.Models
{
    public class CreateCompanyResult
    {
        /// <summary>
        /// Gets or sets the WorkforceHub company id.
        /// </summary>
        public long? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the WorkforceHub company code.
        /// </summary>​
        public string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets the WorkforceHub site id.
        /// </summary>
        public int? SiteId { get; set; }

        [Description("Set to true if WorkforceHub could only partially create a company.  A company may be partially created because another system (e.g., TWP) needed to create it is not available.")]
        public bool PartialCreate { get; set; }

        [Description("If partialCreate is true, the message attribute will include information about why the company could not completely created.  In this case, contact support with the information returned in message to complete the company creation.")]
        public string Message { get; set; }
    }
}

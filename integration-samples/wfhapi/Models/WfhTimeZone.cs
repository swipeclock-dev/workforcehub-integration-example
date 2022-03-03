using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// The WorkforceHub API representation of a timezone (e.g. Eastern Standard Time (EST)).
    /// </summary>
    public class WfhTimeZone
    {
        /// <summary>
        /// The id associated with the timezone(e.g., 1). This id is used when creating or updating a company.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The full spelled-out name of the timezone(e.g., Eastern Standard Time (EST)).
        /// </summary>
        public string Name { get; set; }
    }
}

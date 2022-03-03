using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// The WorkforceHub API representation of a state (e.g. Arkansas).
    /// </summary>
    public class WfhState
    {
        /// <summary>
        /// The id associated with the state(e.g. AR). This id is used when creating or updating a company.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The full spelled-out name of the state(e.g., Arkansas).
        /// </summary>
        public string Name { get; set; }
    }
}

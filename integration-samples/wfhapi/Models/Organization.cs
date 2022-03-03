using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// An organization such as marketing, which could have an organization level of department.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// The WorkforceHub id of the organization record.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The WorkforceHub id of the organization level associated with the organization. 
        /// Use the Get Company Organization Levels route to get more information on the organizational level.
        /// </summary>
        public long OrganizationLevelId { get; set; }

        /// <summary>
        /// Value from the payroll system.  This can be used in the Upsert Company Organization and Upsert Company Organizations routes.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The organization name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A value that indicates whether or not the organization is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Value from the payroll system.  This can be used in the Upsert Company Organization and Upsert Company Organizations routes.
        /// </summary>
        public string ExternalId { get; set; }
    }
}

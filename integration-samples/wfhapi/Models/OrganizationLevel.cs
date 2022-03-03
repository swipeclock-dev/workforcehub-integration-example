using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// An organization level such as department, position, or location.
    /// </summary>
    public class OrganizationLevel
    {
        /// <summary>
        /// A unique id for the organization level/company combination.You will use the id returned when getting or upserting organizations for the organization level.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The organization level name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The organization level description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// If enabled, the organizational level will display in the user interface.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// A third-party for the organization level.
        /// </summary>
        /// <remarks>
        /// This is reserved for future use.
        /// </remarks>
        public string ExternalId { get; set; }

        /// <summary>
        /// The organization type (e.g. department, location, position).
        /// </summary>
        public string Type { get; set; }
    }
}

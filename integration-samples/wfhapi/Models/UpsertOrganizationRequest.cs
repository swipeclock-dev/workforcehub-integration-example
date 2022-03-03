using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// Upsert Organization Request
    /// </summary>
    public class UpsertOrganizationRequest
    {
        /// <summary>
        /// Required for an update or delete. If an id is not provided or does not match an existing record, 
        /// a new company organization will be created.\nIf you want to update the code or eternalId, you 
        /// must use the id as the idType to find the record.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// The WorkforceHub Organization Level id is required to create a new organization.
        /// </summary>
        public long? OrganizationLevelId { get; set; }

        /// <summary>
        /// Code you have assigned to the organization. Must be unique within an organization level.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The NewCode is used when you want to change the code of an existing organization.
        /// </summary>
        public string NewCode { get; set; }

        /// <summary>
        /// The name of the organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A value indicating whether the organization is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Optional id from an external system.
        /// </summary>
        public string ExternalId { get; set; }
    }
}

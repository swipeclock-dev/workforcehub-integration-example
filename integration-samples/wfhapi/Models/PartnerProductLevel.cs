using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    public class PartnerProductLevel
    {
        /// <summary>
        /// Gets or sets the partner product level id.
        /// </summary>
        /// <remarks>
        /// The partner product level id the specific product level for the partner. This is the id
        /// that should be used when creating a company.
        /// </remarks>
        public int PartnerProductLevelId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the partner product level has been deleted.
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// Gets or sets the product level id.
        /// </summary>
        /// <remarks>
        /// The product level id is Swipeclock's global id for the product level.
        /// </remarks>
        public int ProductLevelId { get; set; }

        /// <summary>
        /// Gets or sets the display name for the product level.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation or short name for the product level.
        /// </summary>
        public string DisplayShortName { get; set; }

        /// <summary>
        /// Gets or sets the name of the product level.
        /// </summary>
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    public class EmployeeUpsertResult
    {
        /// <summary>
        /// Gets or sets the Id of the employee that was upserted.
        /// </summary>
        [Description("The id of the employee added or updated.")]
        public long Id { get; set; }
    }
}

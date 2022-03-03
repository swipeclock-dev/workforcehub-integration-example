using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Models
{
    /// <summary>
    /// The Employee Self Service Pending Change.
    /// </summary>
    public class EssPendingChange
    {
        /// <summary>
        /// This value will be passed to the Acknowledge Pending Change route after the respective information has been updated.
        /// </summary>
        public string ChangeID { get; set; }

        /// <summary>
        /// The pending change type.
        /// </summary>
        public string ChangeType { get; set; }

        /// <summary>
        /// The parameters returned differ based on the change type.
        /// </summary>
        public List<KeyValuePair<string, string>> Parameters { get; set; }
    }
}

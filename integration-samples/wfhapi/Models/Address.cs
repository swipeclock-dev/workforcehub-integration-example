using System.ComponentModel;

namespace integration_samples.wfhapi.Models
{
    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        [Description("Use the two-letter standard state codes for US states")]
        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}

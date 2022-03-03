using integration_samples.wfhapi;
using System;

namespace integration_samples
{
    class Program
    {
        static void Main(string[] args)
        {
            object result = null;
            var wfhApi = new WorkforceHubApi();

            // Get WorkforceHub system values.
            result = wfhApi.GetWorkforceHubStates().Result;
            result = wfhApi.GetWorkforceHubTimeZones().Result;

            // Get Partner values.
            result = wfhApi.GetPartnerProductLevels().Result;

            // The WorkforceHub system values and partner product level values are needed to create
            // a WorkforceHub company.
            result = wfhApi.CreateCompany().Result;

            // Get, add and update a company's organizations.
            result = wfhApi.GetOrganizationLevels().Result;
            result = wfhApi.GetAllOrganizations().Result;
            result = wfhApi.GetAllOrganizationsForOrganizationLevel().Result;
            result = wfhApi.AddOrganization().Result;
            result = wfhApi.UpdateOrganization().Result;

            // Create and Modify an employee
            result = wfhApi.CreateEmployee().Result;
            result = wfhApi.UpdateEmployee().Result;

            // Time card information
            result = wfhApi.GetTimeCardInformation().Result;

            // Get a pending new hire and process the new hire
            result = wfhApi.GetPendingNewHires().Result;
            result = wfhApi.GetNewHire().Result;
            result = wfhApi.ProcessNewHire().Result;

            // Get and Acknowledge ESS pending changes
            result = wfhApi.GetEssPendingChanges();
            result = wfhApi.AcknowledgePendingChange();

            // Authentication
            result = wfhApi.GetEmployeeToken().Result;

            System.Console.WriteLine($"Result: {result}");

            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadLine();
        }
    }
}

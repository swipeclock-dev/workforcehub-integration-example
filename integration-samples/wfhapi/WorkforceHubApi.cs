using integration_samples.wfhapi.Models;
using integration_samples.wfhapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace integration_samples.wfhapi
{
    public class WorkforceHubApi
    {
        private const int MY_WFH_SITE_ID = -1; // <MY_WFH_SITE_ID>
        private const string MY_WFH_INTEGRATION_PARTNER_ID = "<MY_SWIPECLOCK_INTEGRATION_PARTNER_ID>";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly AuthenticationService authenticationService = new AuthenticationService();
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public WorkforceHubApi()
        {
            httpClient.BaseAddress = new Uri("https://api.workforcehub.com/api/");
            httpClient.Timeout = new TimeSpan(0, 0, 50);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // required
            httpClient.DefaultRequestHeaders.Add("x-api-version", "1");
            httpClient.DefaultRequestHeaders.Add("x-integration-partner-id", MY_WFH_INTEGRATION_PARTNER_ID);
        }

        #region Authentication

        /// <summary>
        /// Example of creating an employee token that can be used for SSO
        /// </summary>
        /// <remarks>.</remarks>
        /// <returns></returns>
        public async Task<string> GetEmployeeToken()
        {
            var token = await authenticationService.GetEmployeeToken(MY_WFH_SITE_ID, "A0000001");

            return token;
        }

        #endregion

        #region WorkforceHub System

        /// <summary>
        /// This route is an example of how you would get a list of WorkforceHub API states (e.g. Arkansas).
        /// </summary>
        /// <remarks>
        /// The WorkforceHub API state Id is required when creating a new company. This route returns the list of 
        /// valid states in WorkforceHub API.
        /// </remarks>
        /// <returns>A list of WorkforceHub API states.</returns>
        public async Task<List<WfhState>> GetWorkforceHubStates()
        {
            // A Swipeclock authentication token is required to call the route to get the list of WorkforceHub states..
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken();

            var request = new HttpRequestMessage(HttpMethod.Get, "shared/state");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var wfhStates = JsonSerializer.Deserialize<List<WfhState>>(content, serializerOptions);

            return wfhStates;
        }

        /// <summary>
        /// This route is an example of how you would get a list of WorkforceHub API timezones (e.g. Eastern Standard Time).
        /// </summary>
        /// <remarks>
        /// The WorkforceHub API timezone Id is required when creating a new company. This route returns the list of 
        /// valid timezones in WorkforceHub API.
        /// </remarks>
        /// <returns>A list of WorkforceHub API timezones.</returns>
        public async Task<List<WfhTimeZone>> GetWorkforceHubTimeZones()
        {
            // A Swipeclock authentication token is required to call the route to get the list of WorkforceHub timezones.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken();

            var request = new HttpRequestMessage(HttpMethod.Get, "shared/timezone");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var wfhTimeZones = JsonSerializer.Deserialize<List<WfhTimeZone>>(content, serializerOptions);

            return wfhTimeZones;
        }

        #endregion

        #region Partner

        /// <summary>
        /// This method is an example of how you would get the product levels for a partner.
        /// </summary>
        /// <remarks>
        /// The call to get the product levels for a partner requires a Swipeclock authentication 
        /// token for the partner. This token is obtained by calling the Swipeclock authentication service.
        /// </remarks>
        /// <returns>A list of the partner's product levels.</returns>
        public async Task<List<PartnerProductLevel>> GetPartnerProductLevels()
        {
            // A Swipeclock authentication token is required to call the route to get partner product levels.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken();

            var request = new HttpRequestMessage(HttpMethod.Get, "partner/productlevel");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var partnerProductLevels = JsonSerializer.Deserialize<List<PartnerProductLevel>>(content, serializerOptions);

            return partnerProductLevels;
        }

        #endregion

        #region Company

        /// <summary>
        /// This method is an example of creating a WorkforceHub company
        /// </summary>
        /// <remarks>Requires a swipeclock partner level token.</remarks>
        /// <returns></returns>
        public async Task<CreateCompanyResult> CreateCompany()
        {
            // in order to call create company route
            // get a swipeclock partner token by calling the swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken();

            var createCompanyRequest = new
            {
                Address = new
                {
                    Address1 = "10644 S Jordan Gateway",
                    Address2 = "Ste 400",
                    City = "South Jordan",
                    State = "UT",
                    ZipCode = "84095"
                },
                CompanyName = "WFH EXAMPLE",
                PartnerProductLevelId = 3,                  // this can be obtained by calling the route GET /api/partner/productlevel
                Phone = "(888) 223-3450",
                PrimaryContact = "Joe Schmoe",
                TimezoneId = 3,                             // this can be obtained by calling the route GET /api/shared/timezone
            };

            var company = JsonSerializer.Serialize(createCompanyRequest);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "company");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdCompany = JsonSerializer.Deserialize<CreateCompanyResult>(content, serializerOptions);

            return createdCompany;
        }

        #endregion

        #region Employee

        /// <summary>
        /// This method is an example of how you would create an employee using the WorkforceHub API.
        /// </summary>
        /// <remarks>
        /// The route to create an employee requires a Swipeclock authentication token for the WorkforceHub site
        /// that the employee will be added to. 
        /// </remarks>
        /// <returns>
        /// The result, which includes the WorkforceHub id of the employee.
        /// </returns>
        public async Task<EmployeeUpsertResult> CreateEmployee()
        {
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var createNewEmployeeRequest = new Employee()
            {
                FirstName = "Jane",
                LastName = "Swipeclock",
                Address1 = "10644 S Jordan Gateway",
                Address2 = "Ste 400",
                City = "South Jordan",
                State = "UT",
                ZipCode = "84095",
                Email = "support@swipeclock.com",
                Phone = "(888) 223-3450",
                EmployeeCode = "A0000001"
            };

            var company = JsonSerializer.Serialize(createNewEmployeeRequest);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "employee");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EmployeeUpsertResult>(content, serializerOptions);

            return result;
        }

        /// <summary>
        /// This method is an example of updating an employee. In this case, the employee's email address 
        /// is updated.
        /// </summary>
        /// <remarks>
        /// The employee code is required. 
        /// The employee code can be changed if the employee's WorkforceHub id
        /// is specified. The new employee code must be unique.
        /// </remarks>
        /// <returns>
        /// The result includes the WorkforceHub id of the employee that was updated.
        /// </returns>
        public async Task<EmployeeUpsertResult> UpdateEmployee()
        {
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var createNewEmployeeRequest = new Employee()
            {
                Email = "hubexchangeinfo@swipeclock.com",
                EmployeeCode = "A0000001"
            };

            var company = JsonSerializer.Serialize(createNewEmployeeRequest);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "employee");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EmployeeUpsertResult>(content, serializerOptions);

            return result;
        }

        #endregion

        #region Time Card

        /// <summary>
        /// This method is an example to get time card information in the specified format
        /// </summary>
        /// <remarks>
        /// Requires a swipeclock partner level or client level token.  The token has to be site specific.
        /// </remarks>
        /// <returns>
        /// The time card information.
        /// </returns>
        public async Task<object> GetTimeCardInformation()
        {
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            // going return 7 days of data
            var beginDate = DateTime.Now.AddDays(-6).Date.ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var format = "sum3";

            var route = $"timecardexport/?beginDate={beginDate}&endDate={endDate}&format={format}";

            var request = new HttpRequestMessage(HttpMethod.Get, route);
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<object>(content, serializerOptions);

            return result;
        }

        #endregion

        #region Organizations

        /// <summary>
        /// This method is an example of how you would get the organization levels for a company.
        /// </summary>
        /// <remarks>
        /// The call to get the organization levels for a company requires a Swipeclock authentication 
        /// token for the company. This token is obtained by calling the Swipeclock authentication service.
        /// </remarks>
        /// <returns>A list of the partner's product levels.</returns>
        public async Task<List<OrganizationLevel>> GetOrganizationLevels()
        {
            var mySite = MY_WFH_SITE_ID;

            // A Swipeclock authentication token is required to call the route to get partner product levels.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(mySite);

            var request = new HttpRequestMessage(HttpMethod.Get, "company/organizationLevels");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var organizationLevels = JsonSerializer.Deserialize<List<OrganizationLevel>>(content, serializerOptions);

            return organizationLevels;
        }

        /// <summary>
        /// This is an example of getting all organizations for the company.
        /// </summary>
        /// <remarks>
        /// A Swipeclock authentication token for the specific WorkforceHub site is required
        /// to get the organizations for the company.
        /// </remarks>
        /// <returns>A list of organizations for the company.</returns>
        public async Task<List<Organization>> GetAllOrganizations()
        {
            var mySite = MY_WFH_SITE_ID;

            // A Swipeclock authentication token is required to call the route to get all organizations.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(mySite);

            var request = new HttpRequestMessage(HttpMethod.Get, "company/organization");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            // the swipeclock api version is required
            request.Headers.Add("x-api-version", "1");
            request.Headers.Add("x-integration-partner-id", MY_WFH_INTEGRATION_PARTNER_ID);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var organizations = JsonSerializer.Deserialize<List<Organization>>(content, serializerOptions);

            return organizations;
        }

        /// <summary>
        /// This is an example of how you could get all organizations for an organization level. For example,
        /// if you wanted to get all active (enabled) departments for the company, this is how you could do that.
        /// </summary>
        /// <returns>A list of all organizations for an organization level.</returns>
        public async Task<List<Organization>> GetAllOrganizationsForOrganizationLevel()
        {
            var mySite = MY_WFH_SITE_ID;

            // A Swipeclock authentication token is required to call the route to get all organizations.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(mySite);

            // The first step would be to get the WorkforceHub Organization Level Id for the organization level
            // that we are interested in. In this example, the request is to get all departments. So the 
            // first step is to get the Workforce Hub Organization Level Id for a department.
            var organizationLevels = await this.GetOrganizationLevels();

            var departmentId = organizationLevels.First(l => l.Type.Equals("Department"))?.Id;

            var requestUri = $"company/organization?organizationLevelId={departmentId}&activeOnly=true";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var organizations = JsonSerializer.Deserialize<List<Organization>>(content, serializerOptions);

            return organizations;
        }

        /// <summary>
        /// This is an example of how you would add a new organization to a company.
        /// </summary>
        /// <remarks>
        /// The call to add an organization to a company requires a Swipeclock authentication 
        /// token for the company. This token is obtained by calling the Swipeclock authentication service.
        /// </remarks>
        /// <returns>True, if the organization was added.</returns>
        public async Task<bool> AddOrganization()
        {
            var mySite = MY_WFH_SITE_ID;

            // A Swipeclock authentication token is required to call the route to add a new organization.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(mySite);

            // To add a new organization, you need to specify the WorkforceHub organization level id. 
            // This call will get all the organization levels for the company. Then the organization level id 
            // of the Department organization level can be used to add a New Department.
            var organizationLevels = await this.GetOrganizationLevels();

            var departmentId = organizationLevels.First(l => l.Type.Equals("Department"))?.Id;

            var addOrganizationRequest = new UpsertOrganizationRequest()
            {
                Id = null, // This is null because we are adding a new organization not updating an existing organization.
                OrganizationLevelId = departmentId,
                Name = "Quality Assurance",
                Code = "QUALITY",
                NewCode = null, // This is null because we are not changing the code of the organization.
                Enabled = true,
                ExternalId = null, // This could be the id of the organization in an external system.
            };

            var organization = JsonSerializer.Serialize(addOrganizationRequest);
            var requestContent = new StringContent(organization, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "company/organization");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// This is an example of how you would update an organization of a company.
        /// </summary>
        /// <remarks>
        /// The call to update an organization of a company requires a Swipeclock authentication 
        /// token for the company. This token is obtained by calling the Swipeclock authentication service.
        /// </remarks>
        /// <returns>True, if the organization was updated.</returns>
        public async Task<bool> UpdateOrganization()
        {
            var mySite = MY_WFH_SITE_ID;

            // A Swipeclock authentication token is required to call the route to add a new organization.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(mySite);

            // To update the Quality Control department and change the name to Customer Service, the Id of the 
            // Customer Service department is required. This call gets all the organizations for the company
            // then finds the organization with a code of "QUALITY".
            var organizations = await this.GetAllOrganizations();
            var qualityControlOrganization = organizations.First(o => o.Code.Equals("QUALITY"));

            var updateOrganizationRequest = new UpsertOrganizationRequest()
            {
                Id = qualityControlOrganization.Id, // This id is required to update an existing organization.
                OrganizationLevelId = qualityControlOrganization.OrganizationLevelId,
                Name = "Customer Service",
                Code = qualityControlOrganization.Code,
                NewCode = "CS", // This will change the code from QUALITY to CS.
                Enabled = true,
                ExternalId = null, // This could be the id of the organization in an external system.
            };

            var organization = JsonSerializer.Serialize(updateOrganizationRequest);
            var requestContent = new StringContent(organization, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"company/organization");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return true;
        }

        #endregion

        #region Pending Changes

        /// <summary>
        /// This method is an example of how you would get a list of pending changes for a WorkforceHub site.
        /// </summary>
        /// <remarks>
        /// The route to get pending changes requires a token for the WorkforceHub site.
        /// </remarks>
        /// <returns>A list of pending changes.</returns>
        public async Task<List<EssPendingChange>> GetEssPendingChanges()
        {
            // A Swipeclock authentication token is required to call the route to get pending changes for a site.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var request = new HttpRequestMessage(HttpMethod.Get, "ess/pendingChange");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var pendingChanges = JsonSerializer.Deserialize<List<EssPendingChange>>(content, serializerOptions);

            return pendingChanges;
        }

        /// <summary>
        /// This method is an example of how you would acknowledge a pending change for a WorkforceHub site.
        /// </summary>
        /// <remarks>
        /// The route to acknowledge a pending change requires a token for the WorkforceHub site.
        /// </remarks>
        /// <returns>A list of pending changes.</returns>
        public async Task<bool> AcknowledgePendingChange()
        {
            var pendingChanges = await this.GetEssPendingChanges();
            var pendingChangeId = pendingChanges.First()?.ChangeID;

            if (pendingChangeId == null)
            {
                return false;
            }

            // A Swipeclock authentication token is required to call the route to acknowledge a pending change for a site.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var request = new HttpRequestMessage(HttpMethod.Post, $"ess/acknowledgeChange?changeId={pendingChangeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return true;
        }

        #endregion

        #region New Hires

        /// <summary>
        /// This method is an example of how you would get a list of all pending new hires for a WorkforceHub site.
        /// </summary>
        /// <remarks>
        /// The method to get pending new hires requires a token for the WorkforceHub site.
        /// </remarks>
        /// <returns>A list of pending new hires.</returns>
        public async Task<List<EssPendingChange>> GetPendingNewHires()
        {
            // A Swipeclock authentication token is required to call the route to get pending new hirs for a site.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var request = new HttpRequestMessage(HttpMethod.Get, "ess/pendingChange?changeType=AddNewHireToPayroll");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var pendingChanges = JsonSerializer.Deserialize<List<EssPendingChange>>(content, serializerOptions);

            return pendingChanges;
        }

        /// <summary>
        /// The route to get a pending new hire requires the new hire id. The new hire id can be found by 
        /// getting the ESS pending changes and grabbing just the 'AddNewHireToPayroll' changes.
        /// </summary>
        /// <returns></returns>
        public async Task<NewHire> GetNewHire()
        {
            // This call will get all the pending new hires and then select the new hire id for
            // the first pending new hire. That new hire id will be used later to get the 
            // new hire information.
            var pendingNewHires = await this.GetPendingNewHires();
            var newHireId = pendingNewHires.First()?.Parameters.First(p => p.Key == "NewHireID").Value;

            // A Swipeclock authentication token is required to call the route to get a new hire for a site.
            // The Swipeclock authentication token is obtained by calling the Swipeclock authentication service.
            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var request = new HttpRequestMessage(HttpMethod.Get, $"ess/newHire?newHireId={newHireId}");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var newHire = JsonSerializer.Deserialize<NewHire>(content, serializerOptions);

            return newHire;
        }

        /// <summary>
        /// This example shows the process to take a new hire record and add them as a new employee.
        /// </summary>
        /// <remarks>
        /// The example first gets the new hire id, then gets the new hire information and finally
        /// creates a new employee that links to the new hire record and completes the onboarding process.
        /// </remarks>
        /// <returns></returns>
        public async Task<EmployeeUpsertResult> ProcessNewHire()
        {
            // This will get the first new hire from the WorkforceHub site specified.
            var newHire = await this.GetNewHire();

            var token = await authenticationService.GetPartnerToken(MY_WFH_SITE_ID);

            var createNewEmployeeRequest = new Employee()
            {
                FirstName = newHire.LegalFirstName,
                LastName = newHire.LegalLastName,
                Address1 = newHire.HomeAddress?.Address1,
                Address2 = newHire.HomeAddress?.Address2,
                City = newHire.HomeAddress?.City,
                State = newHire.HomeAddress?.State,
                ZipCode = newHire.HomeAddress.ZipCode,
                Email = newHire.EmailAddress,
                Phone = newHire.PrimaryPhoneNumber,
                EmployeeCode = newHire.RequestedEmployeeNumber ?? "btest", 
                Fields = new Dictionary<string, object>
                {
                    // Specifying a new hire id is required to link the new hire in WorkforceHub with the
                    // new employee that will be created from thew new hire.
                    { "NewHireId", newHire.NewHireID },
                },
            };

            var company = JsonSerializer.Serialize(createNewEmployeeRequest);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "employee");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EmployeeUpsertResult>(content, serializerOptions);

            return result;
        }

        #endregion
    }
}

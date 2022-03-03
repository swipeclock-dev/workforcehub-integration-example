using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace integration_samples.wfhapi.Services
{
    public class AuthenticationService
    {
        private static string MY_WFH_API_PARTNER_SECRET => "<MY_WFH_API_SECRET>";
        private static string MY_WFH_API_CLIENT_SECRET => "<MY_WFH_API_SECRET>";
        private static int MY_WFH_PARTNER_ID => -1; // <MY_WFH_PARTNER_ID>;

        private static readonly HttpClient httpClient = new HttpClient();

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public AuthenticationService()
        {
            httpClient.BaseAddress = new Uri("https://clock.payrollservers.us/AuthenticationService/");
            httpClient.Timeout = new TimeSpan(0, 0, 50);
        }

        /// <summary>
        /// Example of calling Swipeclock authentication service to get a partner token
        /// that can be used to call the WorkforceHub API
        /// </summary>
        /// <param name="siteId">Required for site specific routes.</param>
        /// <returns></returns>
        public async Task<string> GetPartnerToken(int? siteId = null)
        {
            var requestToken = CreatePartnerRequestToken(siteId);

            TokenViewModel result = await CallAuthenticationService(requestToken);

            return result.Token;
        }

        /// <summary>
        /// Example of calling Swipeclock authentication service to get an employee token
        /// that can be used to call the WorkforceHub API
        /// </summary>
        /// <param name="siteId">Required for site specific routes.</param>
        /// <returns></returns>
        public async Task<string> GetEmployeeToken(int siteId, string employeeCode)
        {
            var requestToken = CreateEmployeeToken(siteId, employeeCode);

            TokenViewModel result = await CallAuthenticationService(requestToken);

            return result.Token;
        }

        private async Task<TokenViewModel> CallAuthenticationService(string requestToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/userToken");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, requestToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TokenViewModel>(content, serializerOptions);
            return result;
        }

        private string CreateEmployeeToken(int siteId, string employeeCode, bool isPartnerSecret = true)
        {
            var mySecret = isPartnerSecret ? MY_WFH_API_PARTNER_SECRET : MY_WFH_API_CLIENT_SECRET;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myClaims = new Dictionary<string, object>
            {
                { "sub", isPartnerSecret ? "partner" : "client" },
                { "iss", isPartnerSecret ? MY_WFH_PARTNER_ID : siteId },
                { "product", "twpemp" },
            };

            myClaims.Add("siteInfo", new { type = "id", id = siteId.ToString() });
            myClaims.Add("user", new { type = "empcode", id = employeeCode });

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = myClaims,
                Expires = DateTime.UtcNow.AddMinutes(4), // cannot exceed 5 minutes.
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates a partner request token.
        /// </summary>
        /// <remarks>the token that is created is used to call the swipeclock authentication service.
        /// If successful the authentication service will return a token that can be used
        /// when calling WorkforceHub API.</remarks>
        /// <param name="siteId">Required for site specific routes.</param>
        /// <returns></returns>
        private string CreatePartnerRequestToken(int? siteId = null)
        {
            var mySecret = MY_WFH_API_PARTNER_SECRET;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = MY_WFH_PARTNER_ID;

            var myClaims = new Dictionary<string, object>
            {
                { "sub", "partner" },
                { "iss", MY_WFH_PARTNER_ID },
                { "product", "twppartner" },
            };

            if (siteId.HasValue)
            {
                myClaims.Add("siteInfo", new { type = "id", id = siteId.Value });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = myClaims,
                Expires = DateTime.UtcNow.AddMinutes(4), // cannot exceed 5 minutes.
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class TokenViewModel
    {
        /// <summary>
        /// Token used for WorkforceHub API
        /// </summary>
        public string Token { get; set; }
    }
}
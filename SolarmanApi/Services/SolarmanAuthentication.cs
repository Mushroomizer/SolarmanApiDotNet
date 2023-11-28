using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rest.Net;
using Rest.Net.Authenticators;
using Rest.Net.Interfaces;
using SolarmanApi.Options;
using System.Security.Cryptography;
using System.Text;

namespace SolarmanApi.Services
{
    public class SolarmanAuthentication : IAuthentication
    {
        private readonly SolarmanAuthenticationOptions _authenticationOptions;
        private readonly ILogger<SolarmanAuthentication> _logger;

        private readonly IRestClient _restClient;

        private string
            _issuer,
            _grantType,
            _clientId,
            _clientSecret,
            _username,
            _password,
            _token,
            _refreshToken;

        private DateTime _tokenExpiration = DateTime.MinValue;

        public SolarmanAuthentication(IOptions<SolarmanAuthenticationOptions> authenticationOptions, ILogger<SolarmanAuthentication> logger)
        {
            _authenticationOptions = authenticationOptions.Value;
            _logger = logger;
            _restClient = new RestClient(_authenticationOptions.issuer);
            _restClient.Authentication = this;
        }


        public async Task SetRequestAuthentication(IRestRequest request)
        {
            if (_token == null || DateTime.Now > _tokenExpiration)
            {
                await Login();
            }

            request.SetAuthentication(Http.AuthenticationMethod.Bearer, _token);
        }

        public async Task<AuthResponse> Login()
        {
            String password = getHashSha256(_authenticationOptions.password);
            //26afa885a151261bfd49bcbbb6b5e3d2ec6f7a72598940232ad5439685f5aa17
            _logger.LogInformation("Getting bearer token...");
            var client = new RestClient(_authenticationOptions.issuer);
            client.Headers.Add("Content-Type", "application/json");
            client.AddParameter("appId", _authenticationOptions.appId);
            client.AddParameter("language", "en");

            var response = await client.PostAsync<AuthResponse>("/account/v1.0/token", new
            {
                _authenticationOptions.appSecret,
                _authenticationOptions.email,
                password
            }, false);

            if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Data.AccessToken))
            {
                _token = response.Data.AccessToken;
                _refreshToken = response.Data.RefreshToken;
                _tokenExpiration = DateTime.Now.AddSeconds(response.Data.ExpiresIn - 60);

                _logger.LogInformation("Token retrieved");
            }
            else
            {
                _token = null;
                _refreshToken = null;
                _tokenExpiration = DateTime.MinValue;
                _logger.LogError($"Could not set token: {JsonConvert.SerializeObject(response)}");
                throw new AuthenticationException($"Could not set token: {JsonConvert.SerializeObject(response)}");
            }

            return response.Data;
        }

        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
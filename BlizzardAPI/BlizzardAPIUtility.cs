/*
 * Some utility methods specific to the blizzard api.
 */

using Microsoft.VisualStudio.Services.OAuth;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace GarrettBot.BlizzardAPI
{
    public class BlizzardAPIUtility
    {
        // This is the Blizzard token needed to make api calls.
        public static string BlizzAccessToken { get; private set; }

        /// <summary>
        /// This method will get the BlizzardAccess token using my credentials.
        /// </summary>
        /// <returns></returns>
        public static void GetBlizzardAccessToken()
        {
            // Get my blizzard token from environmentvariable
            string clientSecret = Environment.GetEnvironmentVariable("BLIZZARDTOKEN");

            // Get the Token
            var client = new RestClient("https://eu.battle.net/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded",
                                $"grant_type=client_credentials&client_id=4329a43854634303a426830ceabb767d&client_secret={clientSecret}",
                                ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            // Deserialize the token
            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            // Store Token
            BlizzAccessToken = tokenResponse.AccessToken;
        }
    }
}
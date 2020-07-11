/*
 * Some utility methods specific to the blizzard api.
 */

using Microsoft.VisualStudio.Services.OAuth;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public static async Task GetBlizzardAccessToken()
        {
            // Get my blizzard token from environmentvariable
            string clientSecret = Environment.GetEnvironmentVariable("BLIZZARDTOKEN");

            // Create our client
            using (HttpClient client = new HttpClient())
            {
                // Authorization
                string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"4329a43854634303a426830ceabb767d:{clientSecret}"));

                // Headers
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");

                // Content
                HttpContent hc = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                // Request our data and record response
                HttpResponseMessage response = await client.PostAsync("https://eu.battle.net/oauth/token", hc);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the token
                    AccessTokenResponse tokenResponse = await response.Content.ReadAsAsync<AccessTokenResponse>();

                    // Store Token
                    BlizzAccessToken = tokenResponse.AccessToken;
                }
            }
        }
    }
}
/*
 * This class will store our list of available commands.
 */

using Discord.Commands;
using System.Threading.Tasks;
using GarrettBot.BlizzardAPI;
using System.Net.Http;

namespace GarrettBot.DiscordAPI
{
    public class Commands : ModuleBase
    {
        /// <summary>
        /// This will request the MountCollectionSummary from Blizzard.
        /// It has a full Deserializtion, but currently only outputs the count of mounts.
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        [Command("Mounts")]
        public async Task SearchCommand(string characterName)
        {
            // Create our client
            using (HttpClient client = new HttpClient())
            {
                // Request our data and record response
                HttpResponseMessage response = await client.GetAsync("https://us.api.blizzard.com/profile/wow/character/magtheridon/" + characterName + "/collections/mounts?namespace=profile-us&locale=en_US&access_token=" + BlizzardAPIUtility.BlizzAccessToken);

                // If we have a good response, report the output
                if (response.IsSuccessStatusCode)
                {
                    MountCollectionSummary mcs = await response.Content.ReadAsAsync<MountCollectionSummary>();
                    await ReplyAsync(mcs.Mounts.Count.ToString());
                }
                else
                {
                    await ReplyAsync(response.ReasonPhrase);
                }
            }
        }
    }
}
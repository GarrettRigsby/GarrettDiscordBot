/*
 * This class will store our list of available commands.
 */

using Discord.Commands;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using GarrettBot.BlizzardAPI;

namespace GarrettBot.DiscordAPI
{
    public class Commands : ModuleBase
    {
        /// <summary>
        /// HELLO WORLD
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Command("Hello")]
        public async Task HelloCommand(string args)
        {
            await ReplyAsync("Hello, World! " + args.Length);
        }

        /// <summary>
        /// This will request the MountCollectionSummary from Blizzard.
        /// It has a full Deserializtion, but currently only outputs the count of mounts.
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        [Command("Mounts")]
        public async Task SearchCommand(string characterName)
        {
            var client = new RestClient("https://us.api.blizzard.com/profile/wow/character/magtheridon/" + characterName + "/collections/mounts?namespace=profile-us&locale=en_US&access_token=" + BlizzardAPIUtility.BlizzAccessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResponse = JsonConvert.DeserializeObject<MountCollectionSummary>(response.Content);
            await ReplyAsync(apiResponse.Mounts.Count.ToString());
        }
    }
}

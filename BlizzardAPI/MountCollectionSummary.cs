/*
 * This class handles the deserialization of the MountCollectionSummary call.
 * 
 * I decided to use nested classes, because the classes were only needed by their parent class.
 * There might be a better way, but this is something I'm still learning and plan to do some more research on.
 */

using Newtonsoft.Json;
using System.Collections.Generic;

namespace GarrettBot.BlizzardAPI
{
    [JsonObject]
    public class MountCollectionSummary
    {
        [JsonProperty("_links")]
        public Links LinksObject { get; set; }

        [JsonProperty("mounts")]
        public List<MountWrapper> Mounts { get; set; }

        [JsonObject]
        public class Links
        {
            [JsonProperty("self")]
            public Self SelfObject { get; set; }

            [JsonObject]
            public class Self
            {
                [JsonProperty("href")]
                public string href { get; set; }
            }
        }

        [JsonObject]
        public class MountWrapper
        {
            [JsonProperty("mount")]
            public Mount ThisMount { get; set; }

            [JsonObject]
            public class Mount
            {
                [JsonProperty("key")]
                public Key key { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("id")]
                public string ID { get; set; }

                [JsonObject]
                public class Key
                {
                    [JsonProperty("href")]
                    public string href { get; set; }
                }
            }
        }
    }
}
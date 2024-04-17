using Newtonsoft.Json;
using System.Collections.Generic;

namespace z5.ms.domain.user.viewmodels
{
    public class FacebookAppIds
    {
        [JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Datum> Data { get; set; }
        [JsonProperty("paging", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Paging PagingDetails { get; set; }

        public class App
        {
            [JsonProperty("category", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Category { get; set; }
            [JsonProperty("link", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Link { get; set; }
            [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }
            [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }
        }

        public class Datum
        {
            [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }
            [JsonProperty("app", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public App App { get; set; }
        }

        public class Cursors
        {
            [JsonProperty("before", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string Before { get; set; }
            [JsonProperty("after", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public string After { get; set; }
        }

        public class Paging
        {
            [JsonProperty("cursors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public Cursors Cursors { get; set; }
        }
    }
}
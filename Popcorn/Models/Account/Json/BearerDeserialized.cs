using RestSharp.Deserializers;

namespace Popcorn.Models.Account.Json
{
    public class BearerDeserialized
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}

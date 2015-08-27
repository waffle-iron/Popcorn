using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Popcorn.Models.Account.Json
{
    public class UserDeserialized
    {
        [DeserializeAs(Name = "url")]
        public string Url { get; set; }

        [DeserializeAs(Name = "id")]
        public string Id { get; set; }

        [DeserializeAs(Name = "fullName")]
        public string Fullname { get; set; }

        [DeserializeAs(Name = "userName")]
        public string Username { get; set; }

        [DeserializeAs(Name = "email")]
        public string Email { get; set; }

        [DeserializeAs(Name = "emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [DeserializeAs(Name = "level")]
        public int Level { get; set; }

        [DeserializeAs(Name = "joinDate")]
        public DateTime Joindate { get; set; }

        [DeserializeAs(Name = "roles")]
        public List<string> Roles { get; set; }

        [DeserializeAs(Name = "claims")]
        public List<string> Claims { get; set; }
    }
}

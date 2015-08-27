using RestSharp.Deserializers;
using GalaSoft.MvvmLight;

namespace Popcorn.Models.Account.Json
{
    public class BearerDeserialized : ObservableObject
    {
    	private string _accessToken;
        [DeserializeAs(Name = "access_token")]
        public string AccessToken
        { 
            get { return _accessToken; }
            set { Set(() => AccessToken, ref _accessToken, value); }
        }

        private string _tokenType;
        [DeserializeAs(Name = "token_type")]
        public string TokenType
        { 
            get { return _tokenType; }
            set { Set(() => TokenType, ref _tokenType, value); }
        }

        private int _expiresIn;
        [DeserializeAs(Name = "expires_in")]
        public int ExpiresIn
        { 
            get { return _expiresIn; }
            set { Set(() => ExpiresIn, ref _expiresIn, value); }
        }
    }
}

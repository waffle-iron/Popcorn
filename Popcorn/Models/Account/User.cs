using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Account
{
    public class User : ObservableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Bearer { get; set; }

        private string _url;
        [DeserializeAs(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        private string _id;
        [DeserializeAs(Name = "id")]
        public string Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        private string _fullName;
        [DeserializeAs(Name = "fullName")]
        public string Fullname
        {
            get { return _fullName; }
            set { Set(() => Fullname, ref _fullName, value); }
        }

        private string _userName;
        [DeserializeAs(Name = "userName")]
        public string Username
        {
            get { return _userName; }
            set { Set(() => Username, ref _userName, value); }
        }

        private string _email;
        [DeserializeAs(Name = "email")]
        public string Email
        {
            get { return _email; }
            set { Set(() => Email, ref _email, value); }
        }

        private bool _emailConfirmed;
        [DeserializeAs(Name = "emailConfirmed")]
        public bool EmailConfirmed
        {
            get { return _emailConfirmed; }
            set { Set(() => EmailConfirmed, ref _emailConfirmed, value); }
        }

        private int _level;
        [DeserializeAs(Name = "level")]
        public int Level
        {
            get { return _level; }
            set { Set(() => Level, ref _level, value); }
        }

        private DateTime _joinDate;
        [DeserializeAs(Name = "joinDate")]
        public DateTime Joindate
        {
            get { return _joinDate; }
            set { Set(() => Joindate, ref _joinDate, value); }
        }

        private List<string> _roles;
        [DeserializeAs(Name = "roles")]
        public List<string> Roles
        {
            get { return _roles; }
            set { Set(() => Roles, ref _roles, value); }
        }

        private List<string> _claims;
        [DeserializeAs(Name = "claims")]
        public List<string> Claims
        {
            get { return _claims; }
            set { Set(() => Claims, ref _claims, value); }
        }
    }
}

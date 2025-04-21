using Framework.Business.Api;
using RestSharp;

namespace Framework.Business.Api
{
    public class UserBuilder
    {
        private string _name;
        private string _username;

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Name = _name,
                Username = _username
            };
        }
    }
}

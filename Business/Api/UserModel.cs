using System.Text.Json.Serialization;

namespace Framework.Business.Api
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("address")]
        public AddressDetails Address { get; set; }
        [JsonPropertyName("phone")]

        public string Phone { get; set; }
        [JsonPropertyName("website")]
        public string Website { get; set; }
        [JsonPropertyName("company")]
        public CompanyDetails Company { get; set; } 

        public class AddressDetails
        {
            [JsonPropertyName("street")]
            public string Street { get; set; }
            [JsonPropertyName("suite")]
            public string Suite { get; set; }
            [JsonPropertyName("city")]
            public string City { get; set; }
            [JsonPropertyName("zipcode")]
            public string Zipcode { get; set; }
        }

        public class CompanyDetails
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }
}

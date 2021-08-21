using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Facebook
{
    public class FacebookProfile
    {
        public string Name { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        [JsonProperty("mobile_phone")]
        public string MobilePhone { get; set; }
        public string Location { get; set; }
        public string Hometown { get; set; }
        public string Address { get; set; }
        public Picture Picture { get; set; }
        
        public Cover Cover { get; set; }
        [JsonProperty("age_range")]
        public AgeRange AgeRange { get; set; }
        public Device[] Devices { get; set; }
        public bool IsVerified { get; set; }
        public string Id { get; set; }
    }

    public class Picture
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }

    public class Cover
    {
        public string Id { get; set; }
        public int OffsetY { get; set; }
        public string Source { get; set; }
    }

    public class AgeRange
    {
        public int Min { get; set; }
    }

    public class Device
    {
        public string Os { get; set; }
    }

}

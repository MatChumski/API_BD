﻿namespace API_BD.Models
{
    public class Jwt
    {
        public string ?Subject { get; set; }
        public string ?Key { get; set; }

        public string ?Issuer { get; set; }

        public string ?Audience { get; set; }
    }
}
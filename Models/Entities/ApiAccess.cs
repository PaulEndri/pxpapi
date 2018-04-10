using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PixelPubApi.MySQL;

namespace PixelPubApi.Models.Entities
{
    [Table("api_key")]
    public class ApiAccess : Model
    {
        [Key]
        public long id { get; set; }
        public string key { get; set; }
        public string username { get; set; }

        public ApiAccess(WrathIncarnateContext context) : base(context)
        {}

        public ApiAccess() {}

        override public string getTableName()
        {
            return "api_key";
        }

        override public string getInsertQuery() {
            var key = System.Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            return $"INSERT INTO api_key (`key`, `username`, created_at, updated_at) VALUES ('{key}', @username, NOW(), NOW())";
        }
    }

}
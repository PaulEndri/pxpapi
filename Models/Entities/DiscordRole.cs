#region

using System;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PixelPubApi.MySQL;

#endregion

namespace PixelPubApi.Models.Entities
{
    public class DiscordRole : Model
    {
        [Key]
        public long id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string reaction { get; set; }
        public string identifier { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }

        override public string getTableName() {
            return "discord_roles";
        }
    }
}
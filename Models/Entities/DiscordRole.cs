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
    [Table("discord_roles")]
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

        public DiscordRole(WrathIncarnateContext context) : base(context) { }
        public DiscordRole() { }

        override public string getTableName() {
            return "discord_roles";
        }

        override public string getInsertQuery()
        {
            return "INSERT INTO discord_roles (name, value, reaction, identifier, updated_at, created_at) values (@name, @value, @reaction, @identifier, NOW(), NOW())"; 
        }

        override public string getPutQuery(long id)
        {
            return $"UPDATE discord_roles SET name=@name, value=@value, reaction=@reaction, identifier=@identifier, updated_at=NOW() where id = {id}";
        }
    }
}
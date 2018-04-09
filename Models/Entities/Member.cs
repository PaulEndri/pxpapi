#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace PixelPubApi.Models.Entities
{
    [Table("bungie_member")]
    public class Member
    {
        [Key]
        public long id { get; set; }
        public long? bungie_id { get; set; }
        public long? destiny_id { get; set; }
        public long? active_clan_id {get; set;}
        public byte deleted { get; set; }
        public string data { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime last_seen    { get; set; }
        public DateTime created_at { get; set; }
    }
}
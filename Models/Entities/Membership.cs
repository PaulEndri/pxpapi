#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

#endregion

namespace PixelPubApi.Models.Entities {
    [Table("bungie_membership")]
    public class Membership {
        [Key]
        public long id { get; set; }
        public long member_id { get; set; }
        public long clan_id { get; set; }
        public long bungie_clan_id { get; set; }
        public long bungie_member_id { get; set; }
        public long destiny_member_id { get; set; }
        public byte deleted { get; set; }
        public string membership_type { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }

        [ForeignKey("clan_id")]
        public Clan clan {get; set;}
        
        [ForeignKey("member_id")]
        [JsonIgnore]
        public Member member { get; set; }
    }
}
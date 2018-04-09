#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

#endregion

namespace PixelPubApi.Models.Entities
{
    [Table("bungie_clan")]
    public class Clan : Model
    {
        [Key]
        public long id { get; set; }
        public byte deleted;
        public byte latest;
        public string name { get; set; }
        public int group_id { get; set; }
        public string data { get; set; }
        public int member_count { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime synced_at { get; set; }
        public DateTime created_at { get; set; }
        public override long getPrimaryKey() {
            return (long)id;
        }

        [ForeignKey("active_clan_id")]
        public List<Member> Members {get; set;}
    }
}
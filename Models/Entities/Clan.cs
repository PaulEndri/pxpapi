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

        [ForeignKey("active_clan_id")]
        public List<Member> Members {get; set;}

        public Clan(WrathIncarnateContext context) : base(context) { }
        public Clan() { }

        override public string getTableName() {
            return "bungie_clan";
        }

        override public async Task<List<T>> getAllLoaded<T>(int pageNumber, int pageSize) {
            return await getRecord<T>();
        }
        override public async Task<T> getById<T>(long id) {
            var where   = $"WHERE c.id = {id}";
            var results = await getRecord<T>(where) as List<T>;

            return results.FirstOrDefault();
        }

        private async Task<List<T>> getRecord<T>(string where = "") {
            var tableName  = getTableName();
            var primaryKey = getPrimaryKey();
            var clanDict   = new Dictionary<long, Clan>();

            var query = $"{SEARCH} {tableName} c LEFT JOIN bungie_member m ON m.active_clan_id = c.id"
                + $" AND m.deleted = 0 and c.deleted = 0 {where} ORDER BY c.{primaryKey} DESC";

            var results = await _context.connection.QueryAsync<Clan, Member, Clan>(query, (clan, member) => {
                Clan entry;

                if(!clanDict.TryGetValue(clan.id, out entry)) {
                    entry = clan;
                    entry.Members = new List<Member>();
                    clanDict.Add(entry.id, entry);    
                }

                entry.Members.Add(member);

                return entry;
            }, splitOn: "id");

            return results.Distinct().ToList() as List<T>;
        }
    }
}
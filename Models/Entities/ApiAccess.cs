using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelPubApi.Models.Entities
{
    [Table("api_key")]
    public class ApiAccess
    {
        [Key]
        public long id { get; set; }
        public string key { get; set; }
        public string username { get; set; }
        
    }

}
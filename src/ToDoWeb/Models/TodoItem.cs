using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Models
{
    public class TodoItem
    {
        //[JsonProperty("key")]
        [Display(Name = "Unique Key")]
        public string key { get; set; }
        //[JsonProperty("name")]
        [Display(Name = "To Do Item")]
        public string name { get; set; }
        //[JsonProperty("IsComplete")]
        [Display(Name = "Complete")]
        public bool IsComplete { get; set; }
    }
}

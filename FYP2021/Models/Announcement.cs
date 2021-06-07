using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FYP2021.Models
{
    public class Announcement
    {

        [Key]
        public int Id
        {
            get; set;
        }

        public string ViewAnnouncement
        {
            get; set;
        }

        public string Announcement_Date
        {
            get; set;
        }
    }
}

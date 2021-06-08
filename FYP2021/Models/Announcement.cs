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

        [Required(ErrorMessage = "Please enter an announcement")]
        public string ViewAnnouncement
        {
            get; set;
        }

        [Required]
        [DataType(DataType.Date)]
        public string Announcement_Date
        {
            get; set;
        }
    }
}

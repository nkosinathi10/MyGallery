using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace My_Gallery.Models
{
    public partial class Photos
    {
        public string Email { get; set; }
        [DisplayName("Image Name")]
        public string ImageName { get; set; }
        
        public string Location { get; set; }
        [NotMapped]
        [DisplayName("Upload")]
        public IFormFile ImageFile { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectHQTCSDL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profile
    {
        public int idProfile { get; set; }
        public int idUser { get; set; }
        public string tenNguoiDung { get; set; }
        public string diaChi { get; set; }
        public string sdt { get; set; }
        public string email { get; set; }
    
        public virtual UserMain UserMain { get; set; }
    }
}

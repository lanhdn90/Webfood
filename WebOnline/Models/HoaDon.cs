//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            this.ChiTietHoaDon = new HashSet<ChiTietHoaDon>();
        }
    
        public int MaHoaDon { get; set; }
        public Nullable<int> MaTaiKhoan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public Nullable<System.DateTime> ThoiGianGiaoHang { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<bool> TrangThai { get; set; }
        public Nullable<decimal> TongTien { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}

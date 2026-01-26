using System;

namespace Manny_Tools_Claude.DTO
{
    public class SizeLinkItem
    {
        public int OID { get; set; }
        public string Description { get; set; }
        public int Holder { get; set; }

        public override string ToString()
        {
            return $"{OID} - {Description}";
        }
    }
}
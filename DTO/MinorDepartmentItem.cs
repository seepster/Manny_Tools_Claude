using System;

namespace Manny_Tools_Claude.DTO
{
    public class MinorDepartmentItem
    {
        public int OID { get; set; }
        public string ParentDepartment { get; set; }
        public string MinorDepartmentName { get; set; }
        public string MinorDepartmentNumber { get; set; }

        public override string ToString()
        {
            return $"{MinorDepartmentNumber} - {MinorDepartmentName}";
        }
    }
}
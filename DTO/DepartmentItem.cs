using System;

namespace Manny_Tools_Claude.DTO
{
    public class DepartmentItem
    {
        public string DepartmentNumber { get; set; }
        public string DepartmentName { get; set; }

        public override string ToString()
        {
            return $"{DepartmentNumber} - {DepartmentName}";
        }
    }
}
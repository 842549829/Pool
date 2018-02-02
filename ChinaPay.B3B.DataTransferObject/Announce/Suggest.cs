using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Announce
{
    /// <summary>
    /// B3B�û�����
    /// </summary>
    public class Suggest
    {
        public Guid Id { get; set; }
        public SuggestCategory SuggestCategory { get; set; }
        public string ContractInformation { get; set; }
        public string SuggestContent { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Readed { get; set; }
        public bool Handled { get; set; }
        public string Creator { get; set; }
        public string CreatorName { get; set; }
        /// <summary>
        /// Ա�����
        /// </summary>
        public Guid? EmployeeId { get; set; }
    }
}
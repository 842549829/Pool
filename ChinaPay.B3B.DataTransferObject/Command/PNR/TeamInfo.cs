namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    class TeamInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 实际人数
        /// </summary>
        public int ActualNumber { get; set; }
    }
}

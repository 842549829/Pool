using System;

namespace ChinaPay.B3B.DataTransferObject.FlightQuery {
    public class FlightView {
        public bool IsShare { get; set; }
        public int Serial { get; set; }
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public AirportView Departure { get; set; }
        public AirportView Arrival { get; set; }
        public string FlightNo { get; set; }
        public string Aircraft { get; set; }
        public decimal YBPrice { get; set; }
        public decimal AirportFee { get; set; }
        public decimal BAF { get; set; }
        public decimal AdultBAF { get; set; }
        public decimal ChildBAF { get; set; }
        public string BunkCode { get; set; }
        public bool SuportChild { get; set; }
        public B3B.Common.Enums.BunkType? BunkType { get; set; }
        public int SeatCount { get; set; }
        public string BunkDescription { get; set; }
        public decimal? Discount { get; set; }
        public string EI { get; set; }
        public decimal Fare { get; set; }
        public decimal? Rebate { get; set; }
        public decimal SettleAmount { get; set; }

        public decimal? RenderDiscount { get; set; }

        public static FlightView Parse(string args) {
            FlightView result = null;
            if(args != null) {
                var flightArray = args.Split('|');
                if(flightArray.Length == 32) {
                    var flightDaysInterval = 0;
                    int.TryParse(flightArray[13], out flightDaysInterval);
                    result = new FlightView() {
                        Serial = int.Parse(flightArray[0]),
                        AirlineCode = flightArray[1],
                        AirlineName = flightArray[2],
                        Departure = new AirportView() {
                            Code = flightArray[3],
                            Name = flightArray[4],
                            City = flightArray[5],
                            Terminal = flightArray[6],
                            Time = DateTime.Parse(flightArray[7])
                        },
                        Arrival = new AirportView() {
                            Code = flightArray[8],
                            Name = flightArray[9],
                            City = flightArray[10],
                            Terminal = flightArray[11],
                            Time = DateTime.Parse(flightArray[12]).AddDays(flightDaysInterval)
                        },
                        FlightNo = flightArray[14],
                        Aircraft = flightArray[15],
                        YBPrice = decimal.Parse(flightArray[16]),
                        AirportFee = decimal.Parse(flightArray[17]),
                        BAF = decimal.Parse(flightArray[18]),
                        AdultBAF = decimal.Parse(flightArray[19]),
                        ChildBAF = decimal.Parse(flightArray[20]),
                        BunkCode = flightArray[21],
                        SeatCount = int.Parse(flightArray[23]),
                        BunkDescription = flightArray[24],
                        EI = flightArray[26],
                        Fare = decimal.Parse(flightArray[27]),
                        SettleAmount = decimal.Parse(flightArray[29]),
                        SuportChild = bool.Parse(flightArray[30])
                    };
                    if(!string.IsNullOrWhiteSpace(flightArray[22]) && flightArray[22] != "null") {
                        result.BunkType = (B3B.Common.Enums.BunkType)int.Parse(flightArray[22]);
                    }
                    if(!string.IsNullOrWhiteSpace(flightArray[25])) {
                        result.Discount = decimal.Parse(flightArray[25]);
                    }
                    if(!string.IsNullOrWhiteSpace(flightArray[28])) {
                        result.Rebate = decimal.Parse(flightArray[28]);
                    }
                }
            }
            return result;
        }
    }
}

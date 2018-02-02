using System.Collections.Generic;

namespace ChinaPay.B3B.Service.RegularExpression.Repository.Memory
{
    using RegularExpression = Domain.RegularExpression;

    class RegExRepository: IRegExRepository
    {
        IEnumerable<RegularExpression> IRegExRepository.Query()
        {
            var result = new List<RegularExpression>();

            // AvhResult
            var id = "AvhResult";
            var value =
                @"^(?<LineNumber>\d)[-+\s]\s(?:\s|(?<IsCodeShareFlight>[*]))(?<Carrier>[A-Z0-9]{2})(?:(?<InternalNumber>[0-9]{4}|[0-9]{3}[A-Z])|(?<InternalNumber>[0-9]{3})\s)\s{2}(?<CRS>[AD]S[#!])\s(?:(?:(?<OfferedService>[A-Z][A-Z1-9])|\s{2})\s){10}\s(?<Departure>[A-Z]{3})(?<Arrival>[A-Z]{3})\s(?<DepartureHour>\d{2})(?<DepartureMinute>\d{2})\s{3}(?<ArrivalHour>\d{2})(?<ArrivalMinute>\d{2})(?:\s{2}|\+(?<AddDays>\d))\s(?<AircraftType>[0-9A-Z]{3})\s(?<TransitPoint>\d)(?<ASR>[\^\s])(?:(?<Meal>[A-Z])|\s)\s{2}(?<ETicketFlag>E)\s{2}>\s{3}(?:(?<CodeShareFlightCarrier>[A-Z0-9]{2})(?:(?<CodeShareFlightInternalNumber>[0-9]{4}|[0-9]{3}[A-Z])|(?<InternalNumber>[0-9]{3})\s)|\s{6})\s{6}(?:(?:(?<OfferedService>[A-Z][A-Z1-9])|\s{2})\s){16}\s{4}(?:(?<TerminalOfDeparture>[A-Z0-9]{2})|(?<TerminalOfDeparture>[A-Z])\s|[-\s]{2})\s(?:(?<TerminalOfArrival>[A-Z0-9]{2})|(?<TerminalOfArrival>[A-Z])\s|[-\s]{2})\s(?<FlightTime>[\s\d]\d:\d{2})$";
            var exapmle = "";
            var descriotption  = "AVH";
            var regex = new RegularExpression(id, value, exapmle, descriotption);
            result.Add(regex);

            // FfResult
            id = "FfResult";
            value =
                @"(?<AirportCode>[A-Z]{3})\s{3}(?<ArrivalHour>\d{2})(?<ArrivalMinute>\d{2})(?:\s{2}|\+(?<ArrivalAddDays>\d))\s{2}(?<DepartureHour>\d{2})(?<DepartureMinute>\d{2})(?:\s{2}|\+(?<DepartureAddDays>\d))";
            exapmle = "";
            descriotption = "FF";
            regex = new RegularExpression(id, value, exapmle, descriotption);
            result.Add(regex);

            // SsResult
            id = "SsResult";
            value = @"^(?:\s(?<LineNumber>\d)|(?<LineNumber>\d{2})).\s(?:\s|(?<IsCodeShareFlight>[*]))(?<Carrier>[A-Z0-9]{2})(?:(?<InternalNumber>[0-9]{4}|[0-9]{3}[A-Z])|(?<InternalNumber>[0-9]{3})\s)\s(?<Class>[A-Z])(?:\s|(?<SubClass>\d))\s{2}(?<Weekday>MO|TU|WE|TH|FR|SA|SU)(?<Day>\d{2})(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\s{2}|(?<Year>\d{2}))(?<Departure>[a-zA-Z]{3})(?<Arrival>[a-zA-Z]{3})\s(?<SeatStatus>[A-Z]{2})(?:(?<Seatings>\d{2})|(?<Seatings>\d)\s)\s{2}(?<DepartureHour>\d{2})(?<DepartureMinute>\d{2})\s(?<ArrivalHour>\d{2})(?<ArrivalMinute>\d{2})(?:\s{2}|\+(?<AddDays>\d))\s{8}(?<IsETicket>E)(?:\s(?:(?<TerminalOfDeparture>[A-Z0-9]{2})|(?<TerminalOfDeparture>[A-Z])\s|[-\s]{2})(?:(?<TerminalOfArrival>[A-Z0-9]{2})|(?<TerminalOfArrival>[A-Z])\s|[-\s]{2}))?(?:\s{1,2}(?<ExtendedInformation>[\w-]*))?\s$";
            exapmle = "";
            descriotption = "SS";
            regex = new RegularExpression(id, value, exapmle, descriotption);
            result.Add(regex);


            return result;
        }
    }
}

using System;
using ChinaPay.B3B.Service.FlightSchedual.Domain;
using ChinaPay.B3B.Service.FlightSchedual.Repository;

namespace ChinaPay.B3B.Service.FlightSchedual
{
    public class FlightSchedualService
    {
        public static Schedual GetFlightScheduals(FlightNumber flightNumber, DateTime flightDate, string departureAirport, string arrivalAirport)
        {
            var repository = Factory.CreateFlightSchedualRepository();
            return repository.Query(flightNumber, departureAirport, arrivalAirport, flightDate);
        }

        public static bool AddCanceledFlightSchedual(CanceledSchedual flightSchedual)
        {
            var schedualRepository = Factory.CreateFlightSchedualRepository();
            return schedualRepository.Save(flightSchedual) == 0;
        }

        public static bool AddDelayedFlightSchedual(DelayedSchedual flightSchedual)
        {
            var schedualRepository = Factory.CreateFlightSchedualRepository();
            return schedualRepository.Save(flightSchedual) == 0;
        }

        public static bool AddTransferedFlightSchedual(TransferedSchedual flightSchedual)
        {
            var schedualRepository = Factory.CreateFlightSchedualRepository();
            return schedualRepository.Save(flightSchedual) == 0;
        }
        
        public static bool AddSchedual(Schedual schedual)
        {
            var flightRepository = Factory.CreateFlightSchedualRepository();
            return flightRepository.Save(schedual) > 0;
        }

        public static bool Synchronous()
        {
            var flightRepository = Factory.CreateFlightTransferRepository();
            return flightRepository.Synchronous() == 0;
        }
    }
}

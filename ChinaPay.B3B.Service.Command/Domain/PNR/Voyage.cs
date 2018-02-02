using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Command.PNR;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    public class Voyage
    {
        public Voyage(List<Segment> segments)
        {
            _segments = segments;
            _segments.Sort();
        }
        
        public List<Segment> Segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                _segments.Sort();
            }
        }

        /// <summary>
        /// 根据航程列表，判断航程是否连续；
        /// </summary>
        public bool IsContinuousVoyages
        {
            get
            {
                bool flag = true;
                if (Segments.Count > 1)
                {
                    for (int i = 1; i < Segments.Count; i++)
                    {
                        if (Segments[i].DepartureAirport != Segments[i - 1].ArrivalAirport)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                return flag;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public ItineraryType Type
        {
            get
            {
                ItineraryType voyageType;

                if (Segments.Count == 1)
                {
                    voyageType = ItineraryType.OneWay;
                }
                else
                {
                    if (Segments.Count == 2 && Segments[0].DepartureAirport == Segments[1].ArrivalAirport &&
                        Segments[1].DepartureAirport == Segments[0].ArrivalAirport)
                    {
                        voyageType = ItineraryType.Roundtrip;
                    }
                    else
                    {
                        voyageType = IsContinuousVoyages ? ItineraryType.Conjunction : ItineraryType.Notch;
                    }
                }

                return voyageType;
            }
        }
        
        // 航段
        private List<Segment> _segments;
    }
}

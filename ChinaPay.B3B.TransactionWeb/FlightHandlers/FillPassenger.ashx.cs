using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers {
    /// <summary>
    /// 填写乘机人页面的处理
    /// </summary>
    public class FillPassenger : BaseHandler {
        /// <summary>
        /// 查询常旅客
        /// </summary>
        public object GetCustomers(DataTransferObject.Organization.CustomerQueryCondition condition, Pagination pagination) {
            if(condition == null) {
                condition = new DataTransferObject.Organization.CustomerQueryCondition();
            }
            condition.Company = CurrentCompany.CompanyId;
            if(pagination == null) {
                pagination = Pagination.Default;
            }
            var customers = Service.Organization.CustomerService.Query(condition, pagination);
            return new
                       {
                           Pagination = pagination,
                           Customers = customers.Select(item => new
                                                                    {
                                                                        item.Name,
                                                                        Sex = item.Sex.HasValue ? item.Sex.Value == Sex.Male ? "男" : "女" : string.Empty,
                                                                        PassengerType = item.PassengerType.GetDescription(),
                                                                        CredentialsType = item.CredentialsType.ToString(),
                                                                        CredentialsTypeValue = (int)item.CredentialsType,
                                                                        item.Credentials,
                                                                        item.Mobile,
                                                                        item.BirthDay
                                                                    })
                       };
        }
        /// <summary>
        /// 检查乘机人姓名
        /// </summary>
        public void ValidatePassengerName(string name) {
            // 检查名字中间是否有空格
            if(string.IsNullOrWhiteSpace(name)) {
                throw new CustomException("乘机人名字不能为空");
            }
            if(name.Trim().Contains(" ") && Regex.IsMatch(name, "[\u4e00-\u9fa5]")) {
                throw new CustomException("乘机人名字中不能包含空格");
            }
            // 检查名字是否符合格式规范(英文字母后不能再有中文汉字)
            var namePattern = @"(^[\u4e00-\u9fa5]{1,15}$)|(^[\u4e00-\u9fa5]+[a-z,A-Z]+$)|(^[a-z,A-Z]+(/|\s)[a-z,A-Z]+$)";
            if(!Regex.IsMatch(name.Trim(), namePattern) || name.Length > 20) {
                throw new CustomException("乘机人姓名【" + name + "】不符合名字规范");
            }
            // 检查名字是否是规定字符集中内容
            char errorCharacter;
            if(!IsMatchedName(name, out errorCharacter)) {
                throw new CustomException("乘机人姓名" + name + "中：【" + errorCharacter + "】字系统无法识别，请采用汉语拼音代替！");
            }
        }
        /// <summary>
        /// 获取证件号类型
        /// </summary>
        public object GetCredentialsTypes() {
            var credentialsTypes = Enum.GetValues(typeof(CredentialsType)) as CredentialsType[];
            return credentialsTypes.Select(item => new { Text = item.ToString(), Value = (int)item });
        }
        /// <summary>
        /// 验证身份证号
        /// </summary>
        public bool ValidateIdentifyCard(string identifyCardNo) {
            var validator = new IdentityCard.Validator(identifyCardNo);
            validator.Execute();
            return validator.Success;
        }

        public string RegisterCustomer(PassengerView passenger) {
            var customer = getCustomer(passenger);
            try {
                ValidatePassengerName(passenger.Name);
                if(passenger.CredentialsType == CredentialsType.身份证) {
                    ValidateIdentifyCard(passenger.Credentials);
                }
                Service.Organization.CustomerService.Register(CurrentCompany.CompanyId, customer, CurrentUser.UserName);
                return "OK";
            } catch(Exception e) {
                return e.Message;
            }
        }

        /// <summary>
        /// 提交乘机人信息
        /// </summary>
        public void CommitPassengers(List<PassengerView> passengers, Contact contact, string adultPNR) {
            var passengerType = passengers.First().PassengerType;
            if(passengerType == PassengerType.Child && string.IsNullOrWhiteSpace(adultPNR)) throw new CustomException("缺少成人编码");
            checkPassengers(passengers);
            passengers = passengers.OrderBy(p => p.Name).ToList();
            var policyInfo = Session["FlightPolicy"] as DataTransferObject.FlightQuery.PolicyView;
            var flights = Session["ReservedFlights"] as IEnumerable<DataTransferObject.FlightQuery.FlightView>;
            PNRPair pnrCode = null;
            if(flights == null) throw new CustomException("预订超时，请重新选择航班");
            // 如果是儿童票,检查成人编码
            if(passengerType == PassengerType.Child) {
                //FlightReserveModule.PNRHelper.CheckPNRWithETDZ(adultPNR, PassengerType.Adult);
            }
            //var carrier = flights.First().AirlineCode;
            //if (carrier == "CZ" && passengerType == PassengerType.Child)
            //{
            //    passengers.ForEach(p =>
            //                           {
            //                               if (p.CredentialsType == CredentialsType.出生日期)
            //                               {
            //                                   DateTime birthDay;
            //                                   DateTime.TryParse(p.Credentials,out birthDay);
            //                                   p.BirthDay = birthDay;
            //                               }
            //                               else if (p.CredentialsType == CredentialsType.身份证)
            //                               {
            //                                   var match = Regex.Match(p.Credentials, @"\d{6}(\d{4})(\d{2})(\d{2}).{4}");
            //                                   if (match.Success)
            //                                   {
            //                                       p.BirthDay = DateTime.Parse(match.Groups[1].Value+"-"+match.Groups[2]+"-"+match.Groups[3]);
            //                                   }
            //                               }
            //                           });
            //}
            // 非特殊政策，需要订座
            if(policyInfo.Type != PolicyType.Special) {
                pnrCode = FlightReserveModule.PNRHelper.ReserveSeat(flights, passengers);
            }
            var flightViews = flights.Select(FlightReserveModule.ReserveViewConstuctor.GetOrderFlightView).ToList();
            var orderView = new OrderView {
                FdSuccess = true,
                Source = OrderSource.PlatformOrder,
                PNR = pnrCode,
                Passengers = passengers,
                Flights = flightViews,
                Contact = contact,
                IsTeam = false,
                TripType = flightViews.Count == 2 ? ItineraryType.Roundtrip : ItineraryType.OneWay
            };
            if(passengerType == PassengerType.Child) {
                orderView.AssociatePNR = new PNRPair(adultPNR, string.Empty);
                foreach(var flight in flights) {
                    if(flight.Discount.HasValue) {
                        flight.Fare = Utility.Calculator.Round((flight.Discount >= 1 ? flight.Fare : flight.YBPrice) / 2, 1);
                        flight.Discount = Utility.Calculator.Round(flight.Fare / flight.YBPrice, -3);
                    }
                    flight.AirportFee = 0;
                    flight.BAF = flight.ChildBAF;
                }
            }
            if(FlightReserveModule.PNRHelper.RequirePat(flights, policyInfo.Type))
                orderView.PATPrice = FlightReserveModule.PNRHelper.Pat(pnrCode, flights, passengerType);
            Session["OrderView"] = orderView;
        }
        private void checkPassengers(List<PassengerView> passengers) {
            // 检查乘机人信息
            passengers.ForEach(item => {
                ValidatePassengerName(item.Name);
                if(item.CredentialsType == CredentialsType.身份证 && !ValidateIdentifyCard(item.Credentials)) {
                    throw new CustomException("乘机人[" + item.Name + "]的证件号格式错误");
                }
            });
            // 检查是否有同名字或相同证件号
            checkHasSamePassenger(passengers);
        }
        private void checkHasSamePassenger(List<PassengerView> passengers) {
            var checkedPassengers = new List<PassengerView>();
            passengers.ForEach(item => {
                if(checkedPassengers.Exists(p => string.Compare(p.Name, item.Name, StringComparison.OrdinalIgnoreCase) == 0
                    || string.Compare(p.Credentials, item.Credentials, StringComparison.OrdinalIgnoreCase) == 0)) {
                    throw new CustomException("同一编码中不能出现相同名字或证件号的乘机人");
                } else {
                    checkedPassengers.Add(item);
                }
            });
        }
        private DataTransferObject.Organization.Customer getCustomer(PassengerView passenger) {
            return new DataTransferObject.Organization.Customer {
                Name = passenger.Name,
                CredentialsType = passenger.CredentialsType,
                Credentials = passenger.Credentials,
                PassengerType = passenger.PassengerType,
                Mobile = passenger.Phone,
                BirthDay = passenger.BirthDay
            };
        }

        /// <summary>
        /// 检查中文字符串中是否是GB2312字符集内的汉字
        /// </summary>
        /// <param name="value">中文字符串</param>
        /// <param name="errorCharacter">GB2312字符集外的汉字</param>
        /// <returns>true 全是GB2312字符集内汉字 false 存在GB2312字符集外汉字</returns>
        private static bool IsMatchedName(string value, out char errorCharacter) {
            errorCharacter = ' ';
            if(string.IsNullOrWhiteSpace(value)) {
                return false;
            }
            foreach(var item in value) {
                if(item == ' ') continue;
                if(!IsGB2312(item)) {
                    errorCharacter = item;
                    return false;
                }
            }
            return true;
        }
        private static bool IsGB2312(char character) {
            var byteArray = System.Text.Encoding.GetEncoding("gb2312").GetBytes(character.ToString());
            if(byteArray.Length == 2) {
                if((byteArray[0] >= 0xB0 && byteArray[0] <= 0xF7) && (byteArray[1] >= 0xA1 && byteArray[1] <= 0xFE)) {
                    if(byteArray[0] == 0xD7 && byteArray[1] >= 0xFA && byteArray[1] <= 0xFE) {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
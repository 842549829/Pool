using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.SystemSetting
{
    public class SuggestService
    {
        public static bool AddSuggest(Suggest suggest)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreateSuggestRepository(command);
                db.Insert(suggest);
            }
                return true;
        }

        public static IEnumerable<Suggest> GetSuggest(DateTime? start, DateTime? end, SuggestCategory? category, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreateSuggestRepository(command);
                var result = db.Query(start, end, category, pagination);
                return result;
            }
        }
    }

}
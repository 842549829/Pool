using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Configuration;

namespace ChinaPay.B3B.Service.RegularExpression.Repository.Xml
{
    using RegularExpression = Domain.RegularExpression;
    class RegExRepository : IRegExRepository
    {
        IEnumerable<Domain.RegularExpression> IRegExRepository.Query()
        {
            string path = System.Web.HttpContext.Current == null ? System.Environment.CurrentDirectory + "/" : System.Web.HttpContext.Current.Server.MapPath("/");
            var url = string.Format("{0}{1}", path, ConfigurationManager.AppSettings["RegularExpressionConfig"]);

            //var url = @"D:\Work\代码\ChinaPay.B3B.TransactionWeb\RegularExpression.xml";
            var xdoc = XDocument.Load(url, LoadOptions.None);
            var result = from re in xdoc.Descendants("RegularExpression")
                         let id = re.Attribute("id").Value
                         let value = re.Element("value").Value
                         let example = re.Element("example").Value
                         let description = re.Element("description").Value
                         select new RegularExpression(id, value, example, description);
            return result;
        }
    }
}

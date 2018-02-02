using ChinaPay.B3B.Service.RegularExpression.Repository.Xml;

namespace ChinaPay.B3B.Service.RegularExpression.Repository
{
    class Factory
    {
        public static IRegExRepository CreateRegExRepository()
        {
            // 这里直接新建
            return new RegExRepository();
        }
    }
}

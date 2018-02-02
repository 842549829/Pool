using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// WCF方法执行工具类
    /// </summary>
    class WCFUtil
    {
        public static void Invoke<TContract>(TContract proxy, Action<TContract> action)
        {
            try
            {
                // 执行
                action(proxy);
                // 关闭
                (proxy as ICommunicationObject).Close();
            }
            catch (CommunicationException)
            {
                // 通讯错误异常；
                (proxy as ICommunicationObject).Abort();
                throw new CustomException("与资源服务器通讯错误");
            }
            catch (TimeoutException)
            {
                // 超时错误异常；
                (proxy as ICommunicationObject).Abort();
                throw new CustomException("与资源服务器通讯超时"); 
            }
            catch (System.Exception)
            {
                // 其它异常
                (proxy as ICommunicationObject).Close();
                throw new CustomException("与资源服务器交互时出现未知错误"); 
            }
        }
        
        public static TReturn Invoke<TContract, TReturn>(TContract proxy, Func<TContract, TReturn> func)
        {
            TReturn returnValue = default(TReturn);
            try
            {
                // 执行
                returnValue = func(proxy);
                // 关闭
                (proxy as ICommunicationObject).Close();
            }
            catch (CommunicationException)
            {
                // 通讯错误异常；
                (proxy as ICommunicationObject).Abort();
                throw new CustomException("与资源服务器通讯错误");
            }
            catch (TimeoutException)
            {
                // 超时错误异常；
                (proxy as ICommunicationObject).Abort();
                throw new CustomException("与资源服务器通讯超时");
            }
            catch (System.Exception)
            {
                // 其它异常
                (proxy as ICommunicationObject).Close();
                throw new CustomException("与资源服务器交互时出现未知错误");
            }
            return returnValue;
        }
    }
}

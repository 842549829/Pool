﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChinaPay.B3B.Service.Command.XapiServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.b3b.so/xapi/", ConfigurationName="XapiServiceReference.IXapiService")]
    public interface IXapiService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.b3b.so/xapi/IXapiService/GetMessage", ReplyAction="http://www.b3b.so/xapi/IXapiService/GetMessageResponse")]
        string GetMessage(string cmd, byte commandType, byte lockfirst, byte flag, byte type, string tid, short agentId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IXapiServiceChannel : ChinaPay.B3B.Service.Command.XapiServiceReference.IXapiService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class XapiServiceClient : System.ServiceModel.ClientBase<ChinaPay.B3B.Service.Command.XapiServiceReference.IXapiService>, ChinaPay.B3B.Service.Command.XapiServiceReference.IXapiService {
        
        public XapiServiceClient() {
        }
        
        public XapiServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public XapiServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public XapiServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public XapiServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetMessage(string cmd, byte commandType, byte lockfirst, byte flag, byte type, string tid, short agentId) {
            return base.Channel.GetMessage(cmd, commandType, lockfirst, flag, type, tid, agentId);
        }
    }
}
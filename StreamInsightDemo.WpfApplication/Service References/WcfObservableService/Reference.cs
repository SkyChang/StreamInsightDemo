﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18046
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace StreamInsightDemo.WpfApplication.WcfObservableService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Microsoft.ServiceModel.WcfObservable", ConfigurationName="WcfObservableService.IWCFObservable")]
    public interface IWCFObservable {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnCompleted")]
        void OnCompleted();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnCompleted")]
        System.Threading.Tasks.Task OnCompletedAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnError")]
        void OnError(System.Exception error);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnError")]
        System.Threading.Tasks.Task OnErrorAsync(System.Exception error);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnNext", ReplyAction="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnNextResponse")]
        void OnNext(StreamInsightDemo.Model.InputEvent value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnNext", ReplyAction="http://Microsoft.ServiceModel.WcfObservable/IWCFObservable/OnNextResponse")]
        System.Threading.Tasks.Task OnNextAsync(StreamInsightDemo.Model.InputEvent value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWCFObservableChannel : StreamInsightDemo.WpfApplication.WcfObservableService.IWCFObservable, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WCFObservableClient : System.ServiceModel.ClientBase<StreamInsightDemo.WpfApplication.WcfObservableService.IWCFObservable>, StreamInsightDemo.WpfApplication.WcfObservableService.IWCFObservable {
        
        public WCFObservableClient() {
        }
        
        public WCFObservableClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WCFObservableClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFObservableClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFObservableClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OnCompleted() {
            base.Channel.OnCompleted();
        }
        
        public System.Threading.Tasks.Task OnCompletedAsync() {
            return base.Channel.OnCompletedAsync();
        }
        
        public void OnError(System.Exception error) {
            base.Channel.OnError(error);
        }
        
        public System.Threading.Tasks.Task OnErrorAsync(System.Exception error) {
            return base.Channel.OnErrorAsync(error);
        }
        
        public void OnNext(StreamInsightDemo.Model.InputEvent value) {
            base.Channel.OnNext(value);
        }
        
        public System.Threading.Tasks.Task OnNextAsync(StreamInsightDemo.Model.InputEvent value) {
            return base.Channel.OnNextAsync(value);
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5420
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.CompactFramework.Design.Data 2.0.50727.5420 版自动生成。
// 
namespace PDA.PlatformServer {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    using System.Data;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IServiceBase", Namespace="http://tempuri.org/")]
    public partial class ServiceBase : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public ServiceBase() {
            this.Url = "http://218.91.152.186:8045/ServiceBase.svc";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/GetDt", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Data.DataTable GetDt([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("GetDt", new object[] {
                        sql});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDt(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDt", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataTable EndGetDt(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/GetDtSap", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Data.DataTable GetDtSap([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("GetDtSap", new object[] {
                        sql});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDtSap(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDtSap", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataTable EndGetDtSap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/RunSql", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result RunSql([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("RunSql", new object[] {
                        sql});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRunSql(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RunSql", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndRunSql(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/RunSqlSap", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result RunSqlSap([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("RunSqlSap", new object[] {
                        sql});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRunSqlSap(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RunSqlSap", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndRunSqlSap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/RunSqls", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result RunSqls([System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] string[] Sqls) {
            object[] results = this.Invoke("RunSqls", new object[] {
                        Sqls});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRunSqls(string[] Sqls, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RunSqls", new object[] {
                        Sqls}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndRunSqls(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/RunSqlsSap", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result RunSqlsSap([System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] string[] Sqls) {
            object[] results = this.Invoke("RunSqlsSap", new object[] {
                        Sqls});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRunSqlsSap(string[] Sqls, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RunSqlsSap", new object[] {
                        Sqls}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndRunSqlsSap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/Insert", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result Insert([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("Insert", new object[] {
                        sql});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginInsert(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Insert", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndInsert(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/RunScalar", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result RunScalar([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string sql) {
            object[] results = this.Invoke("RunScalar", new object[] {
                        sql});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRunScalar(string sql, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RunScalar", new object[] {
                        sql}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndRunScalar(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IServiceBase/SaveImage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Result SaveImage([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string tableName, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string fieldName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] image, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string PK, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string PKValue) {
            object[] results = this.Invoke("SaveImage", new object[] {
                        tableName,
                        fieldName,
                        image,
                        PK,
                        PKValue});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSaveImage(string tableName, string fieldName, byte[] image, string PK, string PKValue, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SaveImage", new object[] {
                        tableName,
                        fieldName,
                        image,
                        PK,
                        PKValue}, callback, asyncState);
        }
        
        /// <remarks/>
        public Result EndSaveImage(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Result)(results[0]));
        }
    }
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/PlatformServer")]
    public partial class Result {
        
        private string messageField;
        
        private int statusField;
        
        private bool statusFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public int Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
            }
        }
    }
}

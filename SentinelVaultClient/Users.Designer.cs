﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SentinelVaultClient {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Users : global::System.Configuration.ApplicationSettingsBase {
        
        private static Users defaultInstance = ((Users)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Users())));
        
        public static Users Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DEVICEIDENTIES {
            get {
                return ((string)(this["DEVICEIDENTIES"]));
            }
            set {
                this["DEVICEIDENTIES"] = value;
            }
        }
    }
}
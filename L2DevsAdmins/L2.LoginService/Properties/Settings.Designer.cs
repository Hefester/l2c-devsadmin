﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace L2.Net.LoginService.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("201")]
        public byte ServiceUniqueID {
            get {
                return ((byte)(this["ServiceUniqueID"]));
            }
            set {
                this["ServiceUniqueID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string CacheServiceAddress {
            get {
                return ((string)(this["CacheServiceAddress"]));
            }
            set {
                this["CacheServiceAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3001")]
        public int CacheServicePort {
            get {
                return ((int)(this["CacheServicePort"]));
            }
            set {
                this["CacheServicePort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:00:10")]
        public global::System.TimeSpan CacheServiceReconnectInterval {
            get {
                return ((global::System.TimeSpan)(this["CacheServiceReconnectInterval"]));
            }
            set {
                this["CacheServiceReconnectInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CacheServiceAutoReconnect {
            get {
                return ((bool)(this["CacheServiceAutoReconnect"]));
            }
            set {
                this["CacheServiceAutoReconnect"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string LoginServiceAddress {
            get {
                return ((string)(this["LoginServiceAddress"]));
            }
            set {
                this["LoginServiceAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2106")]
        public int LoginServicePort {
            get {
                return ((int)(this["LoginServicePort"]));
            }
            set {
                this["LoginServicePort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int LoginServiceBacklog {
            get {
                return ((int)(this["LoginServiceBacklog"]));
            }
            set {
                this["LoginServiceBacklog"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool LoginServiceEnableFirewall {
            get {
                return ((bool)(this["LoginServiceEnableFirewall"]));
            }
            set {
                this["LoginServiceEnableFirewall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool LoginServiceAutoCreateUsers {
            get {
                return ((bool)(this["LoginServiceAutoCreateUsers"]));
            }
            set {
                this["LoginServiceAutoCreateUsers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public byte LoginServiceDefaultAccessLevel {
            get {
                return ((byte)(this["LoginServiceDefaultAccessLevel"]));
            }
            set {
                this["LoginServiceDefaultAccessLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public byte LoginServiceAllowedAccessLevel {
            get {
                return ((byte)(this["LoginServiceAllowedAccessLevel"]));
            }
            set {
                this["LoginServiceAllowedAccessLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public byte LoginServicePasswordsProtectionLevel {
            get {
                return ((byte)(this["LoginServicePasswordsProtectionLevel"]));
            }
            set {
                this["LoginServicePasswordsProtectionLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[A-Za-z0-9]")]
        public string LoginServiceUserLoginRegex {
            get {
                return ((string)(this["LoginServiceUserLoginRegex"]));
            }
            set {
                this["LoginServiceUserLoginRegex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int LoginServiceMaxAwaitingUsersCount {
            get {
                return ((int)(this["LoginServiceMaxAwaitingUsersCount"]));
            }
            set {
                this["LoginServiceMaxAwaitingUsersCount"] = value;
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeBinder.Apple {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ObjCResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ObjCResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CodeBinder.Apple.ObjC.Resources.ObjCResources", typeof(ObjCResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #ifndef CBOCINTEROP_HEADER
        ///#define CBOCINTEROP_HEADER
        ///#pragma once
        ///
        ///#import &lt;Foundation/Foundation.h&gt;
        ///#include &lt;CBInterop.h&gt;
        ///
        ///class SN2OC
        ///{
        ///private:
        ///    bool m_handled;
        ///    cbstring_t m_cstr;
        ///	NSString * __strong * m_ocstr;	
        ///public:
        ///    SN2OC(NSString * __strong * str)
        ///	{
        ///		m_handled = true;
        ///		m_cstr = nullptr;
        ///		m_ocstr = str;	
        ///	}
        ///
        ///    SN2OC(const cbstring_t str)
        ///	{
        ///		m_handled = false;		
        ///		m_cstr = (cbstring_t)str;
        ///		m_ocstr = nullptr;
        ///	}
        ///	
        ///    SN2OC(cbstring_t &amp;&amp;str)
        ///	{
        ///		m_ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CBOCInterop_h {
            get {
                return ResourceManager.GetString("CBOCInterop_h", resourceCulture);
            }
        }
    }
}

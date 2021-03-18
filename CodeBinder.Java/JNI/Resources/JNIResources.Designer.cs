﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeBinder.JNI.Resources {
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
    internal class JNIResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal JNIResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CodeBinder.Java.JNI.Resources.JNIResources", typeof(JNIResources).Assembly);
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
        ///   Looks up a localized string similar to #include &quot;JNITypes.h&quot;
        ///
        ///extern &quot;C&quot;
        ///{
        ///    JNIEXPORT jlong JNICALL Java_CodeBinder_BinderUtils_newGlobalRef(
        ///        JNIEnv *env, jclass, jobject obj)
        ///    {
        ///        return (jlong)env-&gt;NewGlobalRef(obj);
        ///    }
        ///
        ///    JNIEXPORT void JNICALL Java_CodeBinder_BinderUtils_deleteGlobalRef(
        ///        JNIEnv *env, jclass, jlong globalref)
        ///    {
        ///        env-&gt;DeleteGlobalRef((jobject)globalref);
        ///    }
        ///	
        ///	JNIEXPORT jlong JNICALL Java_CodeBinder_BinderUtils_newGlobalWeakRef(
        ///		JNIEnv *env, jclass, jobject obj) [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIBinderUtils_cpp {
            get {
                return ResourceManager.GetString("JNIBinderUtils_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &quot;JNIBoxes.h&quot;
        ///
        ///BJ2NImpl&lt;_jBooleanBox&gt; BJ2N(JNIEnv * env, jBooleanBox box, bool commit)
        ///{
        ///    return BJ2NImpl&lt;_jBooleanBox&gt;(env, box, commit);
        ///}
        ///
        ///BJ2NImpl&lt;_jCharacterBox&gt; BJ2N(JNIEnv * env, jCharacterBox box, bool commit)
        ///{
        ///    return BJ2NImpl&lt;_jCharacterBox&gt;(env, box, commit);
        ///}
        ///
        ///BJ2NImpl&lt;_jByteBox&gt; BJ2N(JNIEnv * env, jByteBox box, bool commit)
        ///{
        ///    return BJ2NImpl&lt;_jByteBox&gt;(env, box, commit);
        ///}
        ///
        ///BJ2NImpl&lt;_jShortBox&gt; BJ2N(JNIEnv * env, jShortBox box, bool commit)
        ///{
        ///    return BJ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIBoxes_cpp {
            get {
                return ResourceManager.GetString("JNIBoxes_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /* This file was generated. DO NOT EDIT! */
        ///#pragma once
        ///
        ///#include &quot;JNITypesPrivate.h&quot;
        ///
        ///// Wraps custom java box type
        ///template &lt;typename TJBox, typename TN = typename TJBox::ValueType&gt;
        ///class BJ2NImpl
        ///{
        ///public:
        ///    BJ2NImpl(JNIEnv* env, typename TJBox::BoxPtr box, bool commit)
        ///    {
        ///        m_env = env;
        ///        m_box = box;
        ///        m_commit = commit;
        ///        Value = (TN)box-&gt;GetValue(env);
        ///    }
        ///    ~BJ2NImpl()
        ///    {
        ///        if (m_commit)
        ///            m_box-&gt;SetValue(m_env, (typename TJBox [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIBoxes_h {
            get {
                return ResourceManager.GetString("JNIBoxes_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &quot;JNICommon.h&quot;
        ///#include &lt;utility&gt;
        ///#include &lt;string&gt;
        ///#include &lt;cassert&gt;
        ///#include &lt;CBInterop.h&gt;
        ///
        ///using namespace std;
        ///
        ///SJ2N::SJ2N(JNIEnv* env, jstring str)
        ///    : m_env(env), m_string(str), m_chars(nullptr), m_isCopy(false)
        ///{
        ///    if (m_string != nullptr)
        ///        m_chars = m_env-&gt;GetStringUTFChars(m_string, &amp;m_isCopy);
        ///}
        ///
        ///SN2J::SN2J(JNIEnv* env, const cbstring&amp; str)
        ///    : m_handled(false), m_env(env), m_string(str) { }
        ///
        ///// Move semantics
        ///SN2J::SN2J(JNIEnv* env, cbstring&amp;&amp; str)
        ///    :  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNICommon_cpp {
            get {
                return ResourceManager.GetString("JNICommon_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &lt;jni.h&gt;
        ///#include &lt;stdexcept&gt;
        ///#include &quot;JNIShared.h&quot;
        ///#include &quot;JNIBoxes.h&quot;
        ///#include &lt;CBBaseTypes.h&gt;
        ///
        ///// Wraps jstring and convert to utf-16 chars
        ///class SJ2N
        ///{
        ///public:
        ///    SJ2N(JNIEnv* env, jstring str);
        ///    ~SJ2N();
        ///public:
        ///    operator cbstring() const;
        ///private:
        ///    JNIEnv* m_env;
        ///    jstring m_string;
        ///    const char* m_chars;
        ///    jboolean m_isCopy;
        ///};
        ///
        ///// Wraps utf-16 chars and convert to jstring
        ///class SN2J
        ///{
        ///public:
        ///    SN2J(JNIEnv* env, const cbstring&amp; st [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNICommon_h {
            get {
                return ResourceManager.GetString("JNICommon_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &lt;jni.h&gt;
        ///#include &lt;cassert&gt;
        ///#include &quot;JNIShared.h&quot;
        ///
        ///#define JNI_VERSION JNI_VERSION_1_6
        ///
        ///static JavaVM* s_jvm;
        ///
        ///static jfieldID handleFieldID;
        ///
        ///static JNIEnv* getEnv(JavaVM* jvm);
        ///
        ///jlong GetHandle(JNIEnv* env, jHandleRef handleref)
        ///{
        ///    return env-&gt;GetLongField(handleref, handleFieldID);
        ///}
        ///
        ///JNIEnv* GetEnv()
        ///{
        ///    return getEnv(s_jvm);
        ///}
        ///
        ///JNIEnv* getEnv(JavaVM* jvm)
        ///{
        ///    // GetEnv can be used only if current thread was created
        ///    // with Java, otherwise AttachCurrentProces [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIShared_cpp {
            get {
                return ResourceManager.GetString("JNIShared_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &quot;JNITypesPrivate.h&quot;
        ///
        ///jlong GetHandle(JNIEnv* env, jHandleRef handleref);
        ///JNIEnv* GetEnv();
        ///JavaVM* GetJvm();
        ///.
        /// </summary>
        internal static string JNIShared_h {
            get {
                return ResourceManager.GetString("JNIShared_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &quot;JNITypesPrivate.h&quot;
        ///
        ///const char * _jBooleanBoxBase::getFieldIdSignature()
        ///{
        ///    return &quot;Z&quot;;
        ///}
        ///
        ///_jBooleanBoxBase::ValueType _jBooleanBoxBase::getValue(JNIEnv * env, jfieldID field) const
        ///{
        ///    return env-&gt;GetBooleanField((jobject)this, field);
        ///}
        ///
        ///void _jBooleanBoxBase::setValue(JNIEnv * env, jfieldID field, ValueType value)
        ///{
        ///    env-&gt;SetBooleanField(this, field, value);
        ///}
        ///
        ///const char * _jCharacterBoxBase::getFieldIdSignature()
        ///{
        ///    return &quot;C&quot;;
        ///}
        ///
        ///_jCharacterBoxBase::ValueType [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNITypes_cpp {
            get {
                return ResourceManager.GetString("JNITypes_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &lt;jni.h&gt;
        ///
        ///#if defined(__APPLE__) &amp;&amp; !defined(JNI_VERSION_1_8)
        ///// Workaround for old macosx JDK7 build that doesn&apos;t
        ///// export symbols on gcc/clang
        ///// TODO: Create a custom jni.h wrapper header and include it
        ///// as a more common header
        ///#undef JNIIMPORT
        ///#undef JNIEXPORT
        ///#define JNIIMPORT __attribute__((visibility(&quot;default&quot;)))
        ///#define JNIEXPORT __attribute__((visibility(&quot;default&quot;)))
        ///#endif
        ///
        ///#define jBooleanBox jobject
        ///#define jCharacterBox jobject
        ///#define jByteBox jobject        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNITypes_h {
            get {
                return ResourceManager.GetString("JNITypes_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &quot;JNITypesPrivate.h&quot;
        ///
        ///#include &quot;JNIShared.h&quot;
        ///
        ///jlong _jHandleRef::getHandle(JNIEnv *env)
        ///{
        ///    return ::GetHandle(env, this);
        ///}
        ///.
        /// </summary>
        internal static string JNITypesPrivate_cpp {
            get {
                return ResourceManager.GetString("JNITypesPrivate_cpp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &quot;JNITypes.h&quot;
        ///
        ///#undef jBooleanBox
        ///#undef jCharacterBox
        ///#undef jByteBox
        ///#undef jShortBox
        ///#undef jIntegerBox
        ///#undef jLongBox
        ///#undef jFloatBox
        ///#undef jDoubleBox
        ///#undef jStringBox
        ///#undef jHandleRef
        ///
        ///// Template for box types
        ///template &lt;typename BaseT&gt;
        ///class _jTypeBox : public BaseT
        ///{
        ///public:
        ///    typename BaseT::ValueType GetValue(JNIEnv *env) const;
        ///    void SetValue(JNIEnv *env, typename BaseT::ValueType value);
        ///private:
        ///    jfieldID getFieldId(JNIEnv *env) const;
        ///} [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNITypesPrivate_h {
            get {
                return ResourceManager.GetString("JNITypesPrivate_h", resourceCulture);
            }
        }
    }
}

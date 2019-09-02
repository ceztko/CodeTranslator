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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
        ///   Looks up a localized string similar to #include &lt;jni.h&gt;
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
        ///		JNIEnv *env, jclass, jobject obj)
        ///	{        /// [rest of string was truncated]&quot;;.
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
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &quot;JNIBoxesTemplate.h&quot;
        ///
        ///BJ2NImpl&lt;_jBooleanBox&gt; BJ2N(JNIEnv *env, jBooleanBox box, bool commit = true);
        ///BJ2NImpl&lt;_jCharacterBox&gt; BJ2N(JNIEnv *env, jCharacterBox box, bool commit = true);
        ///BJ2NImpl&lt;_jByteBox&gt; BJ2N(JNIEnv *env, jByteBox box, bool commit = true);
        ///BJ2NImpl&lt;_jShortBox&gt; BJ2N(JNIEnv *env, jShortBox box, bool commit = true);
        ///BJ2NImpl&lt;_jIntegerBox&gt; BJ2N(JNIEnv *env, jIntegerBox box, bool commit = true);
        ///BJ2NImpl&lt;_jLongBox&gt; BJ2N(JNIEnv *env, jLongBox box, bool commit = tru [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIBoxes_h {
            get {
                return ResourceManager.GetString("JNIBoxes_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #pragma once
        ///
        ///#include &lt;stdexcept&gt;
        ///#include &quot;JNITypesPrivate.h&quot;
        ///
        ///// Wraps custom java box type
        ///template &lt;typename TJBox, typename TN = typename TJBox::ValueType&gt;
        ///class BJ2NImpl
        ///{
        ///public:
        ///    BJ2NImpl(JNIEnv *env, typename TJBox::BoxPtr box, bool commit);
        ///    ~BJ2NImpl();
        ///public:
        ///    inline TN * ptr() { return &amp;Value; }
        ///    inline TN &amp; ref() { return Value; }
        ///    inline operator TN *() { return &amp;Value; }
        ///    inline operator TN &amp;() { return Value; }
        ///public:
        ///    TN Value;
        ///private:
        ///    JNIEn [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JNIBoxesTemplate_h {
            get {
                return ResourceManager.GetString("JNIBoxesTemplate_h", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &lt;jni.h&gt;
        ///#include &lt;cassert&gt;
        ///#include &quot;JNIShared.h&quot;
        ///
        ///#define JNI_VERSION JNI_VERSION_1_6
        ///
        ///static jfieldID handleFieldID;
        ///
        ///jlong GetHandle(JNIEnv *env, jHandleRef handleref)
        ///{
        ///    return env-&gt;GetLongField(handleref, handleFieldID);
        ///}
        ///
        ///JNIEnv * GetEnv(JavaVM *jvm)
        ///{
        ///    // GetEnv can be used only if current thread was created
        ///    // with Java, otherwise AttachCurrentProcess should be used
        ///    // instead
        ///    JNIEnv *env;
        ///    jint rs = jvm-&gt;GetEnv((void **)&amp;env, JNI_VERSION);
        ///    asse [rest of string was truncated]&quot;;.
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
        ///jlong GetHandle(JNIEnv *env, jHandleRef handleref);
        ///JNIEnv * GetEnv(JavaVM *jvm);
        ///JavaVM * GetJvm(JNIEnv *env);
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
        ///#define jBooleanBox jobject
        ///#define jCharacterBox jobject
        ///#define jByteBox jobject
        ///#define jShortBox jobject
        ///#define jIntegerBox jobject
        ///#define jLongBox jobject
        ///#define jFloatBox jobject
        ///#define jDoubleBox jobject
        ///#define jStringBox jobject
        ///#define jHandleRef jobject
        ///.
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

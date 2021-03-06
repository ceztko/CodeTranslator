﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using CodeBinder.Shared;
using CodeBinder.Attributes;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace CodeBinder.CLang
{
    public static class CLangMethodExtensions
    {
        enum ParameterType
        {
            Regular,
            ByRef,
            Return
        }

        public static CodeBuilder Append(this CodeBuilder builder, ParameterSyntax parameter,
            ICompilationContextProvider provider)
        {
            return Append(builder, parameter, false, provider);
        }

        public static CodeBuilder Append(this CodeBuilder builder, ParameterSyntax parameter,
            bool cppMethod, ICompilationContextProvider provider)
        {
            bool isByRef = parameter.IsRef() || parameter.IsOut();
            var symbol = parameter.Type!.GetTypeSymbol(provider);
            string? suffix;
            string type = getCLangType(symbol, parameter.GetAttributes(provider),
                isByRef ? ParameterType.ByRef : ParameterType.Regular, cppMethod, out suffix);
            builder.Append(type).Space().Append(parameter.Identifier.Text);
            if (suffix != null)
                builder.Append(suffix);

            return builder;
        }

        public static bool TryGetCLangBinder(this ParameterSyntax parameter, ICompilationContextProvider provider, [NotNullWhen(true)] out string? binderStr)
        {
            return TryGetCLangBinder(parameter, false, provider, out binderStr);
        }

        public static bool TryGetCLangBinder(this ParameterSyntax parameter, bool pointerType, ICompilationContextProvider provider, [NotNullWhen(true)] out string? binderStr)
        {
            var symbol = parameter.Type!.GetTypeSymbol(provider);
            if (symbol.TypeKind == TypeKind.Enum)
            {
                var attributes = symbol.GetAttributes();
                binderStr = attributes.GetAttribute<NativeBindingAttribute>().GetConstructorArgument<string>(0);
                return true;
            }

            var binder = parameter.GetAttributes(provider).FirstOrDefault((item) => item.Inherits<NativeTypeBinder>());
            if (binder == null)
            {
                binderStr = null;
                return false;
            }

            if (pointerType)
                binderStr = $"{binder.AttributeClass!.Name}*";
            else
                binderStr = $"{binder.AttributeClass!.Name}";
            return true;
        }

        public static string GetCLangType(this ParameterSyntax parameter, ICompilationContextProvider provider)
        {
            var symbol = parameter.Type!.GetTypeSymbol(provider);
            return getCLangType(symbol, parameter.GetAttributes(provider), ParameterType.Regular, false, out _);
        }

        public static string GetCLangType(this SpecialType type)
        {
            switch(type)
            {
                case SpecialType.System_Boolean:
                    return "cbbool";
                case SpecialType.System_Byte:
                    return "uint8_t";
                case SpecialType.System_SByte:
                    return "int8_t";
                case SpecialType.System_UInt16:
                    return "uint16_t";
                case SpecialType.System_Int16:
                    return "int16_t";
                case SpecialType.System_UInt32:
                    return "uint32_t";
                case SpecialType.System_Int32:
                    return "int32_t";
                case SpecialType.System_UInt64:
                    return "uint64_t";
                case SpecialType.System_Int64:
                    return "int64_t";
                case SpecialType.System_Single:
                    return "float";
                case SpecialType.System_Double:
                    return "double";
                case SpecialType.System_IntPtr:
                    return "void*";
                case SpecialType.System_UIntPtr:
                    return "void*";
                default:
                    throw new Exception($"Unsupported by type {type}");
            }
        }

        public static string GetCLangMethodName(this MethodDeclarationSyntax method)
        {
            return method.GetName();
        }

        internal static string GetCLangReturnType(this MethodDeclarationSyntax method,
            ICompilationContextProvider provider)
        {
            return GetCLangReturnType(method, false, provider);
        }

        internal static string GetCLangReturnType(this MethodDeclarationSyntax method, bool cppMethod, ICompilationContextProvider provider)
        {
            var symbol = method.GetDeclaredSymbol<IMethodSymbol>(provider);
            return getCLangReturnType(cppMethod, symbol);
        }

        public static string GetCLangReturnType(this DelegateDeclarationSyntax dlg, ICompilationContextProvider provider)
        {
            // TODO: Should be possible to prpare static cpp trampolines also for delegates.
            // Maybe not so easy
            var symbol = dlg.GetDeclaredSymbol<INamedTypeSymbol>(provider);
            return getCLangReturnType(false, symbol.DelegateInvokeMethod!);
        }

        private static string getCLangReturnType(bool cppMethod, IMethodSymbol method)
        {
            string? suffix;
            string type = getCLangType(method.ReturnType, method.GetReturnTypeAttributes(),
                ParameterType.Return, cppMethod, out suffix);
            if (suffix == null)
                return type;
            else
                return $"{type} {suffix}";
        }

        private static string getCLangType(ITypeSymbol symbol, IEnumerable<AttributeData> attributes,
            ParameterType type, bool cppMethod, out string? suffix)
        {
            switch(symbol.TypeKind)
            {
                // Handle some special types first
                case TypeKind.Enum:
                {
                    suffix = null;
                    var bindingAttr = symbol.GetAttribute<NativeBindingAttribute>();
                    string binded = bindingAttr.GetConstructorArgument<string>(0);
                    if (type == ParameterType.ByRef)
                        return $"{binded} *";
                    else
                        return binded;
                }
                case TypeKind.Delegate:
                {
                    suffix = null;
                    var bindingAttr = symbol.GetAttribute<NativeBindingAttribute>();
                    return bindingAttr.GetConstructorArgument<string>(0);
                }
            }

            string typeName;
            bool constParameter = false;
            if (symbol.TypeKind == TypeKind.Array)
            {
                var arrayType = (IArrayTypeSymbol)symbol;
                typeName = arrayType.ElementType.GetFullName();

                if (type == ParameterType.Return)
                {
                    suffix = null;
                    if (attributes.HasAttribute<ConstAttribute>())
                        constParameter = true;
                }
                else
                {
                    if (!attributes.HasAttribute<OutAttribute>())
                        constParameter = true;

                    int fixedSizeArray;
                    if (attributes.TryGetAttribute<MarshalAsAttribute>(out var marshalAsAttr) &&
                        marshalAsAttr.TryGetNamedArgument("SizeConst", out fixedSizeArray))
                    {
                        suffix = $"[{fixedSizeArray}]";
                    }
                    else
                    {
                        suffix = "[]";
                    }
                }
            }
            else
            {
                suffix = null;
                typeName = symbol.GetFullName();
            }

            // TODO: Optmize CLR types with ITypeSymbol.SpecialType, handling of constParameter,
            // CHECK-ME evaluate supporting string arrays
            string bindedType;
            if (typeName == "CodeBinder.cbstring")
            {
                switch (type)
                {
                    case ParameterType.Regular:
                    {
                        if (cppMethod)
                        {
                            constParameter |= true;
                            bindedType = "cbstringp&";
                        }
                        else
                        {
                            bindedType = "cbstring";
                        }

                        break;
                    }
                    case ParameterType.Return:
                    {
                        if (cppMethod)
                            bindedType= "cbstringr";
                        else
                            bindedType= "cbstring";
                        break;
                    }
                    case ParameterType.ByRef:
                    {
                        if (cppMethod)
                            bindedType = "cbstringpr&";
                        else
                            bindedType = "cbstring*";
                        break;
                    }
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                switch (type)
                {
                    case ParameterType.Regular:
                        bindedType = getCLangType(typeName, attributes);
                        break;
                    case ParameterType.Return:
                        if (symbol.TypeKind == TypeKind.Array)
                            bindedType = getCLangPointerType(typeName, attributes);
                        else
                            bindedType = getCLangType(typeName, attributes);

                        break;
                    case ParameterType.ByRef:
                        bindedType = getCLangPointerType(typeName, attributes);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            if (constParameter)
                return $"const {bindedType}";
            else
                return bindedType;
        }

        static string getCLangType(string typeName, IEnumerable<AttributeData> attributes)
        {
            switch (typeName)
            {
                case "System.Void":
                    return "void";
                case "System.Runtime.InteropServices.HandleRef":
                case "System.IntPtr":
                {
                    var binder = attributes.FirstOrDefault((item) => item.Inherits<NativeTypeBinder>());
                    if (binder != null)
                        return $"{binder.AttributeClass!.Name}*";

                    return "void*";
                }
                case "System.Boolean":
                    // TODO: Check this has the attribute [MarshalAs(UnmanageType.I1)]
                    return "cbbool";
                case "System.Char":
                    return "cbchar_t";
                case "System.Byte":
                    return "uint8_t";
                case "System.SByte":
                    return "int8_t";
                case "System.Int16":
                    return "int16_t";
                case "System.UInt16":
                    return "uint16_t";
                case "System.Int32":
                    // TODO: Add CodeBinder.Attributes attribute to specify explicitly sized 32 bit signed integer
                    return "int";
                case "System.UInt32":
                    return "uint32_t";
                case "System.Int64":
                    return "int64_t";
                case "System.UInt64":
                    return "uint64_t";
                case "System.Single":
                    return "float";
                case "System.Double":
                    return "double";
                default:
                    throw new Exception($"Unsupported by type {typeName}");
            }
        }

        static string getCLangPointerType(string typeName, IEnumerable<AttributeData> attributes)
        {
            switch (typeName)
            {
                case "System.Runtime.InteropServices.HandleRef":
                case "System.IntPtr":
                {
                    var binder = attributes.FirstOrDefault((item) => item.Inherits<NativeTypeBinder>());
                    if (binder != null)
                        return $"{binder.AttributeClass!.Name}**";

                    return "void**";
                }
                case "System.Boolean":
                    // TODO: Check this has the attribute [MarshalAs(UnmanageType.I1)]
                    return "cbbool*";
                case "System.Char":
                    return "cbchar_t*";
                case "System.Byte":
                    return "uint8_t*";
                case "System.SByte":
                    return "int8_t*";
                case "System.Int16":
                    return "int16_t*";
                case "System.UInt16":
                    return "uint16_t*";
                case "System.Int32":
                    // TODO: Add CodeBinder.Attributes attribute to specify explicitly sized 32 bit signed integer
                    return "int*";
                case "System.UInt32":
                    return "uint32_t*";
                case "System.Int64":
                    return "int64_t*";
                case "System.UInt64":
                    return "uint64_t*";
                case "System.Single":
                    return "float*";
                case "System.Double":
                    return "double*";
                default:
                    throw new Exception($"Unsupported by type {typeName}");
            }
        }
    }
}

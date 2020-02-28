﻿using CodeBinder.Util;
using CodeBinder.Shared;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using CodeBinder.Shared.CSharp;
using CodeBinder.Attributes;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeBinder.Apple
{
    class ObjCTypesHeaderConversion : ObjCHeaderConversionWriter
    {
        public bool IsInternalHeader { get; private set; }

        public ObjCTypesHeaderConversion(ObjCCompilationContext compilation, bool isInternalHeader)
            : base(compilation)
        {
            IsInternalHeader = isInternalHeader;
        }

        protected override string GetFileName() => ConversionCSharpToObjC.TypesHeader;

        protected override string? GetBasePath() => IsInternalHeader ? ConversionCSharpToObjC.InternalBasePath : null;

        protected override string? GetGeneratedPreamble() => ConversionCSharpToObjC.SourcePreamble;

        protected override void write(CodeBuilder builder)
        {
            BeginHeaderGuard(builder);
            builder.AppendLine();
            if (IsInternalHeader)
                builder.AppendLine($"#import \"../{ConversionCSharpToObjC.TypesHeader}\"");
            else
                builder.AppendLine($"#import \"{BaseTypesHeader}\"");
            builder.AppendLine();
            writeOpaqueTypes(builder);
            builder.AppendLine();
            writeEnums(builder);
            builder.AppendLine();
            writeCallbacks(builder);
            builder.AppendLine();
            EndHeaderGuard(builder);
        }

        void writeOpaqueTypes(CodeBuilder builder)
        {
            // Forward declared classes
            builder.AppendLine("// Forward declared classes");
            builder.AppendLine();
            foreach (var type in Compilation.Classes)
            {
                if (ShouldSkipType(type))
                    continue;

                builder.Append("@class").Space().Append(type.GetObjCName(Compilation)).EndOfStatement();
            }

            builder.AppendLine();

            // Forward declared interfaces
            builder.AppendLine("// Forward declared interfaces");
            builder.AppendLine();
            foreach (var type in Compilation.Interfaces)
            {
                if (ShouldSkipType(type))
                    continue;

                builder.Append("@protocol").Space().Append(type.GetObjCName(Compilation)).EndOfStatement();
            }
        }

        private void writeEnums(CodeBuilder builder)
        {
            // Enums
            builder.AppendLine("// Enums");
            builder.AppendLine();
            var tmpbuilder = new StringBuilder();
            foreach (var enm in Compilation.Enums)
            {
                if (ShouldSkipType(enm))
                    continue;

                string enumName = enm.GetObjCName(Compilation);
                var symbol = enm.GetDeclaredSymbol<INamedTypeSymbol>(Compilation);
                bool isflag = symbol.HasAttribute<FlagsAttribute>();
                string underlyingType = ObjCUtils.GetSimpleType(symbol.EnumUnderlyingType!.GetFullName());
                builder.Append("typedef").Space().Append(isflag ? "NS_OPTIONS" : "NS_ENUM").Parenthesized()
                    .Append(underlyingType).CommaSeparator().Append(enumName).Close().AppendLine();

                using (builder.EnumBlock())
                {
                    foreach (var item in enm.Members)
                    {
                        long value = item.GetEnumValue(Compilation);
                        builder.Append(item.GetObjCName(Compilation)).Space().Append("=").Space().Append(value.ToString()).Comma().AppendLine();
                    }
                }

                builder.AppendLine();
            }
        }

        private void writeCallbacks(CodeBuilder builder)
        {
            // Callbacks
            builder.AppendLine("// Callbacks");
            builder.AppendLine();
            foreach (var callback in Compilation.Callbacks)
            {
                string name;
                AttributeData? bidingAttrib;
                if (callback.TryGetAttribute<NativeBindingAttribute>(Compilation, out bidingAttrib))
                    name = bidingAttrib.GetConstructorArgument<string>(0);
                else
                    name = callback.Identifier.Text;

                throw new NotImplementedException();
                /* TODO
                builder.Append("typedef").Space().Append(callback.GetCLangReturnType(Compilation))
                    .Append("(*").Append(name).Append(")")
                    .Append("(").Append(new CLangParameterListWriter(callback.ParameterList, Compilation)).Append(")")
                    .EndOfLine();
                */
            }
        }

        string getCLangSplittedIdentifier(StringBuilder builder, string prefix, string[] splitted)
        {
            builder.Clear();
            if (prefix != null)
            {
                builder.Append(prefix);
                builder.Append("_");
            }

            bool first = true;
            foreach (var str in splitted)
            {
                if (first)
                    first = false;
                else
                    builder.Append("_");
                builder.Append(str.ToUpperInvariant());
            }

            return builder.ToString();
        }

        bool ShouldSkipType(BaseTypeDeclarationSyntax type)
        {
            bool publicType = type.HasAccessibility(Accessibility.Public, Compilation);
            if (IsInternalHeader)
            {
                if (publicType)
                    return true;
            }
            else
            {
                if (!publicType)
                    return true;
            }

            return false;
        }

        protected override string HeaderGuardStem => IsInternalHeader ? "INTERNAL_TYPES" : "TYPES";

        public string BaseTypesHeader => IsInternalHeader
            ? ConversionCSharpToObjC.BaseTypesHeader
            : $"{ConversionCSharpToObjC.InternalBasePath}/{ConversionCSharpToObjC.BaseTypesHeader}";
    }
}

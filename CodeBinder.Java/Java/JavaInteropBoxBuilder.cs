﻿using CodeBinder.Shared;
using CodeBinder.Java.Shared;
using CodeBinder.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeBinder.Java
{
    class InteropBoxBuilder : JavaConversionBuilder
    {
        JavaInteropType _primitiveType;

        public InteropBoxBuilder(JavaInteropType primitiveType, ConversionCSharpToJava conversion)
            : base(conversion)
        {
            _primitiveType = primitiveType;
        }

        public override string FileName
        {
            get { return BoxTypeName + ".java"; }
        }

        protected override string GetBasePath()
        {
            return ConversionCSharpToJava.CodeBinderNamespace;
        }

        public override void write(CodeBuilder builder)
        {
            builder.Append("package").Space().Append(ConversionCSharpToJava.CodeBinderNamespace).EndOfStatement();
            builder.AppendLine();
            builder.Append("public class").Space().Append(BoxTypeName).AppendLine();
            using (builder.Block())
            {
                // Field
                builder.Append("public").Space().Append(JavaType).Space().Append("value").EndOfStatement();
                builder.AppendLine();

                // Default constructor
                builder.Append("public").Space().Append(BoxTypeName).EmptyParameterList().Space().AppendLine("{ }");
                builder.AppendLine();

                // Constructor with parameter
                builder.Append("public").Space().Append(BoxTypeName).Parenthesized()
                    .Append(JavaType).Space().Append("value").Close().AppendLine();
                using (builder.Block())
                {
                    builder.Append("this.value = value").EndOfStatement();
                }
            }
        }

        private string JavaType
        {
            get { return JavaUtils.ToJavaType(_primitiveType); }
        }

        private string BoxTypeName
        {
            get { return _primitiveType + "Box"; }
        }

        protected override string GetGeneratedPreamble() => ConversionCSharpToJava.SourcePreamble;
    }
}

﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using CodeTranslator.Shared.CSharp;
using CodeTranslator.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeTranslator.Java
{
    abstract class JavaTypeConversion<TTypeContext> : CSharpTypeConversion<TTypeContext>
        where TTypeContext : CSharpTypeContext
    {
        string _Namespace;
        string _Basepath;

        public string Namespace
        {
            get { return _Namespace; }
            set
            {
                _Namespace = value;
                _Basepath = value.Replace('.', Path.DirectorySeparatorChar);
            }
        }

        public override string BasePath
        {
            get { return _Basepath; }
        }

        public override string FileName
        {
            get { return TypeName + ".java"; }
        }

        public virtual string TypeName
        {
            get { return TypeContext.Node.GetName(); }
        }

        public override string GeneratedPreamble
        {
            get { return "/* This file was generated. DO NOT EDIT! */"; }
        }

        public virtual IEnumerable<string> Imports
        {
            get { yield break; }
        }

        public sealed override void Write(IndentStringBuilder builder)
        {
            builder.Append("package ");
            builder.Append(Namespace);
            builder.AppendLine(";");
            builder.AppendLine();
            bool hasImports = false;
            foreach (var import in Imports)
            {
                builder.AppendLine();
                hasImports = true;
            }

            if (hasImports)
                builder.AppendLine();

            WriteDeclaration(builder);
        }

        public abstract void WriteDeclaration(IndentStringBuilder builder);
    }
}

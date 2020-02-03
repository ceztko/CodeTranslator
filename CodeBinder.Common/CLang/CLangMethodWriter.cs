﻿using CodeBinder.Shared;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.CLang
{
    abstract class CLangMethodWriter : CodeWriter<MethodDeclarationSyntax, CLangModuleConversion>
    {
        protected CLangMethodWriter(MethodDeclarationSyntax method, CLangModuleConversion context)
            : base(method, context) { }

        public static CLangMethodWriter Create(MethodDeclarationSyntax method, bool widechar,
            CLangModuleConversion context)
        {
            return new SyntaxSignatureMethodWriter(method, widechar, context);
        }

        protected override void Write()
        {
            Builder.Append(Context.Compilation.LibraryName.ToUpper()).Append("_SHARED_API").Space();
            Builder.Append(ReturnType).Space();
            Builder.Append(MethodName).AppendLine("(");
            using (Builder.Indent())
            {
                WriteParameters();
                Builder.Append(")").EndOfLine();
            }
        }

        public abstract string MethodName
        {
            get;
        }

        public abstract string ReturnType
        {
            get;
        }

        protected abstract void WriteParameters();

        class SyntaxSignatureMethodWriter : CLangMethodWriter
        {
            public bool WideChar { get; private set; }

            public SyntaxSignatureMethodWriter(MethodDeclarationSyntax method, bool widechar, CLangModuleConversion context)
                : base(method, context)
            {
                WideChar = widechar;
            }

            protected override void WriteParameters()
            {
                Builder.Append(new CLangParameterListWriter(Item.ParameterList, Context));
            }

            public override string ReturnType
            {
                get { return Item.GetCLangReturnType(Context); }
            }

            public override string MethodName
            {
                get { return Item.GetCLangMethodName(WideChar); }
            }
        }
    }

    class CLangParameterListWriter : CodeWriter<ParameterListSyntax, ICompilationContextProvider>
    {
        public CLangParameterListWriter(ParameterListSyntax list, ICompilationContextProvider module)
            : base(list, module) { }

        protected override void Write()
        {
            bool first = true;
            foreach (var parameter in Item.Parameters)
            {
                if (first)
                    first = false;
                else
                    Builder.CommaSeparator();

                Builder.Append(parameter, Context);
            }
        }
    }
}

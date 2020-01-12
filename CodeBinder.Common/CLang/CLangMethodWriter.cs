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
        protected CLangMethodWriter(MethodDeclarationSyntax method, CLangModuleConversion module)
            : base(method, module, module) { }

        public static CLangMethodWriter Create(MethodDeclarationSyntax method, bool widechar,
            CLangModuleConversion module)
        {
            return new SyntaxSignatureMethodWriter(method, widechar, module);
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

            public SyntaxSignatureMethodWriter(MethodDeclarationSyntax method, bool widechar, CLangModuleConversion module)
                : base(method, module)
            {
                WideChar = widechar;
            }

            protected override void WriteParameters()
            {
                Builder.Append(new CLangParameterListWriter(Item.ParameterList, Context));
            }

            public override string ReturnType
            {
                get { return Item.ReturnType.GetCLangType(false, this); }
            }

            public override string MethodName
            {
                get { return Item.GetCLangMethodName(WideChar, Context.Context); }
            }
        }
    }

    class CLangParameterListWriter : CodeWriter<ParameterListSyntax, ICompilationContextProvider>
    {
        public CLangParameterListWriter(ParameterListSyntax list, ICompilationContextProvider module)
            : base(list, module, module) { }

        protected override void Write()
        {
            foreach (var parameter in Item.Parameters)
                WriteParameter(parameter);
        }

        private void WriteParameter(ParameterSyntax parameter)
        {
            Builder.CommaSeparator();
            bool isRef = parameter.IsRef() || parameter.IsOut();
            WriteType(parameter.Type, isRef);
            Builder.Append(parameter.Identifier.Text);
        }

        private void WriteType(TypeSyntax type, bool isRef)
        {
            Builder.Append(type.GetCLangType(isRef, this)).Space();
        }
    }
}

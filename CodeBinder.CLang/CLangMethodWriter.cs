﻿// Copyright(c) 2020 Francesco Pretto
// This file is subject to the MIT license
using CodeBinder.Shared;
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
        public bool CppMethod { get; private set; }

        protected CLangMethodWriter(MethodDeclarationSyntax method, bool cppMethod, CLangModuleConversion context)
            : base(method, context)
        {
            CppMethod = cppMethod;
        }

        protected override void Write()
        {
            if (CppMethod)
                Builder.Append("inline");
            else
                Builder.Append(Context.Compilation.LibraryName.ToUpper()).Append("_SHARED_API");

            Builder.Space().Append(ReturnType).Space();
            Builder.Append(MethodName).AppendLine("(");
            using (Builder.Indent())
            {
                WriteParameters();
                Builder.Append(")");
            }

            if (HasBody)
            {
                Builder.AppendLine();
                using (Builder.Block())
                    WriteBody();
            }
            else
            {
                Builder.EndOfLine();
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
        public abstract bool HasBody
        {
            get;
        }

        protected virtual void WriteBody()
        {
            /* Do Nothing */
        }

        protected abstract void WriteParameters();
    }

    class CLangMethodDeclarationWriter : CLangMethodWriter
    {
        public CLangMethodDeclarationWriter(MethodDeclarationSyntax method, bool cppMethod, CLangModuleConversion context)
            : base(method, cppMethod, context) { }

        protected override void WriteParameters()
        {
            Builder.Append(new CLangParameterListWriter(Item.ParameterList, CppMethod, Context));
        }

        public override string ReturnType
        {
            get { return Item.GetCLangReturnType(CppMethod, Context); }
        }

        public override string MethodName
        {
            get { return Item.GetCLangMethodName(); }
        }

        public override bool HasBody => false;
    }

    class CLangMethodTrampolineWriter : CLangMethodWriter
    {
        public CLangMethodTrampolineWriter(MethodDeclarationSyntax method, CLangModuleConversion context)
            : base(method, false, context) { }

        protected override void WriteParameters()
        {
            Builder.Append(new CLangParameterListWriter(Item.ParameterList, false, Context));
        }

        public override string ReturnType
        {
            get { return Item.GetCLangReturnType(false, Context); }
        }

        public override string MethodName
        {
            get { return Item.GetCLangMethodName(); }
        }

        protected override void WriteBody()
        {
            if (Item.ReturnType.GetTypeSymbol(Context).SpecialType != SpecialType.System_Void)
                Builder.Append("return").Space();

            Builder.Append(Context.Compilation.LibraryName.ToLower()).Append("::")
                .Append(Item.GetCLangMethodName());

            using (Builder.ParameterList())
            {
                bool first = true;
                foreach (var param in Item.ParameterList.Parameters)
                {
                    Builder.CommaSeparator(ref first);
                    var symbol = param.GetDeclaredSymbol<IParameterSymbol>(Context);
                    if (symbol.Type.GetFullName() == "CodeBinder.cbstring")
                    {
                        if (symbol.RefKind == RefKind.None)
                            Builder.Append("std::move(").Append(param.Identifier.Text).Append(")");
                        else
                            Builder.Append("cbstringpr(").Append(param.Identifier.Text).Append(")");
                    }
                    else
                    {
                        Builder.Append(param.Identifier.Text);
                    }
                }
            }
            Builder.EndOfLine();
        }

        public override bool HasBody => true;
    }

    class CLangParameterListWriter : CodeWriter<ParameterListSyntax, ICompilationContextProvider>
    {
        public bool CppMethod { get; private set; }

        public CLangParameterListWriter(ParameterListSyntax list, bool cppMethod,
            ICompilationContextProvider module)
            : base(list, module)
        {
            CppMethod = cppMethod;
        }

        protected override void Write()
        {
            bool first = true;
            foreach (var parameter in Item.Parameters)
            {
                if (first)
                    first = false;
                else
                    Builder.CommaSeparator();

                Builder.Append(parameter, CppMethod, Context);
            }
        }
    }
}

﻿using CodeBinder.Shared;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.Java
{
    class InterfaceTypeWriter : TypeWriter<InterfaceDeclarationSyntax>
    {
        public InterfaceTypeWriter(InterfaceDeclarationSyntax declaration, PartialDeclarationsTree partialDeclarations,
                ICompilationContextProvider context) : base(declaration, partialDeclarations, context) { }

        protected override void WriteTypeParameters()
        {
            Builder.Append(Context.GetTypeParameters(), this).Space();
        }

        public override int Arity
        {
            get { return Context.Arity; }
        }
    }
}
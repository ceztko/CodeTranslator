﻿using CodeTranslator.Shared;
using CodeTranslator.Shared.CSharp;
using CodeTranslator.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Java
{
    class JavaInterfaceConversion : JavaTypeConversion<CSharpInterfaceTypeContext>
    {
        public JavaInterfaceConversion(CSToJavaConversion conversion)
            : base(conversion) { }

        protected override IContextWriter GetTypeWriter()
        {
            return new InterfaceTypeWriter(TypeContext.Node, this);
        }
    }

    class InterfaceTypeWriter : TypeWriter<InterfaceDeclarationSyntax>
    {
        public InterfaceTypeWriter(InterfaceDeclarationSyntax syntax, ICompilationContextProvider context)
            : base(syntax, context) { }

        public override bool IsInterface
        {
            get { return true; }
        }

        protected override void WriteTypeMembers()
        {
            WriteTypeMembers(Context.Members);
        }
    }
}

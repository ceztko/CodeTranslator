﻿using CodeTranslator.Shared;
using CodeTranslator.Shared.CSharp;
using CodeTranslator.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Java
{
    class JavaStructConversion : JavaTypeConversion<CSharpStructTypeContext>
    {
        protected override TypeWriter GetTypeWriter()
        {
            return new StructTypeWriter(TypeContext.Node, this);
        }
    }

    class StructTypeWriter : TypeWriter<StructDeclarationSyntax>
    {
        public StructTypeWriter(StructDeclarationSyntax node, ISemanticModelProvider context)
            : base(node, context) { }

        protected override void WriteTypeMembers()
        {
            WriteTypeMembers(Type.Members);
        }
    }
}
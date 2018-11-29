﻿using CodeBinder.Shared;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.Java
{
    class StructTypeWriter : TypeWriter<StructDeclarationSyntax>
    {
        public StructTypeWriter(StructDeclarationSyntax declaration, PartialDeclarationsTree partialDeclarations,
                JavaCodeConversionContext context) : base(declaration, partialDeclarations, context) { }

        protected override void WriteTypeParameters()
        {
            Builder.Append(Item.GetTypeParameters(), Context).Space();
        }

        public override int Arity
        {
            get { return Item.Arity; }
        }

        public override bool NeedStaticKeyword
        {
            get { return true; }
        }
    }
}

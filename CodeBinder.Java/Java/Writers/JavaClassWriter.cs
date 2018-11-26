﻿using CodeBinder.Shared.CSharp;
using CodeBinder.Shared;
using CodeBinder.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.Java
{
    class ClassTypeWriter : TypeWriter<ClassDeclarationSyntax>
    {
        public ClassTypeWriter(ClassDeclarationSyntax declaration, PartialDeclarationsTree partialDeclarations,
                JavaCodeWriterContext context) : base(declaration, partialDeclarations, context) { }

        protected override void WriteTypeParameters()
        {
            Builder.Append(Item.GetTypeParameters(), this).Space();
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

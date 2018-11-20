﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeTranslator.Shared.CSharp
{
    public sealed class CSharpSyntaxTreeContext : SyntaxTreeContext<CSharpBaseTypeContext, CSharpLanguageConversion>
    {
        public CSharpSyntaxTreeContext(CSharpLanguageConversion conversion)
            : base(conversion) { }

        public override void Visit(SyntaxTree tree)
        {
            var walker = new CSharpNodeVisitor(this, Conversion);
            walker.Visit(tree.GetRoot());
        }

        public new void AddType(CSharpBaseTypeContext type, CSharpBaseTypeContext parent)
        {
            base.AddType(type, parent);
        }
    }
}

﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Shared.CSharp
{
    public abstract class CSharpTypeContext : TypeContext<CSharpTypeContext, CSharpSyntaxTreeContext>
    {
        internal CSharpTypeContext(CSharpSyntaxTreeContext treeContext)
            : base(treeContext) { }

        public BaseTypeDeclarationSyntax Node
        {
            get { return GetNode(); }
        }

        protected abstract BaseTypeDeclarationSyntax GetNode();
    }

    public abstract class CSharpTypeContext<TNode, TTypeConversion> : CSharpTypeContext
        where TNode : BaseTypeDeclarationSyntax
        where TTypeConversion : TypeConversion
    {
        public new TTypeConversion Conversion { get; private set; }

        public new TNode Node { get; private set; }

        protected CSharpTypeContext(TNode node, CSharpSyntaxTreeContext treeContext, TTypeConversion conversion)
            : base(treeContext)
        {
            Node = node;
            Conversion = conversion;
        }

        protected override BaseTypeDeclarationSyntax GetNode()
        {
            return Node;
        }

        protected override TypeConversion GetConversion()
        {
            return Conversion;
        }
    }
}

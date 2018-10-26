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
    public sealed class CSharpEnumTypeContext : CSharpTypeContext<EnumDeclarationSyntax>
    {
        public CSharpEnumTypeContext(EnumDeclarationSyntax node, CSharpSyntaxTreeContext treeContext)
            : base(node, treeContext) { }
    }
}

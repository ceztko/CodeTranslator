﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.Shared.CSharp
{
    public sealed class CSharpEnumTypeContext : CSharpBaseTypeContext<EnumDeclarationSyntax, TypeConversion<CSharpEnumTypeContext>>
    {
        public CSharpEnumTypeContext(EnumDeclarationSyntax node,
                CSharpCompilationContext compilation,
                TypeConversion<CSharpEnumTypeContext> conversion)
            : base(node, compilation, conversion)
        {
            conversion.TypeContext = this;
        }
    }
}

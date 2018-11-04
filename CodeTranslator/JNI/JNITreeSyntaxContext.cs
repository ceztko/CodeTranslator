﻿using CodeTranslator.Shared;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.JNI
{
    public class JNISyntaxTreeContext : SyntaxTreeContext<JNIModuleContext, CSToJNIConversion>
    {
        public JNISyntaxTreeContext(CompilationContext compilation, CSToJNIConversion conversion)
            : base(compilation, conversion)  { }

        public override void Visit(SyntaxTree tree)
        {
            var walker = new JNINodeVisitor(this, Conversion);
            walker.Visit(tree.GetRoot());
        }
    }
}
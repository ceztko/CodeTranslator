﻿using CodeBinder.Shared;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.JNI
{
    public class JNISyntaxTreeContext : JNICompilationContext.SyntaxTree<JNICompilationContext>
    {
        public JNISyntaxTreeContext(JNICompilationContext compilation)
            : base(compilation)  { }
    }
}

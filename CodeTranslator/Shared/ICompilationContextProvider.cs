﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Shared
{
    public interface ICompilationContextProvider
    {
        CompilationContext Compilation { get; }
    }
}

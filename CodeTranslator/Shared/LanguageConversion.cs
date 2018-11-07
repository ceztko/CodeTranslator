﻿// Copyright (c) 2017-2018 ICSharpCode
// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using System;
using System.Collections.Generic;
using System.Text;
using CodeTranslator.Util;
using Microsoft.CodeAnalysis;

namespace CodeTranslator.Shared
{
    public abstract class LanguageConversion
    {
        internal LanguageConversion() { }

        public abstract SyntaxTreeContext GetSyntaxTreeContext(CompilationContext compilation);

        public virtual string GetWarningsOrNull(CompilationContext compilation)
        {
            return CompilationWarnings.WarningsForCompilation(compilation, "source");
        }

        public virtual IEnumerable<ConversionBuilder> DefaultConversions
        {
            get { yield break; }
        }

        internal IEnumerable<ConversionDelegate> DefaultConversionDelegates
        {
            get
            {
                foreach (var conversion in DefaultConversions)
                    yield return new ConversionDelegate(conversion);
            }
        }
    }

    public abstract class LanguageConversion<TSyntaxTreeContext, TTypeContext> : LanguageConversion
        where TSyntaxTreeContext : SyntaxTreeContext<TTypeContext>
        where TTypeContext : TypeContext<TTypeContext, TSyntaxTreeContext>
    {
        public sealed override SyntaxTreeContext GetSyntaxTreeContext(CompilationContext compilation)
        {
            return getSyntaxTreeContext(compilation);
        }

        protected abstract TSyntaxTreeContext getSyntaxTreeContext(CompilationContext compilation);
    }
}

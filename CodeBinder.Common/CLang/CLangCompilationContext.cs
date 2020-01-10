﻿using CodeBinder.Shared;
using CodeBinder.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.CLang
{
    public class CLangCompilationContext : CompilationContext<CLangSyntaxTreeContext, CLangModuleContext, ConversionCSharpToCLang>
    {
        Dictionary<string, CLangModuleContextParent> _modules;
        List<EnumDeclarationSyntax> _enums;
        List<ClassDeclarationSyntax> _types;

        public CLangCompilationContext(ConversionCSharpToCLang conversion)
            : base(conversion)
        {
            _modules = new Dictionary<string, CLangModuleContextParent>();
            _enums = new List<EnumDeclarationSyntax>();
            _types = new List<ClassDeclarationSyntax>();
        }

        public void AddModule(CompilationContext compilation, CLangModuleContextParent module)
        {
            _modules.Add(module.Name, module);
            AddType(module, null);
        }

        public void AddModuleChild(CompilationContext compilation, CLangModuleContextChild module, CLangModuleContextParent parent)
        {
            AddType(module, parent);
        }

        public bool TryGetModule(string moduleName, out CLangModuleContextParent module)
        {
            return _modules.TryGetValue(moduleName, out module);
        }

        public void AddEnum(EnumDeclarationSyntax enm)
        {
            _enums.Add(enm);
        }

        public void AddType(ClassDeclarationSyntax type)
        {
            _types.Add(type);
        }

        protected override CLangSyntaxTreeContext createSyntaxTreeContext()
        {
            return new CLangSyntaxTreeContext(this);
        }

        public IEnumerable<CLangModuleContextParent> Modules
        {
            get { return _modules.Values; }
        }

        public IEnumerable<EnumDeclarationSyntax> Enums
        {
            get { return _enums; }
        }

        public IEnumerable<ClassDeclarationSyntax> Types
        {
            get { return _types; }
        }

        public override IEnumerable<ConversionBuilder> DefaultConversions
        {
            get
            {
                yield return new CLangLibsDefsHeaderBuilder(this);
                yield return new CLangTypesHeaderBuilder(this);
                yield return new CLangMethodInitBuilder(this);
            }
        }
    }

    abstract class CLangCompilationContextBuilder : ConversionBuilder
    {
        public CLangCompilationContext Compilation { get; private set; }

        public CLangCompilationContextBuilder(CLangCompilationContext compilation)
        {
            Compilation = compilation;
        }
    }
}

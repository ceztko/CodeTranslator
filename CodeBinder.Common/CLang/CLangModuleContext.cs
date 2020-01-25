﻿using CodeBinder.Shared;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.CLang
{
    public abstract class CLangModuleContext : TypeContext<CLangModuleContext, CLangCompilationContext>
    {
        protected CLangModuleContext(CLangCompilationContext context)
            : base(context) { }

        public abstract string Name
        {
            get;
        }

        public abstract IEnumerable<MethodDeclarationSyntax> Methods
        {
            get;
        }
    }

    public class CLangModuleContextParent : CLangModuleContext
    {
        private string _Name;

        public CLangModuleContextParent(string name, CLangCompilationContext context)
            : base(context)
        {
            _Name = name;
        }

        protected override TypeConversion GetConversion()
        {
            var ret = new CLangModuleConversion(Compilation.Conversion);
            ret.Context = this;
            return ret;
        }

        public override IEnumerable<MethodDeclarationSyntax> Methods
        {
            get
            {
                foreach (var child in Children)
                {
                    foreach (var method in child.Methods)
                        yield return method;
                }
            }
        }

        public override string Name
        {
            get { return _Name; }
        }
    }

    public class CLangModuleContextChild : CLangModuleContext
    {
        private List<MethodDeclarationSyntax> _methods;

        public CLangModuleContextChild(CLangCompilationContext context)
            : base(context)
        {
            _methods = new List<MethodDeclarationSyntax>();
        }

        public void AddNativeMethod(MethodDeclarationSyntax method)
        {
            _methods.Add(method);
        }

        protected override TypeConversion GetConversion()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<MethodDeclarationSyntax> Methods
        {
            get { return _methods; }
        }

        public override string Name
        {
            get { return Parent!.Name; }
        }
    }
}
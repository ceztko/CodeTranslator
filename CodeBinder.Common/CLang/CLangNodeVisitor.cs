﻿using CodeBinder.Shared.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using CodeBinder.Shared;
using CodeBinder.Attributes;
using Microsoft.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CodeBinder.CLang
{
    class CLangNodeVisitor : CSharpNodeVisitor<CLangSyntaxTreeContext, CLangCompilationContext, ConversionCSharpToCLang>
    {
        public CLangNodeVisitor(CLangSyntaxTreeContext treeContext)
            : base(treeContext, treeContext.Compilation, treeContext.Compilation.Conversion) { }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var sym = node.GetDeclaredSymbol<ITypeSymbol>(this);
            if (sym.Inherits<NativeTypeBinder>())
            {
                // These are the binders for types
                Compilation.AddType(node);
            }

            visitType(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            visitType(node);
        }

        private void visitType(TypeDeclarationSyntax type)
        {
            CLangModuleContextChild module = null;
            string moduleName;
            if (TryGetModuleName(type, out moduleName))
            {
                CLangModuleContextParent parent;
                if (!Compilation.TryGetModule(moduleName, out parent))
                {
                    parent = new CLangModuleContextParent(moduleName, Compilation);
                    Compilation.AddModule(Compilation, parent);
                }

                module = new CLangModuleContextChild(Compilation);
                Compilation.AddModuleChild(Compilation, module, parent);
            }

            foreach (var member in type.Members)
            {
                var kind = member.Kind();
                switch (kind)
                {
                    case SyntaxKind.MethodDeclaration:
                        if (module != null && !member.ShouldDiscard(this))
                        {
                            var method = member as MethodDeclarationSyntax;
                            if (method.IsNative(this))
                                module.AddNativeMethod(member as MethodDeclarationSyntax);
                        }
                        break;
                    case SyntaxKind.ClassDeclaration:
                        visitType(member as ClassDeclarationSyntax);
                        break;
                    case SyntaxKind.StructDeclaration:
                        visitType(member as StructDeclarationSyntax);
                        break;
                    case SyntaxKind.EnumDeclaration:
                        visitType(member as StructDeclarationSyntax);
                        break;
                    case SyntaxKind.DelegateDeclaration:
                        visitType(member as DelegateDeclarationSyntax);
                        break;
                }
            }
        }

        public void visitType(DelegateDeclarationSyntax node)
        {
            var attributes = node.GetAttributes(this);
            if (!attributes.HasAttribute<UnmanagedFunctionPointerAttribute>() || attributes.HasAttribute<NativeIgnoreAttribute>())
                return;

            Compilation.AddCallback(node);
        }

        bool TryGetModuleName(TypeDeclarationSyntax type, out string moduleName)
        {
            var attributes = type.GetAttributes(this);
            foreach (var attribute in attributes)
            {
                if (attribute.IsAttribute<ModuleAttribute>())
                {
                    moduleName = attribute.GetConstructorArgument<string>(0);
                    return true;
                }
            }

            moduleName = null;
            return false;
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (!node.GetAttributes(this).HasAttribute<NativeBindingAttribute>())
                return;

            Compilation.AddEnum(node);
        }
    }
}

﻿using CodeTranslator.Shared;
using CodeTranslator.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeTranslator.Shared.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Java
{
    abstract class MethodWriter<TMethod> : CodeWriter<TMethod>
        where TMethod : BaseMethodDeclarationSyntax
    {
        protected MethodWriter(TMethod method, ICompilationContextProvider context)
            : base(method, context) { }

        protected override void Write()
        {
            WriteModifiers();
            if (Arity != 0)
                WriteTypeParameters();

            WriteReturnType();
            Builder.Append(MethodName);
            WriteParameters();
            WriteMethodBody();
        }

        protected virtual void WriteTypeParameters() { }

        protected virtual void WriteParameters()
        {
            if (Context.ParameterList.Parameters.Count == 0)
            {
                Builder.Append("()");
            }
            else if (Context.ParameterList.Parameters.Count == 1)
            {
                using (Builder.ParameterList())
                {
                    WriteParameters(Context.ParameterList);
                }
            }
            else
            {
                using (Builder.Indent())
                {
                    using (Builder.ParameterList(true))
                    {
                        WriteParameters(Context.ParameterList);
                        Builder.AppendLine();
                    }
                }
            }
        }

        protected virtual void WriteMethodBody()
        {
            if (Context.Body == null)
                Builder.EndOfStatement();
            else
                Builder.AppendLine().Append(Context.Body, this).AppendLine();
        }

        protected virtual void WriteModifiers()
        {
            string modifiers = Context.GetJavaModifiersString();
            if (!string.IsNullOrEmpty(modifiers))
                Builder.Append(modifiers).Space();
        }

        private void WriteParameters(ParameterListSyntax list)
        {
            bool first = true;
            foreach (var parameter in list.Parameters)
            {
                if (first)
                    first = false;
                else
                    Builder.AppendLine(",");

                WriteParameter(parameter);
            }
        }

        private void WriteParameter(ParameterSyntax parameter)
        {
            var flags = IsNative ? JavaTypeFlags.NativeMethod : JavaTypeFlags.None;
            bool isRef = parameter.IsRef() | parameter.IsOut();
            if (isRef)
                flags |= JavaTypeFlags.IsByRef;

            WriteType(parameter.Type, flags);
            Builder.Space().Append(parameter.Identifier.Text);
        }

        protected void WriteType(TypeSyntax type, JavaTypeFlags flags)
        {
            Builder.Append(type.GetJavaType(flags, this));
        }

        public virtual int Arity
        {
            get { return 0; }
        }

        protected virtual void WriteReturnType() { }

        public abstract string MethodName { get; }

        public abstract bool IsNative { get; }
    }

    class MethodWriter : MethodWriter<MethodDeclarationSyntax>
    {
        public MethodWriter(MethodDeclarationSyntax method, ICompilationContextProvider context)
            : base(method, context) { }

        protected override void WriteModifiers()
        {
            if (IsParentInterface)
                return;

            base.WriteModifiers();
        }

        protected override void WriteTypeParameters()
        {
            Builder.Append(Context.GetTypeParameters(), this);
        }

        protected override void WriteReturnType()
        {
            WriteType(Context.ReturnType, IsNative ? JavaTypeFlags.NativeMethod : JavaTypeFlags.None);
            Builder.Space();
        }

        protected override void WriteMethodBody()
        {
            if (IsParentInterface)
                Builder.EndOfStatement();
            else
                base.WriteMethodBody();
        }

        public bool IsParentInterface
        {
            get { return Context.Parent.Kind() == SyntaxKind.InterfaceDeclaration; }
        }

        public override string MethodName
        {
            get
            {
                var methodName = Context.GetName();
                if (IsNative)
                    return methodName;
                else
                    return methodName.ToJavaCase();
            }
        }

        public override bool IsNative
        {
            get { return Context.IsNative(this); }
        }

        public override int Arity
        {
            get { return Context.Arity; }
        }
    }

    class SignatureMethodWriter : MethodWriter<MethodDeclarationSyntax>
    {
        MethodSignatureInfo _signature;

        public SignatureMethodWriter(MethodSignatureInfo signature, MethodDeclarationSyntax method, ICompilationContextProvider context)
            : base(method, context)
        {
            _signature = signature;
        }

        protected override void WriteModifiers()
        {
            Builder.Append(_signature.GetJavaModifiersString()).Space();
        }

        protected override void WriteReturnType()
        {
            Builder.Append(_signature.ReturnType.GetJavaTypeName(JavaTypeFlags.NativeMethod)).Space();
        }

        protected override void WriteMethodBody()
        {
            Builder.EndOfStatement();
        }

        protected override void WriteParameters()
        {
            for (int i = 0; i < _signature.Parameters.Length; i++)
                WriteParameter(ref _signature.Parameters[i]);
        }

        private void WriteParameter(ref MethodParameterInfo parameter)
        {
            Builder.CommaSeparator().Append(parameter.GetJavaTypeName(JavaTypeFlags.NativeMethod)).Space().Append(parameter.Name);
        }

        public override string MethodName
        {
            get { return _signature.MethodName; }
        }

        public override bool IsNative
        {
            get { return true; } // TODO: Check method native
        }
    }

    class ConstructorWriter : MethodWriter<ConstructorDeclarationSyntax>
    {
        public ConstructorWriter(ConstructorDeclarationSyntax method, ICompilationContextProvider context)
            : base(method, context) { }

        public override string MethodName
        {
            get { return (Context.Parent as BaseTypeDeclarationSyntax).GetName();}
        }

        public override bool IsNative
        {
            get { return false; }
        }
    }

    class DestructorWriter : MethodWriter<DestructorDeclarationSyntax>
    {
        public DestructorWriter(DestructorDeclarationSyntax method, ICompilationContextProvider context)
            : base(method, context) { }

        protected override void WriteModifiers()
        {
            Builder.Append("protected").Space();
        }

        protected override void WriteReturnType()
        {
            Builder.Append("void").Space();
        }

        public override string MethodName
        {
            get { return "finalize"; }
        }

        public override bool IsNative
        {
            get { return false; }
        }
    }
}

﻿using CodeTranslator.Shared;
using CodeTranslator.Shared.CSharp;
using CodeTranslator.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CodeTranslator.Shared.Java;
using Microsoft.CodeAnalysis;

namespace CodeTranslator.Java
{
    static partial class JavaWriterExtension
    {
        public static CodeBuilder Append(this CodeBuilder builder, ConstructorInitializerSyntax syntax, ICompilationContextProvider context)
        {
            return builder;
        }

        public static void Append(this CodeBuilder builder,
            CSharpTypeParameters typeParameters, ICompilationContextProvider context)
        {
            using (builder.TypeParameterList(typeParameters.Count > 1))
            {
                foreach (var parameter in typeParameters)
                {
                    builder.Append(parameter.Type.Identifier.Text);
                    if (parameter.Constraints != null)
                    {
                        builder.Space();
                        writeTypeConstraints(builder, parameter.Constraints, context);
                    }

                    if (typeParameters.Count > 1)
                        builder.AppendLine();
                }
            }
        }

        static void writeTypeConstraints(CodeBuilder builder,
            TypeParameterConstraintClauseSyntax constraints,
            ICompilationContextProvider context)
        {
            bool first = true;
            foreach (var constraint in constraints.Constraints)
            {
                if (first)
                    first = false;
                else
                    builder.Space().Append("&").Space();

                builder.Append(constraint, context);
            }
        }

        public static CodeBuilder Append(this CodeBuilder builder,
            TypeParameterConstraintSyntax syntax, ICompilationContextProvider context)
        {
            switch (syntax.Kind())
            {
                case SyntaxKind.TypeConstraint:
                {
                    var typeContraints = syntax as TypeConstraintSyntax;
                    string javaTypeName = typeContraints.Type.GetJavaType(context, out var isInterface);

                    builder.Append(isInterface ? "implements" : "extends").Space().Append(javaTypeName);
                    break;
                }
                default:
                    throw new Exception("Unsupported type constraint");
            }

            return builder;
        }

        public static CodeBuilder EndOfStatement(this CodeBuilder builder)
        {
            return builder.AppendLine(";");
        }

        public static CodeBuilder SemiColonSeparator(this CodeBuilder builder)
        {
            return builder.Append("; ");
        }

        public static CodeBuilder Colon(this CodeBuilder builder)
        {
            return builder.Append(":");
        }

        public static CodeBuilder SemiColon(this CodeBuilder builder)
        {
            return builder.Append(";");
        }

        public static CodeBuilder QuestionMark(this CodeBuilder builder)
        {
            return builder.Append("?");
        }

        public static CodeBuilder CommaSeparator(this CodeBuilder builder)
        {
            return builder.Append(", ");
        }

        public static CodeBuilder Space(this CodeBuilder builder)
        {
            return builder.Append(" ");
        }

        /// <summary>One line parenthesized</summary>
        public static CodeBuilder Parenthesized(this CodeBuilder builder, Action parenthesized)
        {
            builder.Append("(");
            parenthesized();
            return builder.Append(")");
        }

        /// <summary>One line parenthesized</summary>
        public static CodeBuilder Parenthesized(this CodeBuilder builder)
        {
            builder.Append("(");
            return builder.UsingChild(")");
        }

        public static CodeBuilder Block(this CodeBuilder builder, bool appendLine = true)
        {
            builder.AppendLine("{");
            return builder.Indent("}", appendLine);
        }

        public static CodeBuilder ParameterList(this CodeBuilder builder, bool multiLine = false)
        {
            if (multiLine)
            {
                builder.AppendLine("(");
                return builder.Indent(")");
            }
            else
            {
                builder.Append("(");
                return builder.Using(")");
            }
        }

        public static CodeBuilder TypeParameterList(this CodeBuilder builder, bool multiLine = false)
        {
            if (multiLine)
            {
                builder.AppendLine("<");
                return builder.Indent(">", true);
            }
            else
            {
                builder.Append("<");
                return builder.Using("> ");
            }
        }
    }
}

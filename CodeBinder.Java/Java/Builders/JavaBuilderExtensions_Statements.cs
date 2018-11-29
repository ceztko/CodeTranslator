﻿using CodeBinder.Shared;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CodeBinder.Shared.Java;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace CodeBinder.Java
{
    static partial class JavaBuilderExtension
    {
        public static CodeBuilder Append(this CodeBuilder builder, BlockSyntax syntax, JavaCodeConversionContext context, bool skipBraces = false)
        {
            if (skipBraces)
            {
                builder.Append(syntax.Statements, context);
            }
            else
            {
                // Allows to not doubly indent single statements blocks, e.g after "if" statement
                builder.ResetChildIndent();
                using (builder.Block(false))
                {
                    builder.Append(syntax.Statements, context).AppendLine();
                }
            }

            return builder;
        }

        static CodeBuilder Append(this CodeBuilder builder, IEnumerable<StatementSyntax> staments, JavaCodeConversionContext context)
        {
            bool first = true;
            foreach (var statement in staments)
            {
                IEnumerable<CodeWriter> writers;
                if (statement.HasReplacementWriter(context, out writers))
                {
                    foreach (var writer in writers)
                        builder.AppendLine(ref first).Append(writer);
                }
                else
                {
                    builder.AppendLine(ref first).Append(statement, context);
                }
            }

            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, BreakStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("break").SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ForEachStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("for").Space().Parenthesized().Append(syntax.Type, context).Space().Append(syntax.Identifier.Text)
                .Space().Colon().Space().Append(syntax.Expression, context).Close().AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ContinueStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("continue").SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, DoStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("do").AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            builder.Append("while").Space().Parenthesized().Append(syntax.Condition, context).Close().SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, EmptyStatementSyntax syntax, JavaCodeConversionContext context)
        {
            return builder.SemiColon();
        }

        public static CodeBuilder Append(this CodeBuilder builder, ExpressionStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append(syntax.Expression, context).SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ForStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("for").Space().Parenthesized(() =>
            {
                builder.Append(syntax.Declaration, context).SemiColonSeparator()
                    .Append(syntax.Condition, context).SemiColonSeparator()
                    .Append(syntax.Incrementors, context);
            }).AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, IfStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("if").Space().Parenthesized().Append(syntax.Condition, context).Close().AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            if (syntax.Else != null)
                builder.AppendLine().Append(syntax.Else, context);

            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, LocalDeclarationStatementSyntax syntax, JavaCodeConversionContext context)
        {
            if (syntax.IsConst)
                builder.Append("final").Space();

            builder.Append(syntax.Declaration, context).SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, LockStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("synchronized").Space().Parenthesized().Append(syntax.Expression, context).Close().AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ReturnStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("return");
            if (syntax.Expression != null)
                builder.Space().Append(syntax.Expression, context);
            builder.SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, SwitchStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("switch").Space().Parenthesized().Append(syntax.Expression, context).Close().AppendLine();
            using (builder.Block(false))
            {
                bool first = true;
                foreach (var section in syntax.Sections)
                    builder.AppendLine(ref first).Append(section, context);
            }
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ThrowStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("throw").Space().Append(syntax.Expression, context).SemiColon();
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, TryStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("try").AppendLine();
            builder.Append(syntax.Block, context).AppendLine();
            bool first = true;
            foreach (var catchClause in syntax.Catches)
                builder.AppendLine(ref first).Append(catchClause, context);

            if (syntax.Finally != null)
                builder.AppendLine().Append(syntax.Finally, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, UsingStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("try").Space().Parenthesized().Append(syntax.Declaration, context).Close().AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, WhileStatementSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("while").Space().Append(syntax.Condition, context).AppendLine()
                .IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        // Reference: roslyn/src/Compilers/CSharp/Portable/Generated/Syntax.xml.Main.Generated.cs
        public static CodeBuilder Append(this CodeBuilder builder, StatementSyntax statement, JavaCodeConversionContext context)
        {
            var kind = statement.Kind();
            switch (kind)
            {
                case SyntaxKind.Block:
                    return builder.Append(statement as BlockSyntax, context);
                case SyntaxKind.BreakStatement:
                    return builder.Append(statement as BreakStatementSyntax, context);
                case SyntaxKind.ForEachStatement:
                    return builder.Append(statement as ForEachStatementSyntax, context);
                case SyntaxKind.ContinueStatement:
                    return builder.Append(statement as ContinueStatementSyntax, context);
                case SyntaxKind.DoStatement:
                    return builder.Append(statement as DoStatementSyntax, context);
                case SyntaxKind.EmptyStatement:
                    return builder.Append(statement as EmptyStatementSyntax, context);
                case SyntaxKind.ExpressionStatement:
                    return builder.Append(statement as ExpressionStatementSyntax, context);
                case SyntaxKind.ForStatement:
                    return builder.Append(statement as ForStatementSyntax, context);
                case SyntaxKind.IfStatement:
                    return builder.Append(statement as IfStatementSyntax, context);
                case SyntaxKind.LocalDeclarationStatement:
                    return builder.Append(statement as LocalDeclarationStatementSyntax, context);
                case SyntaxKind.LockStatement:
                    return builder.Append(statement as LockStatementSyntax, context);
                case SyntaxKind.ReturnStatement:
                    return builder.Append(statement as ReturnStatementSyntax, context);
                case SyntaxKind.SwitchStatement:
                    return builder.Append(statement as SwitchStatementSyntax, context);
                case SyntaxKind.ThrowStatement:
                    return builder.Append(statement as ThrowStatementSyntax, context);
                case SyntaxKind.TryStatement:
                    return builder.Append(statement as TryStatementSyntax, context);
                case SyntaxKind.UsingStatement:
                    return builder.Append(statement as UsingStatementSyntax, context);
                case SyntaxKind.WhileStatement:
                    return builder.Append(statement as WhileStatementSyntax, context);
                // Unsupported statements
                case SyntaxKind.CheckedStatement:
                case SyntaxKind.UnsafeStatement:
                case SyntaxKind.LabeledStatement:
                case SyntaxKind.FixedStatement:
                case SyntaxKind.LocalFunctionStatement:
                case SyntaxKind.ForEachVariableStatement:
                // Unsupported yield statements
                case SyntaxKind.YieldBreakStatement:
                case SyntaxKind.YieldReturnStatement:
                // Unsupported goto statements
                case SyntaxKind.GotoStatement:
                case SyntaxKind.GotoCaseStatement:
                case SyntaxKind.GotoDefaultStatement:
                default:
                    throw new Exception();
            }
        }

        public static CodeBuilder Append(this CodeBuilder builder, VariableDeclarationSyntax syntax, JavaCodeConversionContext context)
        {
            Debug.Assert(syntax.Variables.Count == 1);
            builder.Append(syntax.Type, context).Space().Append(syntax.Variables[0], context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, VariableDeclaratorSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append(syntax.Identifier.Text);
            if (syntax.Initializer != null)
                builder.Space().Append(syntax.Initializer, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, EqualsValueClauseSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("=").Space().Append(syntax.Value, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, FinallyClauseSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("finally").AppendLine().Append(syntax.Block, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, CatchClauseSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("catch").Space().Parenthesized().Append(syntax.Declaration.Type, context).Space().Append(syntax.Declaration.Identifier.Text).Close().AppendLine();
            builder.Append(syntax.Block, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ElseClauseSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append("else").AppendLine();
            builder.IndentChild().Append(syntax.Statement, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, SwitchSectionSyntax syntax, JavaCodeConversionContext context)
        {
            foreach (var label in syntax.Labels)
                builder.Append(label, context).AppendLine();

            builder.IndentChild().Append(syntax.Statements, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, SwitchLabelSyntax syntax, JavaCodeConversionContext context)
        {
            builder.Append(syntax.Keyword.Text);
            switch (syntax.Kind())
            {
                case SyntaxKind.DefaultSwitchLabel:
                    break;
                case SyntaxKind.CaseSwitchLabel:
                    var caselabel = syntax as CaseSwitchLabelSyntax;
                    builder.Space().Append(caselabel.Value, context);
                    break;
                default:
                    throw new Exception();
            }
            builder.Colon();
            return builder;
        }
    }
}

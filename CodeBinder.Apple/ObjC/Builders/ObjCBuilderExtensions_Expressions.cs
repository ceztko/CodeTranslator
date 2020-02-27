﻿using CodeBinder.Shared;
using CodeBinder.Shared.CSharp;
using CodeBinder.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace CodeBinder.Apple
{
    static partial class ObjCBuilderExtension
    {
        public static CodeBuilder Append(this CodeBuilder builder, ArrayCreationExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append("new").Space().Append(syntax.Type, context);
            if (syntax.Initializer != null)
                builder.Append(syntax.Initializer, context);

            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, OmittedArraySizeExpressionSyntax syntax, ObjCCompilationContext context)
        {
            // Do nothing
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, AssignmentExpressionSyntax syntax, ObjCCompilationContext context)
        {
            var symbol = syntax.Left.GetSymbol(context);
            // Symbol can be null https://github.com/dotnet/roslyn/issues/31471
            if (symbol?.Kind == SymbolKind.Property)
            {
                var operatorKind = syntax.OperatorToken.Kind();
                switch (operatorKind)
                {
                    case SyntaxKind.EqualsToken:
                        {
                            if (syntax.Left.Kind() == SyntaxKind.ElementAccessExpression)
                            {
                                // Determine if the LHS of the assignment is an indexer set operation
                                var property = (IPropertySymbol)symbol;
                                if (!property.IsIndexer)
                                    break;

                                var elementAccess = (ElementAccessExpressionSyntax)syntax.Left;
                                builder.Append(elementAccess.Expression, context).Dot()
                                    .Append(elementAccess, property, context)
                                        .Parenthesized().Append(elementAccess.ArgumentList, false, context).CommaSeparator().Append(syntax.Right, context); ;
                                return builder;
                            }

                            builder.Append(syntax.Left, context).Parenthesized().Append(syntax.Right, context);
                            return builder;
                        }
                    default:
                        throw new Exception();
                }
            }

            builder.Append(syntax.Left, context).Space().Append(syntax.GetObjCOperator()).Space().Append(syntax.Right, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, BinaryExpressionSyntax syntax, ObjCCompilationContext context)
        {
            var kind = syntax.Kind();
            switch (kind)
            {
                case SyntaxKind.AsExpression:
                {
                    builder.Append("BinderUtils").Dot().Append("as").Parenthesized().Append(syntax.Left, context).CommaSeparator().Append(syntax.Right, context).Dot().Append("class");
                    return builder;
                }
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                {
                    IMethodSymbol? method;
                    if (syntax.TryGetSymbol(context, out method))
                    {
                        SymbolReplacement? replacement;
                        if (method.HasObjCReplacement(out replacement))
                        {
                            switch (replacement!.Kind)
                            {
                                case SymbolReplacementKind.StaticMethod:
                                {
                                    if (replacement.Negate)
                                        builder.ExclamationMark();

                                    builder.Append(replacement.Name).Parenthesized().Append(syntax.Left, context).CommaSeparator().Append(syntax.Right, context);
                                    return builder;
                                }
                                default:
                                {
                                    throw new NotImplementedException();
                                }
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }

            builder.Append(syntax.Left, context).Space().Append(syntax.GetObjCOperator()).Space().Append(syntax.Right, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, CastExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Parenthesized().Append(syntax.Type, context).Close().Append(syntax.Expression, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ConditionalExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax.Condition, context).Space().QuestionMark().Space()
                .Append(syntax.WhenTrue, context).Space().Colon().Space()
                .Append(syntax.WhenFalse, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ElementAccessExpressionSyntax syntax, ObjCCompilationContext context)
        {
            var symbol = syntax.GetSymbol(context);
            if (symbol?.Kind == SymbolKind.Property)
            {
                var property = (IPropertySymbol)symbol;
                Debug.Assert(property.IsIndexer);
                builder.Append(syntax.Expression, context).Dot().Append(syntax, property, context).Parenthesized().Append(syntax.ArgumentList, false, context);
                return builder;
            }

            builder.Append(syntax.Expression, context).Append(syntax.ArgumentList, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, InitializerExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Braced().Append(syntax.Expressions, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, BaseExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append("super");
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ThisExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append("this");
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, InvocationExpressionSyntax syntax, ObjCCompilationContext context)
        {
            var methodSymbol = syntax.GetSymbol<IMethodSymbol>(context)!;
            bool hasEmptyBody;
            if (methodSymbol.IsPartialMethod(out hasEmptyBody) && (hasEmptyBody || methodSymbol.PartialImplementationPart!.ShouldDiscard(context.Conversion)))
                return builder;

            if (methodSymbol.IsNative() && methodSymbol.ReturnType.TypeKind == TypeKind.Enum)
                builder.Append(methodSymbol.ReturnType.Name).Dot().Append("fromValue").Parenthesized(() => append(builder, syntax, context));
            else
                append(builder, syntax, context);

            return builder;
        }

        static void append(CodeBuilder builder, InvocationExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax.Expression, context).Append(syntax.ArgumentList, context);
        }

        public static CodeBuilder Append(this CodeBuilder builder, LiteralExpressionSyntax syntax, ObjCCompilationContext context)
        {
            var kind = syntax.Kind();
            switch (kind)
            {
                case SyntaxKind.NullLiteralExpression:
                {
                    return builder.Append("nil");
                }
                case SyntaxKind.StringLiteralExpression:
                {
                    if (syntax.Token.Kind() == SyntaxKind.StringLiteralToken && syntax.Token.Text.StartsWith("@"))
                        return builder.Append($"@{syntax.Token.Text.Replace("\"", "\"\"")}"); // Handle verbatim strings
                    else
                        return builder.Append($"@{syntax.Token.Text}");
                }
                case SyntaxKind.TrueLiteralExpression:
                {
                    return builder.Append("YES");
                }
                case SyntaxKind.FalseLiteralExpression:
                {
                    return builder.Append("NO");
                }
                default:
                {
                    return builder.Append(syntax.Token.Text);
                }
            }
        }

        public static CodeBuilder Append(this CodeBuilder builder, MemberAccessExpressionSyntax syntax, ObjCCompilationContext context)
        {
            if (builder.TryToReplace(syntax, context))
                return builder;

            var typeSymbol = syntax.Expression.GetTypeSymbol(context);
            if (typeSymbol?.IsCLRPrimitiveType() == true)
            {
                string objCBoxType = ObjCUtils.GetBoxType(typeSymbol.GetFullName())!;
                builder.Parenthesized().Parenthesized().Append(objCBoxType).Close().Append(syntax.Expression, context).Close().Dot().Append(syntax.Name, context);
                return builder;
            }

            var symbol = syntax.GetSymbol(context)!;
            if (symbol.Kind == SymbolKind.Property
                && symbol.OriginalDefinition.ContainingType.GetFullName() == "System.Nullable<T>"
                && symbol.Name == "Value")
            {
                // There are no nullable types in Java, just discard ".Value" accessor
                builder.Append(syntax.Expression, context);
                return builder;
            }

            builder.Append(syntax.Expression, context).Dot().Append(syntax.Name, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ObjectCreationExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append("new").Space().Append(syntax.Type, context).Append(syntax.ArgumentList!, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ParenthesizedExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Parenthesized().Append(syntax.Expression, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, PostfixUnaryExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax.Operand, context).Append(syntax.GetObjCOperator());
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, PrefixUnaryExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax.GetObjCOperator()).Append(syntax.Operand, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, TypeOfExpressionSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax.Type, context).Append(".class");
            return builder;
        }

        // Reference: roslyn/src/Compilers/CSharp/Portable/Generated/Syntax.xml.Main.Generated.cs
        public static CodeBuilder Append(this CodeBuilder builder, ExpressionSyntax expression, ObjCCompilationContext context)
        {
            var kind = expression.Kind();
            switch (kind)
            {
                case SyntaxKind.ArrayCreationExpression:
                    return builder.Append((ArrayCreationExpressionSyntax)expression, context);
                case SyntaxKind.OmittedArraySizeExpression:
                    return builder.Append((OmittedArraySizeExpressionSyntax)expression, context);
                case SyntaxKind.AddAssignmentExpression:
                case SyntaxKind.AndAssignmentExpression:
                case SyntaxKind.DivideAssignmentExpression:
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                case SyntaxKind.LeftShiftAssignmentExpression:
                case SyntaxKind.ModuloAssignmentExpression:
                case SyntaxKind.MultiplyAssignmentExpression:
                case SyntaxKind.OrAssignmentExpression:
                case SyntaxKind.RightShiftAssignmentExpression:
                case SyntaxKind.SimpleAssignmentExpression:
                case SyntaxKind.SubtractAssignmentExpression:
                    return builder.Append((AssignmentExpressionSyntax)expression, context);
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                case SyntaxKind.LogicalOrExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.ExclusiveOrExpression:
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.IsExpression:
                case SyntaxKind.AsExpression:
                    return builder.Append((BinaryExpressionSyntax)expression, context);
                case SyntaxKind.CastExpression:
                    return builder.Append((CastExpressionSyntax)expression, context);
                case SyntaxKind.ConditionalExpression:
                    return builder.Append((ConditionalExpressionSyntax)expression, context);
                case SyntaxKind.ElementAccessExpression:
                    return builder.Append((ElementAccessExpressionSyntax)expression, context);
                case SyntaxKind.ObjectInitializerExpression:
                case SyntaxKind.CollectionInitializerExpression:
                case SyntaxKind.ArrayInitializerExpression:
                case SyntaxKind.ComplexElementInitializerExpression:
                    return builder.Append((InitializerExpressionSyntax)expression, context);
                case SyntaxKind.BaseExpression:
                    return builder.Append((BaseExpressionSyntax)expression, context);
                case SyntaxKind.ThisExpression:
                    return builder.Append((ThisExpressionSyntax)expression, context);
                case SyntaxKind.InvocationExpression:
                    return builder.Append((InvocationExpressionSyntax)expression, context);
                case SyntaxKind.NumericLiteralExpression:
                case SyntaxKind.StringLiteralExpression:
                case SyntaxKind.CharacterLiteralExpression:
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                case SyntaxKind.NullLiteralExpression:
                    return builder.Append((LiteralExpressionSyntax)expression, context);
                case SyntaxKind.SimpleMemberAccessExpression:
                    return builder.Append((MemberAccessExpressionSyntax)expression, context);
                case SyntaxKind.ObjectCreationExpression:
                    return builder.Append((ObjectCreationExpressionSyntax)expression, context);
                case SyntaxKind.ParenthesizedExpression:
                    return builder.Append((ParenthesizedExpressionSyntax)expression, context);
                case SyntaxKind.PostIncrementExpression:
                case SyntaxKind.PostDecrementExpression:
                    return builder.Append((PostfixUnaryExpressionSyntax)expression, context);
                case SyntaxKind.UnaryPlusExpression:
                case SyntaxKind.UnaryMinusExpression:
                case SyntaxKind.BitwiseNotExpression:
                case SyntaxKind.LogicalNotExpression:
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                    return builder.Append((PrefixUnaryExpressionSyntax)expression, context);
                case SyntaxKind.TypeOfExpression:
                    return builder.Append((TypeOfExpressionSyntax)expression, context);
                case SyntaxKind.QualifiedName:
                case SyntaxKind.ArrayType:
                case SyntaxKind.GenericName:
                case SyntaxKind.IdentifierName:
                case SyntaxKind.NullableType:
                case SyntaxKind.OmittedTypeArgument:
                case SyntaxKind.PredefinedType:
                case SyntaxKind.RefType:
                    return builder.Append((TypeSyntax)expression, context);
                // Unsupported expressions
                case SyntaxKind.RefExpression:
                case SyntaxKind.DeclarationExpression:
                case SyntaxKind.ThrowExpression:
                case SyntaxKind.DefaultExpression:
                case SyntaxKind.AnonymousMethodExpression:
                case SyntaxKind.ParenthesizedLambdaExpression:
                case SyntaxKind.SimpleLambdaExpression:
                case SyntaxKind.RefValueExpression:
                case SyntaxKind.RefTypeExpression:
                case SyntaxKind.ImplicitArrayCreationExpression:
                case SyntaxKind.ElementBindingExpression:
                case SyntaxKind.ImplicitElementAccess:
                case SyntaxKind.MemberBindingExpression:
                case SyntaxKind.SizeOfExpression:
                case SyntaxKind.MakeRefExpression:
                case SyntaxKind.ImplicitStackAllocArrayCreationExpression:
                case SyntaxKind.InterpolatedStringExpression:
                case SyntaxKind.AwaitExpression:
                case SyntaxKind.QueryExpression:
                case SyntaxKind.StackAllocArrayCreationExpression:
                case SyntaxKind.AnonymousObjectCreationExpression:
                case SyntaxKind.TupleExpression:
                case SyntaxKind.IsPatternExpression:
                case SyntaxKind.CheckedExpression:
                case SyntaxKind.ConditionalAccessExpression:
                // Unsupported prefix unary expressions
                case SyntaxKind.AddressOfExpression:
                case SyntaxKind.PointerIndirectionExpression:
                // Unsupported binary expressions
                case SyntaxKind.CoalesceExpression:
                // Unsupported member access expressions
                case SyntaxKind.PointerMemberAccessExpression:
                // Unsupported literal expressions
                case SyntaxKind.ArgListExpression:
                case SyntaxKind.DefaultLiteralExpression:
                // Unsupported type expressions
                case SyntaxKind.AliasQualifiedName:
                case SyntaxKind.TupleType:
                case SyntaxKind.PointerType:
                default:
                    throw new Exception();
            }
        }

        public static CodeBuilder Append(this CodeBuilder builder, BracketedArgumentListSyntax syntax, ObjCCompilationContext context)
        {
            builder.Append(syntax, true, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, BracketedArgumentListSyntax syntax, bool bracketed, ObjCCompilationContext context)
        {
            if (bracketed)
                builder.Bracketed().Append(syntax.Arguments, context);
            else
                builder.Append(syntax.Arguments, context);

            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, ArgumentListSyntax syntax, ObjCCompilationContext context)
        {
            var parentSymbol = syntax.Parent.GetSymbol(context);
            bool isNativeInvocation = false;
            if (parentSymbol?.Kind == SymbolKind.Method && (parentSymbol as IMethodSymbol)!.IsNative())
                isNativeInvocation = true;

            builder.Append(syntax.Arguments, isNativeInvocation, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, IEnumerable<ArgumentSyntax> arguments, ObjCCompilationContext context)
        {
            builder.Append(arguments, false, context);
            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, IEnumerable<ArgumentSyntax> arguments, bool native, ObjCCompilationContext context)
        {
            bool first = true;
            foreach (var arg in arguments)
            {
                builder.ColonSeparator(ref first);

                if (native)
                {
                    // In native invocations, prepend "__" for ref/out arguments
                    if (!arg.RefKindKeyword.IsNone())
                        builder.Append("__");
                }

                builder.Append(arg.Expression, context);

                if (native && arg.RefKindKeyword.IsNone())
                {
                    // In native invocations, append ".value" for enum arguments
                    var typeSymbol = arg.Expression.GetTypeSymbol(context)!;
                    if (typeSymbol.TypeKind == TypeKind.Enum)
                        builder.Dot().Append("value");
                }
            }

            return builder;
        }

        public static CodeBuilder Append(this CodeBuilder builder, IEnumerable<ExpressionSyntax> expressions, ObjCCompilationContext context)
        {
            bool first = true;
            foreach (var expression in expressions)
                builder.CommaSeparator(ref first).Append(expression, context);

            return builder;
        }
    }
}

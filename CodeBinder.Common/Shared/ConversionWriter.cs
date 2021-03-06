﻿// Copyright(c) 2020 Francesco Pretto
// This file is subject to the MIT license
using CodeBinder.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBinder.Shared
{
    /// <summary>
    /// Class to write a conversion with a context
    /// </summary>
    /// <seealso cref="ConversionDelegate"/>
    /// <remarks>Not coupled to CodeBuilder like <see cref="CodeWriter"/></remarks>
    public abstract class ConversionWriter<TContext> : ConversionWriter
    {
        public TContext Context { get; private set; }

        public ConversionWriter(TContext context)
        {
            Context = context;
        }
    }

    /// <summary>
    /// Class to write a conversion 
    /// </summary>
    /// <seealso cref="ConversionDelegate"/>
    /// <remarks>Not coupled to CodeBuilder like <see cref="CodeWriter"/></remarks>
    public abstract class ConversionWriter : IConversionWriter
    {
        public void Write(CodeBuilder builder)
        {
            string? preamble = GetGeneratedPreamble();
            if (!string.IsNullOrEmpty(preamble))
                builder.AppendLine(preamble);

            write(builder);
        }

        public virtual bool Skip => false;

        protected abstract void write(CodeBuilder builder);

        protected abstract string GetFileName();

        protected virtual string? GetBasePath() => null;

        protected virtual string? GetGeneratedPreamble() => null;

        public string FileName => GetFileName();

        public string? BasePath => GetBasePath();

        public string? GeneratedPreamble => GetGeneratedPreamble();
    }

    /// <summary>
    /// Interface to write a conversion
    /// </summary>
    /// <seealso cref="ConversionDelegate"/>
    public interface IConversionWriter
    {
        void Write(CodeBuilder builder);

        string FileName { get; }

        string? BasePath { get; }

        bool Skip { get; }
    }
}

﻿// Copyright (c) 2017-2018 ICSharpCode
// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using CodeTranslator.Shared;
using CodeTranslator.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeTranslator
{
    public class ConversionDelegate
    {
        IConversionBuilder _builder;

        public string SourcePath { get; private set; }

        internal ConversionDelegate(IConversionBuilder builder)
            : this(null, builder) { }

        internal ConversionDelegate(string sourcePath, IConversionBuilder builder)
        {
            SourcePath = sourcePath;
            _builder = builder;
        }

        public string TargetFileName
        {
            get { return _builder.FileName; }
        }

        public string TargetBasePath
        {
            get { return _builder.BasePath; }
        }

        public void Write(Stream stream)
        {
            var writer = new StreamWriter(stream, new UTF8Encoding(true));
            write(writer);
        }

        public string ToFullString()
        {
            var writer = new StringWriter();
            write(writer);
            return writer.ToString();
        }

        private void write(TextWriter writer)
        {
            var codeBuilder = new CodeBuilder(writer);
            string preamble = _builder.GeneratedPreamble;
            if (!string.IsNullOrEmpty(preamble))
                codeBuilder.AppendLine(preamble);

            _builder.Write(codeBuilder);
        }
    }
}

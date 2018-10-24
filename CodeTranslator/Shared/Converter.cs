﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeTranslator.Shared
{
    public class GeneratorArgs
    {
        public string SourceRootPath;
    }

    public abstract class Converter
    {
        public LanguageConversion Conversion
        {
            get { return GetConversion(); }
        }

        protected abstract LanguageConversion GetConversion();

        public abstract IEnumerable<ConversionResult> Convert();

        public void ConvertAndWrite(GeneratorArgs args)
        {
            foreach (var result in Convert().Concat(Conversion.DefaultResults))
            {
                var path = Path.Combine(args.SourceRootPath,
                    result.TargetBasePath, result.TargetFileName);
                File.WriteAllText(path, result.ConvertedCode);
            }
        }
    }
}

﻿// Copyright(c) 2020 Francesco Pretto
// This file is subject to the MIT license
using CodeBinder.Shared;
using CodeBinder.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeBinder.Apple
{
    abstract class ObjCHeaderConversionWriter : ObjCBaseHeaderConversionWriter
    {
        public ObjCCompilationContext Compilation { get; private set; }

        public ObjCHeaderConversionWriter(ObjCCompilationContext compilation)
        {
            Compilation = compilation;
        }

        protected override string HeaderGuardPrefix => $"CODE_BINDER_OBJC_{Compilation.ObjCLibraryName.ToUpper()}";
    }

    abstract class ObjCBaseHeaderConversionWriter : ConversionWriter
    {
        protected void BeginHeaderGuard(CodeBuilder builder)
        {
            builder.AppendLine($"#ifndef {HeaderGuard}");
            builder.AppendLine($"#define {HeaderGuard}");
        }

        protected void EndHeaderGuard(CodeBuilder builder)
        {
            builder.AppendLine($"#endif // {HeaderGuard}");
        }

        private string HeaderGuard
        {
            get
            {
                var stem = HeaderGuardStem;
                if (stem.Length == 0)
                    throw new Exception("Stem is empty");

                return $"{HeaderGuardPrefix}_{HeaderGuardStem}_HEADER";
            }
        }

        protected virtual string HeaderGuardPrefix => "CODE_BINDER_OBJC";

        protected abstract string HeaderGuardStem { get; }
    }
}

﻿// Copyright(c) 2018 Francesco Pretto
// This file is subject to the MIT license
using System;
using System.Collections.Generic;
using System.Linq;
using CodeTranslator.Shared;
using CodeTranslator.Shared.CSharp;

namespace CodeTranslator.Java
{
    public class CSToJavaConversion : CSharpLanguageConversion
    {
        /// <summary>Base namespace of the package, to be set outside</summary>
        public string BaseNamespace { get; set; }

        protected override TypeConversion<CSharpClassTypeContext> GetClassTypeConversion()
        {
            var ret = new JavaClassConversion();
            ret.Namespace = BaseNamespace;
            return ret;
        }

        protected override TypeConversion<CSharpInterfaceTypeContext> GetInterfaceTypeConversion()
        {
            var ret = new JavaInterfaceConversion();
            ret.Namespace = BaseNamespace;
            return ret;
        }

        protected override TypeConversion<CSharpStructTypeContext> GetStructTypeConversion()
        {
            var ret = new JavaStructConversion();
            ret.Namespace = BaseNamespace;
            return ret;
        }

        protected override TypeConversion<CSharpEnumTypeContext> GetEnumTypeConversion()
        {
            var ret = new JavaEnumConversion();
            ret.Namespace = BaseNamespace;
            return ret;
        }
    }
}

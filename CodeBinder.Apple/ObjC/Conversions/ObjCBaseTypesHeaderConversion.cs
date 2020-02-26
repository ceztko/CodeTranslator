﻿using CodeBinder.Util;
using CodeBinder.Shared;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using CodeBinder.Shared.CSharp;
using CodeBinder.Attributes;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeBinder.Apple
{
    class ObjCBaseTypesHeaderConversion : ObjCHeaderConversionWriter
    {
        public ObjCBaseTypesHeaderConversion(ObjCCompilationContext compilation)
            : base(compilation) { }

        protected override string GetFileName() => ConversionCSharpToObjC.BaseTypesHeader;

        protected override string? GetBasePath() => ConversionCSharpToObjC.InternalBasePath;

        protected override string? GetGeneratedPreamble() => ConversionCSharpToObjC.SourcePreamble;

        protected override void write(CodeBuilder builder)
        {
            BeginHeaderGuard(builder);
            builder.AppendLine();
            builder.AppendLine("// Foundation headers");
            builder.AppendLine("#import <Foundation/Foundation.h>");
            builder.AppendLine();
            builder.AppendLine("// C Std headers");
            builder.AppendLine("#ifdef __cplusplus");
            builder.AppendLine("#include <cstdint>");
            builder.AppendLine("#include <cinttypes>");
            builder.AppendLine("#else // __cplusplus");
            builder.AppendLine("#include <stdint.h>");
            builder.AppendLine("#include <uchar.h>");
            builder.AppendLine("#include <inttypes.h>");
            builder.AppendLine("#endif // __cplusplus");
            builder.AppendLine();
            builder.AppendLine("// Interop array box types");
            foreach (var type in ObjCUtils.GetInteropTypes())
                builder.AppendLine($"#include \"{ObjCUtils.ToArrayBoxTypeName(type)}.h\"");
            builder.AppendLine();
            builder.AppendLine("// Other types");
            builder.AppendLine($"#include {nameof(ObjCClasses.CBIEqualityCompararer_h).ToObjCHeaderFilename(ObjCHeaderNameUse.IncludeRelativeFirst)}");
            builder.AppendLine($"#include {nameof(ObjCClasses.CBIReadOnlyList_h).ToObjCHeaderFilename(ObjCHeaderNameUse.IncludeRelativeFirst)}");
            builder.AppendLine($"#include {nameof(ObjCClasses.CBIDisposable_h).ToObjCHeaderFilename(ObjCHeaderNameUse.IncludeRelativeFirst)}");
            builder.AppendLine($"#include {nameof(ObjCClasses.CBKeyValuePair_h).ToObjCHeaderFilename(ObjCHeaderNameUse.IncludeRelativeFirst)}");
            builder.AppendLine($"#include {nameof(ObjCClasses.CBHandleRef_h).ToObjCHeaderFilename(ObjCHeaderNameUse.IncludeRelativeFirst)}");
            EndHeaderGuard(builder);
        }

        protected override string HeaderGuardStem => "BASE_TYPES";
    }
}

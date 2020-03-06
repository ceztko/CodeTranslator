#ifndef CODE_BINDER_INTEROP_INTERNAL_HEADER
#define CODE_BINDER_INTEROP_INTERNAL_HEADER
#pragma once

#include "../CBInterop.h"
#include <cstddef>
#include <cstring>
#include <string>
#include <codecvt>
#include <new>
#include <StringIntl.h>

#ifdef WIN32
extern "C" __declspec(dllimport) void* __stdcall CoTaskMemAlloc(size_t cb);
extern "C" __declspec(dllimport) void* __stdcall LocalAlloc(unsigned int uFlags, size_t uBytes);
#endif // WIN32

// _CBU: Narrow to platform specific codebinder unicode string (std::string in Apple,
//       std::u16string everywhere else) and take data view
#ifdef CBSTRING_UTF8
#define _CBU(str) _U8C(str)
#else
#define _CBU(str) _U16C(str)
#endif

namespace cb
{
    inline cbstring_t CopyString(const cbchar_t* str, size_t len)
    {
        // Allocate also the space of the termination character
#ifdef WIN32
        cbstring_t newstr = (cbstring_t)CoTaskMemAlloc((len + 1) * sizeof(cbchar_t));
#else
        cbstring_t newstr = (cbstring_t)std::malloc((len + 1) * sizeof(cbchar_t));
#endif
        if (newstr == nullptr)
            throw std::bad_alloc();

        std::memcpy(newstr, str, len * sizeof(cbchar_t));
        newstr[len] = CB_NULL_TERMINATION;
        return newstr;
    }

    inline cbstring_t MarshalString(const char* str_, size_t len_)
    {
        if (str_ == nullptr)
            return nullptr;

#ifdef CBSTRING_UTF8
        auto str = str_;
        size_t len = len_;
#else
        auto u16str = usr::_U8ToU16(str_, len_);
        auto str = u16str.c_str();
        size_t len = u16str.length();
#endif

        return CopyString(str, len);
    }

    inline cbstring_t MarshalString(const char* str)
    {
        return MarshalString(str, std::char_traits<char>::length(str));
    }

    inline cbstring_t MarshalString(const std::string& str)
    {
        return MarshalString(str.c_str(), str.length());
    }

    inline cbstring_t MarshalString(const char16_t* str_, size_t len_)
    {
        if (str_ == nullptr)
            return nullptr;

#ifdef CBSTRING_UTF8
        auto u8str = usr::_U16ToU8(str_, len_);
        auto str = u8str.c_str();
        size_t len = u8str.length();
#else
        auto str = str_;
        size_t len = len_;
#endif
        return CopyString(str, len);
    }

    inline cbstring_t MarshalString(const char16_t* str)
    {
        return MarshalString(str, std::char_traits<char16_t>::length(str));
    }

    inline cbstring_t MarshalString(const std::u16string& str)
    {
        return MarshalString(str.c_str(), str.length());
    }

#ifdef WIN32
    inline cbstring_t MarshalString(const wchar_t* str, size_t len)
    {
        return MarshalString(reinterpret_cast<const char16_t*>(str), len);
    }

    inline cbstring_t MarshalString(const wchar_t* str)
    {
        return MarshalString(reinterpret_cast<const char16_t*>(str));
    }

    inline cbstring_t MarshalString(const std::wstring& str)
    {
        return MarshalString(reinterpret_cast<const std::u16string&>(str));
    }
#endif // WIN32

    inline void* AllocMemory(size_t size)
    {
#ifdef WIN32
        unsigned int LMEM_FIXED = 0x0000;
        auto ret = LocalAlloc(LMEM_FIXED, size);
#else
        auto ret = malloc(size);

#endif
        if (ret == nullptr)
            throw std::bad_alloc();

        return ret;
    }
}
#endif // CODE_BINDER_INTEROP_INTERNAL_HEADER
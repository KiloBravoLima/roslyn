﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.CSharp
{
    [Flags]
    internal enum UnaryOperatorKind
    {
        // NOTE: these types should line up with the elements in BinaryOperatorKind

        TypeMask = 0x000000FF,

        SByte = 0x00000011,
        Byte = 0x00000012,
        Short = 0x00000013,
        UShort = 0x00000014,
        Int = 0x00000015,
        UInt = 0x00000016,
        Long = 0x00000017,
        ULong = 0x00000018,
        Char = 0x00000019,
        Float = 0x0000001A,
        Double = 0x0000001B,
        Decimal = 0x0000001C,
        Bool = 0x0000001D,
        _Object = 0x0000001E, // reserved for binary op
        _String = 0x0000001F, // reserved for binary op
        _StringAndObject = 0x00000021, // reserved for binary op
        _ObjectAndString = 0x00000022, // reserved for binary op

        Enum = 0x00000023,
        _EnumAndUnderlying = 0x00000024, // reserved for binary op
        _UnderlyingAndEnum = 0x00000025, // reserved for binary op
        _Delegate = 0x00000026, // reserved for binary op
        Pointer = 0x00000027,
        _PointerAndInt = 0x00000028, // reserved for binary op
        _PointerAndUInt = 0x00000029, // reserved for binary op
        _PointerAndLong = 0x0000002A, // reserved for binary op
        _PointerAndULong = 0x0000002B, // reserved for binary op
        _IntAndPointer = 0x0000002C, // reserved for binary op
        _UIntAndPointer = 0x0000002D, // reserved for binary op
        _LongAndPointer = 0x0000002E, // reserved for binary op
        _ULongAndPointer = 0x0000002F, // reserved for binary op
        _NullableNull = 0x00000030, // reserved for binary op
        UserDefined = 0x00000031,
        Dynamic = 0x00000032,
        Unit = 0x00000033,

        OpMask = 0x0000FF00,
        PostfixIncrement = 0x00001000,
        PostfixDecrement = 0x00001100,
        PrefixIncrement = 0x00001200,
        PrefixDecrement = 0x00001300,
        UnaryPlus = 0x00001400,
        UnaryMinus = 0x00001500,
        LogicalNegation = 0x00001600,
        BitwiseComplement = 0x00001700,
        True = 0x00001800,
        False = 0x00001900,

        Lifted = 0x00010000,
        _Logical = 0x00020000, // reserved for binary op              
        Checked = 0x00040000,

        Error = 0x00000000,

        SBytePostfixIncrement = SByte | PostfixIncrement,
        BytePostfixIncrement = Byte | PostfixIncrement,
        ShortPostfixIncrement = Short | PostfixIncrement,
        UShortPostfixIncrement = UShort | PostfixIncrement,
        IntPostfixIncrement = Int | PostfixIncrement,
        UIntPostfixIncrement = UInt | PostfixIncrement,
        LongPostfixIncrement = Long | PostfixIncrement,
        ULongPostfixIncrement = ULong | PostfixIncrement,
        CharPostfixIncrement = Char | PostfixIncrement,
        FloatPostfixIncrement = Float | PostfixIncrement,
        DoublePostfixIncrement = Double | PostfixIncrement,
        DecimalPostfixIncrement = Decimal | PostfixIncrement,
        EnumPostfixIncrement = Enum | PostfixIncrement,
        UserDefinedPostfixIncrement = UserDefined | PostfixIncrement,
        LiftedSBytePostfixIncrement = Lifted | SByte | PostfixIncrement,
        LiftedBytePostfixIncrement = Lifted | Byte | PostfixIncrement,
        LiftedShortPostfixIncrement = Lifted | Short | PostfixIncrement,
        LiftedUShortPostfixIncrement = Lifted | UShort | PostfixIncrement,
        LiftedIntPostfixIncrement = Lifted | Int | PostfixIncrement,
        LiftedUIntPostfixIncrement = Lifted | UInt | PostfixIncrement,
        LiftedLongPostfixIncrement = Lifted | Long | PostfixIncrement,
        LiftedULongPostfixIncrement = Lifted | ULong | PostfixIncrement,
        LiftedCharPostfixIncrement = Lifted | Char | PostfixIncrement,
        LiftedFloatPostfixIncrement = Lifted | Float | PostfixIncrement,
        LiftedDoublePostfixIncrement = Lifted | Double | PostfixIncrement,
        LiftedDecimalPostfixIncrement = Lifted | Decimal | PostfixIncrement,
        LiftedEnumPostfixIncrement = Lifted | Enum | PostfixIncrement,
        LiftedUserDefinedPostfixIncrement = Lifted | UserDefined | PostfixIncrement,
        PointerPostfixIncrement = Pointer | PostfixIncrement,
        DynamicPostfixIncrement = Dynamic | PostfixIncrement,

        SBytePrefixIncrement = SByte | PrefixIncrement,
        BytePrefixIncrement = Byte | PrefixIncrement,
        ShortPrefixIncrement = Short | PrefixIncrement,
        UShortPrefixIncrement = UShort | PrefixIncrement,
        IntPrefixIncrement = Int | PrefixIncrement,
        UIntPrefixIncrement = UInt | PrefixIncrement,
        LongPrefixIncrement = Long | PrefixIncrement,
        ULongPrefixIncrement = ULong | PrefixIncrement,
        CharPrefixIncrement = Char | PrefixIncrement,
        FloatPrefixIncrement = Float | PrefixIncrement,
        DoublePrefixIncrement = Double | PrefixIncrement,
        DecimalPrefixIncrement = Decimal | PrefixIncrement,
        EnumPrefixIncrement = Enum | PrefixIncrement,
        UserDefinedPrefixIncrement = UserDefined | PrefixIncrement,
        LiftedSBytePrefixIncrement = Lifted | SByte | PrefixIncrement,
        LiftedBytePrefixIncrement = Lifted | Byte | PrefixIncrement,
        LiftedShortPrefixIncrement = Lifted | Short | PrefixIncrement,
        LiftedUShortPrefixIncrement = Lifted | UShort | PrefixIncrement,
        LiftedIntPrefixIncrement = Lifted | Int | PrefixIncrement,
        LiftedUIntPrefixIncrement = Lifted | UInt | PrefixIncrement,
        LiftedLongPrefixIncrement = Lifted | Long | PrefixIncrement,
        LiftedULongPrefixIncrement = Lifted | ULong | PrefixIncrement,
        LiftedCharPrefixIncrement = Lifted | Char | PrefixIncrement,
        LiftedFloatPrefixIncrement = Lifted | Float | PrefixIncrement,
        LiftedDoublePrefixIncrement = Lifted | Double | PrefixIncrement,
        LiftedDecimalPrefixIncrement = Lifted | Decimal | PrefixIncrement,
        LiftedEnumPrefixIncrement = Lifted | Enum | PrefixIncrement,
        LiftedUserDefinedPrefixIncrement = Lifted | UserDefined | PrefixIncrement,
        PointerPrefixIncrement = Pointer | PrefixIncrement,
        DynamicPrefixIncrement = Dynamic | PrefixIncrement,

        SBytePostfixDecrement = SByte | PostfixDecrement,
        BytePostfixDecrement = Byte | PostfixDecrement,
        ShortPostfixDecrement = Short | PostfixDecrement,
        UShortPostfixDecrement = UShort | PostfixDecrement,
        IntPostfixDecrement = Int | PostfixDecrement,
        UIntPostfixDecrement = UInt | PostfixDecrement,
        LongPostfixDecrement = Long | PostfixDecrement,
        ULongPostfixDecrement = ULong | PostfixDecrement,
        CharPostfixDecrement = Char | PostfixDecrement,
        FloatPostfixDecrement = Float | PostfixDecrement,
        DoublePostfixDecrement = Double | PostfixDecrement,
        DecimalPostfixDecrement = Decimal | PostfixDecrement,
        EnumPostfixDecrement = Enum | PostfixDecrement,
        UserDefinedPostfixDecrement = UserDefined | PostfixDecrement,
        LiftedSBytePostfixDecrement = Lifted | SByte | PostfixDecrement,
        LiftedBytePostfixDecrement = Lifted | Byte | PostfixDecrement,
        LiftedShortPostfixDecrement = Lifted | Short | PostfixDecrement,
        LiftedUShortPostfixDecrement = Lifted | UShort | PostfixDecrement,
        LiftedIntPostfixDecrement = Lifted | Int | PostfixDecrement,
        LiftedUIntPostfixDecrement = Lifted | UInt | PostfixDecrement,
        LiftedLongPostfixDecrement = Lifted | Long | PostfixDecrement,
        LiftedULongPostfixDecrement = Lifted | ULong | PostfixDecrement,
        LiftedCharPostfixDecrement = Lifted | Char | PostfixDecrement,
        LiftedFloatPostfixDecrement = Lifted | Float | PostfixDecrement,
        LiftedDoublePostfixDecrement = Lifted | Double | PostfixDecrement,
        LiftedDecimalPostfixDecrement = Lifted | Decimal | PostfixDecrement,
        LiftedEnumPostfixDecrement = Lifted | Enum | PostfixDecrement,
        LiftedUserDefinedPostfixDecrement = Lifted | UserDefined | PostfixDecrement,
        PointerPostfixDecrement = Pointer | PostfixDecrement,
        DynamicPostfixDecrement = Dynamic | PostfixDecrement,

        SBytePrefixDecrement = SByte | PrefixDecrement,
        BytePrefixDecrement = Byte | PrefixDecrement,
        ShortPrefixDecrement = Short | PrefixDecrement,
        UShortPrefixDecrement = UShort | PrefixDecrement,
        IntPrefixDecrement = Int | PrefixDecrement,
        UIntPrefixDecrement = UInt | PrefixDecrement,
        LongPrefixDecrement = Long | PrefixDecrement,
        ULongPrefixDecrement = ULong | PrefixDecrement,
        CharPrefixDecrement = Char | PrefixDecrement,
        FloatPrefixDecrement = Float | PrefixDecrement,
        DoublePrefixDecrement = Double | PrefixDecrement,
        DecimalPrefixDecrement = Decimal | PrefixDecrement,
        EnumPrefixDecrement = Enum | PrefixDecrement,
        UserDefinedPrefixDecrement = UserDefined | PrefixDecrement,
        LiftedSBytePrefixDecrement = Lifted | SByte | PrefixDecrement,
        LiftedBytePrefixDecrement = Lifted | Byte | PrefixDecrement,
        LiftedShortPrefixDecrement = Lifted | Short | PrefixDecrement,
        LiftedUShortPrefixDecrement = Lifted | UShort | PrefixDecrement,
        LiftedIntPrefixDecrement = Lifted | Int | PrefixDecrement,
        LiftedUIntPrefixDecrement = Lifted | UInt | PrefixDecrement,
        LiftedLongPrefixDecrement = Lifted | Long | PrefixDecrement,
        LiftedULongPrefixDecrement = Lifted | ULong | PrefixDecrement,
        LiftedCharPrefixDecrement = Lifted | Char | PrefixDecrement,
        LiftedFloatPrefixDecrement = Lifted | Float | PrefixDecrement,
        LiftedDoublePrefixDecrement = Lifted | Double | PrefixDecrement,
        LiftedDecimalPrefixDecrement = Lifted | Decimal | PrefixDecrement,
        LiftedEnumPrefixDecrement = Lifted | Enum | PrefixDecrement,
        LiftedUserDefinedPrefixDecrement = Lifted | UserDefined | PrefixDecrement,
        PointerPrefixDecrement = Pointer | PrefixDecrement,
        DynamicPrefixDecrement = Dynamic | PrefixDecrement,

        IntUnaryPlus = Int | UnaryPlus,
        UIntUnaryPlus = UInt | UnaryPlus,
        LongUnaryPlus = Long | UnaryPlus,
        ULongUnaryPlus = ULong | UnaryPlus,
        FloatUnaryPlus = Float | UnaryPlus,
        DoubleUnaryPlus = Double | UnaryPlus,
        DecimalUnaryPlus = Decimal | UnaryPlus,
        UserDefinedUnaryPlus = UserDefined | UnaryPlus,
        LiftedIntUnaryPlus = Lifted | Int | UnaryPlus,
        LiftedUIntUnaryPlus = Lifted | UInt | UnaryPlus,
        LiftedLongUnaryPlus = Lifted | Long | UnaryPlus,
        LiftedULongUnaryPlus = Lifted | ULong | UnaryPlus,
        LiftedFloatUnaryPlus = Lifted | Float | UnaryPlus,
        LiftedDoubleUnaryPlus = Lifted | Double | UnaryPlus,
        LiftedDecimalUnaryPlus = Lifted | Decimal | UnaryPlus,
        LiftedUserDefinedUnaryPlus = Lifted | UserDefined | UnaryPlus,
        DynamicUnaryPlus = Dynamic | UnaryPlus,

        IntUnaryMinus = Int | UnaryMinus,
        LongUnaryMinus = Long | UnaryMinus,
        FloatUnaryMinus = Float | UnaryMinus,
        DoubleUnaryMinus = Double | UnaryMinus,
        DecimalUnaryMinus = Decimal | UnaryMinus,
        UserDefinedUnaryMinus = UserDefined | UnaryMinus,
        LiftedIntUnaryMinus = Lifted | Int | UnaryMinus,
        LiftedLongUnaryMinus = Lifted | Long | UnaryMinus,
        LiftedFloatUnaryMinus = Lifted | Float | UnaryMinus,
        LiftedDoubleUnaryMinus = Lifted | Double | UnaryMinus,
        LiftedDecimalUnaryMinus = Lifted | Decimal | UnaryMinus,
        LiftedUserDefinedUnaryMinus = Lifted | UserDefined | UnaryMinus,
        DynamicUnaryMinus = Dynamic | UnaryMinus,

        BoolLogicalNegation = Bool | LogicalNegation,
        UserDefinedLogicalNegation = UserDefined | LogicalNegation,
        LiftedBoolLogicalNegation = Lifted | Bool | LogicalNegation,
        LiftedUserDefinedLogicalNegation = Lifted | UserDefined | LogicalNegation,
        DynamicLogicalNegation = Dynamic | LogicalNegation,

        IntBitwiseComplement = Int | BitwiseComplement,
        UIntBitwiseComplement = UInt | BitwiseComplement,
        LongBitwiseComplement = Long | BitwiseComplement,
        ULongBitwiseComplement = ULong | BitwiseComplement,
        EnumBitwiseComplement = Enum | BitwiseComplement,
        UserDefinedBitwiseComplement = UserDefined | BitwiseComplement,
        LiftedIntBitwiseComplement = Lifted | Int | BitwiseComplement,
        LiftedUIntBitwiseComplement = Lifted | UInt | BitwiseComplement,
        LiftedLongBitwiseComplement = Lifted | Long | BitwiseComplement,
        LiftedULongBitwiseComplement = Lifted | ULong | BitwiseComplement,
        LiftedEnumBitwiseComplement = Lifted | Enum | BitwiseComplement,
        LiftedUserDefinedBitwiseComplement = Lifted | UserDefined | BitwiseComplement,
        DynamicBitwiseComplement = Dynamic | BitwiseComplement,

        // operator true and operator false are almost always user-defined, and never lifted.
        UserDefinedTrue = UserDefined | True,
        UserDefinedFalse = UserDefined | False,

        // The one time operator true is not user-defined is "if(dyn)" where dyn is of type dynamic.
        // In that case we bind this as a dynamic "operator true" invocation, rather than as a 
        // dynamic conversion to bool.
        DynamicTrue = Dynamic | True,

        // Used during lowering of dynamic logical operators.
        DynamicFalse = Dynamic | False,
    }

    [Flags]
    internal enum BinaryOperatorKind
    {
        // NOTE: these types should line up with the elements in UnaryOperatorKind

        TypeMask = UnaryOperatorKind.TypeMask,

        Int = UnaryOperatorKind.Int,
        UInt = UnaryOperatorKind.UInt,
        Long = UnaryOperatorKind.Long,
        ULong = UnaryOperatorKind.ULong,
        Char = UnaryOperatorKind.Char, //not used
        Float = UnaryOperatorKind.Float,
        Double = UnaryOperatorKind.Double,
        Decimal = UnaryOperatorKind.Decimal,
        Bool = UnaryOperatorKind.Bool,
        Object = UnaryOperatorKind._Object,
        String = UnaryOperatorKind._String,
        StringAndObject = UnaryOperatorKind._StringAndObject,
        ObjectAndString = UnaryOperatorKind._ObjectAndString,

        Enum = UnaryOperatorKind.Enum,
        EnumAndUnderlying = UnaryOperatorKind._EnumAndUnderlying,
        UnderlyingAndEnum = UnaryOperatorKind._UnderlyingAndEnum,
        Delegate = UnaryOperatorKind._Delegate,
        Pointer = UnaryOperatorKind.Pointer,
        PointerAndInt = UnaryOperatorKind._PointerAndInt,
        PointerAndUInt = UnaryOperatorKind._PointerAndUInt,
        PointerAndLong = UnaryOperatorKind._PointerAndLong,
        PointerAndULong = UnaryOperatorKind._PointerAndULong,
        IntAndPointer = UnaryOperatorKind._IntAndPointer,
        UIntAndPointer = UnaryOperatorKind._UIntAndPointer,
        LongAndPointer = UnaryOperatorKind._LongAndPointer,
        ULongAndPointer = UnaryOperatorKind._ULongAndPointer,
        NullableNull = UnaryOperatorKind._NullableNull,
        UserDefined = UnaryOperatorKind.UserDefined,
        Dynamic = UnaryOperatorKind.Dynamic,
        Unit = UnaryOperatorKind.Unit,

        OpMask = 0x0000FF00,
        Multiplication = 0x00001000,
        Addition = 0x00001100,
        Subtraction = 0x00001200,
        Division = 0x00001300,
        Remainder = 0x00001400,
        LeftShift = 0x00001500,
        RightShift = 0x00001600,
        Equal = 0x00001700,
        NotEqual = 0x00001800,
        GreaterThan = 0x00001900,
        LessThan = 0x00001A00,
        GreaterThanOrEqual = 0x00001B00,
        LessThanOrEqual = 0x00001C00,
        And = 0x00001D00,
        Xor = 0x00001E00,
        Or = 0x00001F00,

        Lifted = UnaryOperatorKind.Lifted,
        Logical = UnaryOperatorKind._Logical,
        Checked = UnaryOperatorKind.Checked,

        Error = 0x00000000,

        IntMultiplication = Int | Multiplication,
        UIntMultiplication = UInt | Multiplication,
        LongMultiplication = Long | Multiplication,
        ULongMultiplication = ULong | Multiplication,
        FloatMultiplication = Float | Multiplication,
        DoubleMultiplication = Double | Multiplication,
        DecimalMultiplication = Decimal | Multiplication,
        UserDefinedMultiplication = UserDefined | Multiplication,
        LiftedIntMultiplication = Lifted | Int | Multiplication,
        LiftedUIntMultiplication = Lifted | UInt | Multiplication,
        LiftedLongMultiplication = Lifted | Long | Multiplication,
        LiftedULongMultiplication = Lifted | ULong | Multiplication,
        LiftedFloatMultiplication = Lifted | Float | Multiplication,
        LiftedDoubleMultiplication = Lifted | Double | Multiplication,
        LiftedDecimalMultiplication = Lifted | Decimal | Multiplication,
        LiftedUserDefinedMultiplication = Lifted | UserDefined | Multiplication,
        DynamicMultiplication = Dynamic | Multiplication,
        UnitMultiplication = Unit | Multiplication,

        IntDivision = Int | Division,
        UIntDivision = UInt | Division,
        LongDivision = Long | Division,
        ULongDivision = ULong | Division,
        FloatDivision = Float | Division,
        DoubleDivision = Double | Division,
        DecimalDivision = Decimal | Division,
        UserDefinedDivision = UserDefined | Division,
        LiftedIntDivision = Lifted | Int | Division,
        LiftedUIntDivision = Lifted | UInt | Division,
        LiftedLongDivision = Lifted | Long | Division,
        LiftedULongDivision = Lifted | ULong | Division,
        LiftedFloatDivision = Lifted | Float | Division,
        LiftedDoubleDivision = Lifted | Double | Division,
        LiftedDecimalDivision = Lifted | Decimal | Division,
        LiftedUserDefinedDivision = Lifted | UserDefined | Division,
        DynamicDivision = Dynamic | Division,
        UnitDivision = Unit | Division,

        IntRemainder = Int | Remainder,
        UIntRemainder = UInt | Remainder,
        LongRemainder = Long | Remainder,
        ULongRemainder = ULong | Remainder,
        FloatRemainder = Float | Remainder,
        DoubleRemainder = Double | Remainder,
        DecimalRemainder = Decimal | Remainder,
        UserDefinedRemainder = UserDefined | Remainder,
        LiftedIntRemainder = Lifted | Int | Remainder,
        LiftedUIntRemainder = Lifted | UInt | Remainder,
        LiftedLongRemainder = Lifted | Long | Remainder,
        LiftedULongRemainder = Lifted | ULong | Remainder,
        LiftedFloatRemainder = Lifted | Float | Remainder,
        LiftedDoubleRemainder = Lifted | Double | Remainder,
        LiftedDecimalRemainder = Lifted | Decimal | Remainder,
        LiftedUserDefinedRemainder = Lifted | UserDefined | Remainder,
        DynamicRemainder = Dynamic | Remainder,

        IntAddition = Int | Addition,
        UIntAddition = UInt | Addition,
        LongAddition = Long | Addition,
        ULongAddition = ULong | Addition,
        FloatAddition = Float | Addition,
        DoubleAddition = Double | Addition,
        DecimalAddition = Decimal | Addition,
        EnumAndUnderlyingAddition = EnumAndUnderlying | Addition,
        UnderlyingAndEnumAddition = UnderlyingAndEnum | Addition,
        UserDefinedAddition = UserDefined | Addition,
        LiftedIntAddition = Lifted | Int | Addition,
        LiftedUIntAddition = Lifted | UInt | Addition,
        LiftedLongAddition = Lifted | Long | Addition,
        LiftedULongAddition = Lifted | ULong | Addition,
        LiftedFloatAddition = Lifted | Float | Addition,
        LiftedDoubleAddition = Lifted | Double | Addition,
        LiftedDecimalAddition = Lifted | Decimal | Addition,
        LiftedEnumAndUnderlyingAddition = Lifted | EnumAndUnderlying | Addition,
        LiftedUnderlyingAndEnumAddition = Lifted | UnderlyingAndEnum | Addition,
        LiftedUserDefinedAddition = Lifted | UserDefined | Addition,
        PointerAndIntAddition = PointerAndInt | Addition,
        PointerAndUIntAddition = PointerAndUInt | Addition,
        PointerAndLongAddition = PointerAndLong | Addition,
        PointerAndULongAddition = PointerAndULong | Addition,
        IntAndPointerAddition = IntAndPointer | Addition,
        UIntAndPointerAddition = UIntAndPointer | Addition,
        LongAndPointerAddition = LongAndPointer | Addition,
        ULongAndPointerAddition = ULongAndPointer | Addition,
        StringConcatenation = String | Addition,
        StringAndObjectConcatenation = StringAndObject | Addition,
        ObjectAndStringConcatenation = ObjectAndString | Addition,
        DelegateCombination = Delegate | Addition,
        DynamicAddition = Dynamic | Addition,

        IntSubtraction = Int | Subtraction,
        UIntSubtraction = UInt | Subtraction,
        LongSubtraction = Long | Subtraction,
        ULongSubtraction = ULong | Subtraction,
        FloatSubtraction = Float | Subtraction,
        DoubleSubtraction = Double | Subtraction,
        DecimalSubtraction = Decimal | Subtraction,
        EnumSubtraction = Enum | Subtraction,
        EnumAndUnderlyingSubtraction = EnumAndUnderlying | Subtraction,
        UnderlyingAndEnumSubtraction = UnderlyingAndEnum | Subtraction,
        UserDefinedSubtraction = UserDefined | Subtraction,
        LiftedIntSubtraction = Lifted | Int | Subtraction,
        LiftedUIntSubtraction = Lifted | UInt | Subtraction,
        LiftedLongSubtraction = Lifted | Long | Subtraction,
        LiftedULongSubtraction = Lifted | ULong | Subtraction,
        LiftedFloatSubtraction = Lifted | Float | Subtraction,
        LiftedDoubleSubtraction = Lifted | Double | Subtraction,
        LiftedDecimalSubtraction = Lifted | Decimal | Subtraction,
        LiftedEnumSubtraction = Lifted | Enum | Subtraction,
        LiftedEnumAndUnderlyingSubtraction = Lifted | EnumAndUnderlying | Subtraction,
        LiftedUnderlyingAndEnumSubtraction = Lifted | UnderlyingAndEnum | Subtraction,
        LiftedUserDefinedSubtraction = Lifted | UserDefined | Subtraction,
        DelegateRemoval = Delegate | Subtraction,
        PointerAndIntSubtraction = PointerAndInt | Subtraction,
        PointerAndUIntSubtraction = PointerAndUInt | Subtraction,
        PointerAndLongSubtraction = PointerAndLong | Subtraction,
        PointerAndULongSubtraction = PointerAndULong | Subtraction,
        PointerSubtraction = Pointer | Subtraction,
        DynamicSubtraction = Dynamic | Subtraction,

        IntLeftShift = Int | LeftShift,
        UIntLeftShift = UInt | LeftShift,
        LongLeftShift = Long | LeftShift,
        ULongLeftShift = ULong | LeftShift,
        UserDefinedLeftShift = UserDefined | LeftShift,
        LiftedIntLeftShift = Lifted | Int | LeftShift,
        LiftedUIntLeftShift = Lifted | UInt | LeftShift,
        LiftedLongLeftShift = Lifted | Long | LeftShift,
        LiftedULongLeftShift = Lifted | ULong | LeftShift,
        LiftedUserDefinedLeftShift = Lifted | UserDefined | LeftShift,
        DynamicLeftShift = Dynamic | LeftShift,

        IntRightShift = Int | RightShift,
        UIntRightShift = UInt | RightShift,
        LongRightShift = Long | RightShift,
        ULongRightShift = ULong | RightShift,
        UserDefinedRightShift = UserDefined | RightShift,
        LiftedIntRightShift = Lifted | Int | RightShift,
        LiftedUIntRightShift = Lifted | UInt | RightShift,
        LiftedLongRightShift = Lifted | Long | RightShift,
        LiftedULongRightShift = Lifted | ULong | RightShift,
        LiftedUserDefinedRightShift = Lifted | UserDefined | RightShift,
        DynamicRightShift = Dynamic | RightShift,

        IntEqual = Int | Equal,
        UIntEqual = UInt | Equal,
        LongEqual = Long | Equal,
        ULongEqual = ULong | Equal,
        FloatEqual = Float | Equal,
        DoubleEqual = Double | Equal,
        DecimalEqual = Decimal | Equal,
        BoolEqual = Bool | Equal,
        EnumEqual = Enum | Equal,
        NullableNullEqual = NullableNull | Equal,
        UserDefinedEqual = UserDefined | Equal,
        LiftedIntEqual = Lifted | Int | Equal,
        LiftedUIntEqual = Lifted | UInt | Equal,
        LiftedLongEqual = Lifted | Long | Equal,
        LiftedULongEqual = Lifted | ULong | Equal,
        LiftedFloatEqual = Lifted | Float | Equal,
        LiftedDoubleEqual = Lifted | Double | Equal,
        LiftedDecimalEqual = Lifted | Decimal | Equal,
        LiftedBoolEqual = Lifted | Bool | Equal,
        LiftedEnumEqual = Lifted | Enum | Equal,
        LiftedUserDefinedEqual = Lifted | UserDefined | Equal,
        ObjectEqual = Object | Equal,
        StringEqual = String | Equal,
        DelegateEqual = Delegate | Equal,
        PointerEqual = Pointer | Equal,
        DynamicEqual = Dynamic | Equal,

        IntNotEqual = Int | NotEqual,
        UIntNotEqual = UInt | NotEqual,
        LongNotEqual = Long | NotEqual,
        ULongNotEqual = ULong | NotEqual,
        FloatNotEqual = Float | NotEqual,
        DoubleNotEqual = Double | NotEqual,
        DecimalNotEqual = Decimal | NotEqual,
        BoolNotEqual = Bool | NotEqual,
        EnumNotEqual = Enum | NotEqual,
        NullableNullNotEqual = NullableNull | NotEqual,
        UserDefinedNotEqual = UserDefined | NotEqual,
        LiftedIntNotEqual = Lifted | Int | NotEqual,
        LiftedUIntNotEqual = Lifted | UInt | NotEqual,
        LiftedLongNotEqual = Lifted | Long | NotEqual,
        LiftedULongNotEqual = Lifted | ULong | NotEqual,
        LiftedFloatNotEqual = Lifted | Float | NotEqual,
        LiftedDoubleNotEqual = Lifted | Double | NotEqual,
        LiftedDecimalNotEqual = Lifted | Decimal | NotEqual,
        LiftedBoolNotEqual = Lifted | Bool | NotEqual,
        LiftedEnumNotEqual = Lifted | Enum | NotEqual,
        LiftedUserDefinedNotEqual = Lifted | UserDefined | NotEqual,
        ObjectNotEqual = Object | NotEqual,
        StringNotEqual = String | NotEqual,
        DelegateNotEqual = Delegate | NotEqual,
        PointerNotEqual = Pointer | NotEqual,
        DynamicNotEqual = Dynamic | NotEqual,

        IntLessThan = Int | LessThan,
        UIntLessThan = UInt | LessThan,
        LongLessThan = Long | LessThan,
        ULongLessThan = ULong | LessThan,
        FloatLessThan = Float | LessThan,
        DoubleLessThan = Double | LessThan,
        DecimalLessThan = Decimal | LessThan,
        EnumLessThan = Enum | LessThan,
        UserDefinedLessThan = UserDefined | LessThan,
        LiftedIntLessThan = Lifted | Int | LessThan,
        LiftedUIntLessThan = Lifted | UInt | LessThan,
        LiftedLongLessThan = Lifted | Long | LessThan,
        LiftedULongLessThan = Lifted | ULong | LessThan,
        LiftedFloatLessThan = Lifted | Float | LessThan,
        LiftedDoubleLessThan = Lifted | Double | LessThan,
        LiftedDecimalLessThan = Lifted | Decimal | LessThan,
        LiftedEnumLessThan = Lifted | Enum | LessThan,
        LiftedUserDefinedLessThan = Lifted | UserDefined | LessThan,
        PointerLessThan = Pointer | LessThan,
        DynamicLessThan = Dynamic | LessThan,

        IntGreaterThan = Int | GreaterThan,
        UIntGreaterThan = UInt | GreaterThan,
        LongGreaterThan = Long | GreaterThan,
        ULongGreaterThan = ULong | GreaterThan,
        FloatGreaterThan = Float | GreaterThan,
        DoubleGreaterThan = Double | GreaterThan,
        DecimalGreaterThan = Decimal | GreaterThan,
        EnumGreaterThan = Enum | GreaterThan,
        UserDefinedGreaterThan = UserDefined | GreaterThan,
        LiftedIntGreaterThan = Lifted | Int | GreaterThan,
        LiftedUIntGreaterThan = Lifted | UInt | GreaterThan,
        LiftedLongGreaterThan = Lifted | Long | GreaterThan,
        LiftedULongGreaterThan = Lifted | ULong | GreaterThan,
        LiftedFloatGreaterThan = Lifted | Float | GreaterThan,
        LiftedDoubleGreaterThan = Lifted | Double | GreaterThan,
        LiftedDecimalGreaterThan = Lifted | Decimal | GreaterThan,
        LiftedEnumGreaterThan = Lifted | Enum | GreaterThan,
        LiftedUserDefinedGreaterThan = Lifted | UserDefined | GreaterThan,
        PointerGreaterThan = Pointer | GreaterThan,
        DynamicGreaterThan = Dynamic | GreaterThan,

        IntLessThanOrEqual = Int | LessThanOrEqual,
        UIntLessThanOrEqual = UInt | LessThanOrEqual,
        LongLessThanOrEqual = Long | LessThanOrEqual,
        ULongLessThanOrEqual = ULong | LessThanOrEqual,
        FloatLessThanOrEqual = Float | LessThanOrEqual,
        DoubleLessThanOrEqual = Double | LessThanOrEqual,
        DecimalLessThanOrEqual = Decimal | LessThanOrEqual,
        EnumLessThanOrEqual = Enum | LessThanOrEqual,
        UserDefinedLessThanOrEqual = UserDefined | LessThanOrEqual,
        LiftedIntLessThanOrEqual = Lifted | Int | LessThanOrEqual,
        LiftedUIntLessThanOrEqual = Lifted | UInt | LessThanOrEqual,
        LiftedLongLessThanOrEqual = Lifted | Long | LessThanOrEqual,
        LiftedULongLessThanOrEqual = Lifted | ULong | LessThanOrEqual,
        LiftedFloatLessThanOrEqual = Lifted | Float | LessThanOrEqual,
        LiftedDoubleLessThanOrEqual = Lifted | Double | LessThanOrEqual,
        LiftedDecimalLessThanOrEqual = Lifted | Decimal | LessThanOrEqual,
        LiftedEnumLessThanOrEqual = Lifted | Enum | LessThanOrEqual,
        LiftedUserDefinedLessThanOrEqual = Lifted | UserDefined | LessThanOrEqual,
        PointerLessThanOrEqual = Pointer | LessThanOrEqual,
        DynamicLessThanOrEqual = Dynamic | LessThanOrEqual,

        IntGreaterThanOrEqual = Int | GreaterThanOrEqual,
        UIntGreaterThanOrEqual = UInt | GreaterThanOrEqual,
        LongGreaterThanOrEqual = Long | GreaterThanOrEqual,
        ULongGreaterThanOrEqual = ULong | GreaterThanOrEqual,
        FloatGreaterThanOrEqual = Float | GreaterThanOrEqual,
        DoubleGreaterThanOrEqual = Double | GreaterThanOrEqual,
        DecimalGreaterThanOrEqual = Decimal | GreaterThanOrEqual,
        EnumGreaterThanOrEqual = Enum | GreaterThanOrEqual,
        UserDefinedGreaterThanOrEqual = UserDefined | GreaterThanOrEqual,
        LiftedIntGreaterThanOrEqual = Lifted | Int | GreaterThanOrEqual,
        LiftedUIntGreaterThanOrEqual = Lifted | UInt | GreaterThanOrEqual,
        LiftedLongGreaterThanOrEqual = Lifted | Long | GreaterThanOrEqual,
        LiftedULongGreaterThanOrEqual = Lifted | ULong | GreaterThanOrEqual,
        LiftedFloatGreaterThanOrEqual = Lifted | Float | GreaterThanOrEqual,
        LiftedDoubleGreaterThanOrEqual = Lifted | Double | GreaterThanOrEqual,
        LiftedDecimalGreaterThanOrEqual = Lifted | Decimal | GreaterThanOrEqual,
        LiftedEnumGreaterThanOrEqual = Lifted | Enum | GreaterThanOrEqual,
        LiftedUserDefinedGreaterThanOrEqual = Lifted | UserDefined | GreaterThanOrEqual,
        PointerGreaterThanOrEqual = Pointer | GreaterThanOrEqual,
        DynamicGreaterThanOrEqual = Dynamic | GreaterThanOrEqual,

        IntAnd = Int | And,
        UIntAnd = UInt | And,
        LongAnd = Long | And,
        ULongAnd = ULong | And,
        EnumAnd = Enum | And,
        BoolAnd = Bool | And,
        UserDefinedAnd = UserDefined | And,
        LiftedIntAnd = Lifted | Int | And,
        LiftedUIntAnd = Lifted | UInt | And,
        LiftedLongAnd = Lifted | Long | And,
        LiftedULongAnd = Lifted | ULong | And,
        LiftedEnumAnd = Lifted | Enum | And,
        LiftedBoolAnd = Lifted | Bool | And,
        LiftedUserDefinedAnd = Lifted | UserDefined | And,
        DynamicAnd = Dynamic | And,

        LogicalAnd = And | Logical,
        LogicalBoolAnd = Bool | LogicalAnd,
        LogicalUserDefinedAnd = UserDefined | LogicalAnd,
        DynamicLogicalAnd = Dynamic | LogicalAnd,

        IntOr = Int | Or,
        UIntOr = UInt | Or,
        LongOr = Long | Or,
        ULongOr = ULong | Or,
        EnumOr = Enum | Or,
        BoolOr = Bool | Or,
        UserDefinedOr = UserDefined | Or,
        LiftedIntOr = Lifted | Int | Or,
        LiftedUIntOr = Lifted | UInt | Or,
        LiftedLongOr = Lifted | Long | Or,
        LiftedULongOr = Lifted | ULong | Or,
        LiftedEnumOr = Lifted | Enum | Or,
        LiftedBoolOr = Lifted | Bool | Or,
        LiftedUserDefinedOr = Lifted | UserDefined | Or,
        DynamicOr = Dynamic | Or,

        LogicalOr = Or | Logical,
        LogicalBoolOr = Bool | LogicalOr,
        LogicalUserDefinedOr = UserDefined | LogicalOr,
        DynamicLogicalOr = Dynamic | LogicalOr,

        IntXor = Int | Xor,
        UIntXor = UInt | Xor,
        LongXor = Long | Xor,
        ULongXor = ULong | Xor,
        EnumXor = Enum | Xor,
        BoolXor = Bool | Xor,
        UserDefinedXor = UserDefined | Xor,
        LiftedIntXor = Lifted | Int | Xor,
        LiftedUIntXor = Lifted | UInt | Xor,
        LiftedLongXor = Lifted | Long | Xor,
        LiftedULongXor = Lifted | ULong | Xor,
        LiftedEnumXor = Lifted | Enum | Xor,
        LiftedBoolXor = Lifted | Bool | Xor,
        LiftedUserDefinedXor = Lifted | UserDefined | Xor,
        DynamicXor = Dynamic | Xor,
    }
}

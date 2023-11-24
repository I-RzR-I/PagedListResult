// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-30 20:54
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 22:13
// ***********************************************************************
//  <copyright file="MemberExpressionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Extensions.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>MemberExpression extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class MemberExpressionExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build less than binary expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="compareObj">Compare object.</param>
        /// <returns>A BinaryExpression.</returns>
        /// =================================================================================================
        internal static BinaryExpression BuildLessThanBinaryExpression(
            this MemberExpression property,
            object compareObj)
        {
            BinaryExpression expression;
            if (property.Type == typeof(byte) || property.Type == typeof(byte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<byte?>> expressionValue = () => ((byte?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<byte>> expressionValue = () => (byte)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(sbyte) || property.Type == typeof(sbyte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<sbyte?>> expressionValue = () => ((sbyte?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<sbyte>> expressionValue = () => (sbyte)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(short) || property.Type == typeof(short?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<short?>> expressionValue = () => ((short?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<short>> expressionValue = () => (short)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ushort) || property.Type == typeof(ushort?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ushort?>> expressionValue = () => ((ushort?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ushort>> expressionValue = () => (ushort)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(int) || property.Type == typeof(int?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<int?>> expressionValue = () => ((int?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<int>> expressionValue = () => (int)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(uint) || property.Type == typeof(uint?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<uint?>> expressionValue = () => ((uint?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<uint>> expressionValue = () => (uint)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(long) || property.Type == typeof(long?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<long?>> expressionValue = () => ((long?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<long>> expressionValue = () => (long)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ulong) || property.Type == typeof(ulong?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ulong?>> expressionValue = () => ((ulong?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ulong>> expressionValue = () => (ulong)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(float) || property.Type == typeof(float?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<float?>> expressionValue = () => ((float?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<float>> expressionValue = () => (float)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(double) || property.Type == typeof(double?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<double?>> expressionValue = () => ((double?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<double>> expressionValue = () => (double)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(decimal) || property.Type == typeof(decimal?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<decimal?>> expressionValue = () => ((decimal?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<decimal>> expressionValue = () => (decimal)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(bool) || property.Type == typeof(bool?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<bool?>> expressionValue = () => ((bool?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<bool>> expressionValue = () => (bool)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(string))
            {
                Expression<Func<string>> expressionValue = () => (string)compareObj;
                expression = Expression.LessThan(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(char))
            {
                Expression<Func<char>> expressionValue = () => (char)compareObj;
                expression = Expression.LessThan(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(Guid) || property.Type == typeof(Guid?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<Guid?>> expressionValue = () => ((Guid?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<Guid>> expressionValue = () => (Guid)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTime) || property.Type == typeof(DateTime?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTime?>> expressionValue = () => ((DateTime?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTime>> expressionValue = () => (DateTime)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTimeOffset) || property.Type == typeof(DateTimeOffset?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTimeOffset?>> expressionValue = () => ((DateTimeOffset?)compareObj).Value;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTimeOffset>> expressionValue = () => (DateTimeOffset)compareObj;
                    expression = Expression.LessThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(byte[]))
            {
                Expression<Func<byte[]>> expressionValue = () => (byte[])compareObj;
                expression = Expression.LessThan(property, expressionValue.Body);

                return expression;
            }

            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build less than or equal binary expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="compareObj">Compare object.</param>
        /// <returns>A BinaryExpression.</returns>
        /// =================================================================================================
        internal static BinaryExpression BuildLessThanOrEqualBinaryExpression(
            this MemberExpression property,
            object compareObj)
        {
            BinaryExpression expression;
            if (property.Type == typeof(byte) || property.Type == typeof(byte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<byte?>> expressionValue = () => ((byte?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<byte>> expressionValue = () => (byte)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(sbyte) || property.Type == typeof(sbyte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<sbyte?>> expressionValue = () => ((sbyte?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<sbyte>> expressionValue = () => (sbyte)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(short) || property.Type == typeof(short?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<short?>> expressionValue = () => ((short?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<short>> expressionValue = () => (short)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ushort) || property.Type == typeof(ushort?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ushort?>> expressionValue = () => ((ushort?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ushort>> expressionValue = () => (ushort)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(int) || property.Type == typeof(int?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<int?>> expressionValue = () => ((int?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<int>> expressionValue = () => (int)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(uint) || property.Type == typeof(uint?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<uint?>> expressionValue = () => ((uint?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<uint>> expressionValue = () => (uint)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(long) || property.Type == typeof(long?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<long?>> expressionValue = () => ((long?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<long>> expressionValue = () => (long)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ulong) || property.Type == typeof(ulong?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ulong?>> expressionValue = () => ((ulong?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ulong>> expressionValue = () => (ulong)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(float) || property.Type == typeof(float?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<float?>> expressionValue = () => ((float?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<float>> expressionValue = () => (float)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(double) || property.Type == typeof(double?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<double?>> expressionValue = () => ((double?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<double>> expressionValue = () => (double)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(decimal) || property.Type == typeof(decimal?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<decimal?>> expressionValue = () => ((decimal?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<decimal>> expressionValue = () => (decimal)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(bool) || property.Type == typeof(bool?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<bool?>> expressionValue = () => ((bool?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<bool>> expressionValue = () => (bool)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(string))
            {
                Expression<Func<string>> expressionValue = () => (string)compareObj;
                expression = Expression.LessThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(char))
            {
                Expression<Func<char>> expressionValue = () => (char)compareObj;
                expression = Expression.LessThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(Guid) || property.Type == typeof(Guid?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<Guid?>> expressionValue = () => ((Guid?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<Guid>> expressionValue = () => (Guid)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTime) || property.Type == typeof(DateTime?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTime?>> expressionValue = () => ((DateTime?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTime>> expressionValue = () => (DateTime)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTimeOffset) || property.Type == typeof(DateTimeOffset?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTimeOffset?>> expressionValue = () => ((DateTimeOffset?)compareObj).Value;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTimeOffset>> expressionValue = () => (DateTimeOffset)compareObj;
                    expression = Expression.LessThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(byte[]))
            {
                Expression<Func<byte[]>> expressionValue = () => (byte[])compareObj;
                expression = Expression.LessThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build greater than or equal binary expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="compareObj">Compare object.</param>
        /// <returns>A BinaryExpression.</returns>
        /// =================================================================================================
        internal static BinaryExpression BuildGreaterThanOrEqualBinaryExpression(
            this MemberExpression property,
            object compareObj)
        {
            BinaryExpression expression;
            if (property.Type == typeof(byte) || property.Type == typeof(byte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<byte?>> expressionValue = () => ((byte?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<byte>> expressionValue = () => (byte)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(sbyte) || property.Type == typeof(sbyte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<sbyte?>> expressionValue = () => ((sbyte?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<sbyte>> expressionValue = () => (sbyte)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(short) || property.Type == typeof(short?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<short?>> expressionValue = () => ((short?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<short>> expressionValue = () => (short)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ushort) || property.Type == typeof(ushort?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ushort?>> expressionValue = () => ((ushort?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ushort>> expressionValue = () => (ushort)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(int) || property.Type == typeof(int?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<int?>> expressionValue = () => ((int?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<int>> expressionValue = () => (int)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(uint) || property.Type == typeof(uint?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<uint?>> expressionValue = () => ((uint?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<uint>> expressionValue = () => (uint)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(long) || property.Type == typeof(long?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<long?>> expressionValue = () => ((long?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<long>> expressionValue = () => (long)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ulong) || property.Type == typeof(ulong?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ulong?>> expressionValue = () => ((ulong?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ulong>> expressionValue = () => (ulong)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(float) || property.Type == typeof(float?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<float?>> expressionValue = () => ((float?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<float>> expressionValue = () => (float)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(double) || property.Type == typeof(double?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<double?>> expressionValue = () => ((double?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<double>> expressionValue = () => (double)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(decimal) || property.Type == typeof(decimal?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<decimal?>> expressionValue = () => ((decimal?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<decimal>> expressionValue = () => (decimal)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(bool) || property.Type == typeof(bool?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<bool?>> expressionValue = () => ((bool?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<bool>> expressionValue = () => (bool)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(string))
            {
                Expression<Func<string>> expressionValue = () => (string)compareObj;
                expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(char))
            {
                Expression<Func<char>> expressionValue = () => (char)compareObj;
                expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(Guid) || property.Type == typeof(Guid?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<Guid?>> expressionValue = () => ((Guid?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<Guid>> expressionValue = () => (Guid)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTime) || property.Type == typeof(DateTime?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTime?>> expressionValue = () => ((DateTime?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTime>> expressionValue = () => (DateTime)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTimeOffset) || property.Type == typeof(DateTimeOffset?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTimeOffset?>> expressionValue = () => ((DateTimeOffset?)compareObj).Value;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTimeOffset>> expressionValue = () => (DateTimeOffset)compareObj;
                    expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(byte[]))
            {
                Expression<Func<byte[]>> expressionValue = () => (byte[])compareObj;
                expression = Expression.GreaterThanOrEqual(property, expressionValue.Body);

                return expression;
            }

            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build greater than binary expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="compareObj">Compare object.</param>
        /// <returns>A BinaryExpression.</returns>
        /// =================================================================================================
        internal static BinaryExpression BuildGreaterThanBinaryExpression(
            this MemberExpression property,
            object compareObj)
        {
            BinaryExpression expression;
            if (property.Type == typeof(byte) || property.Type == typeof(byte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<byte?>> expressionValue = () => ((byte?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<byte>> expressionValue = () => (byte)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(sbyte) || property.Type == typeof(sbyte?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<sbyte?>> expressionValue = () => ((sbyte?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<sbyte>> expressionValue = () => (sbyte)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(short) || property.Type == typeof(short?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<short?>> expressionValue = () => ((short?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<short>> expressionValue = () => (short)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ushort) || property.Type == typeof(ushort?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ushort?>> expressionValue = () => ((ushort?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ushort>> expressionValue = () => (ushort)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(int) || property.Type == typeof(int?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<int?>> expressionValue = () => ((int?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<int>> expressionValue = () => (int)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(uint) || property.Type == typeof(uint?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<uint?>> expressionValue = () => ((uint?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<uint>> expressionValue = () => (uint)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(long) || property.Type == typeof(long?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<long?>> expressionValue = () => ((long?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<long>> expressionValue = () => (long)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(ulong) || property.Type == typeof(ulong?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<ulong?>> expressionValue = () => ((ulong?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<ulong>> expressionValue = () => (ulong)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(float) || property.Type == typeof(float?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<float?>> expressionValue = () => ((float?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<float>> expressionValue = () => (float)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(double) || property.Type == typeof(double?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<double?>> expressionValue = () => ((double?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<double>> expressionValue = () => (double)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(decimal) || property.Type == typeof(decimal?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<decimal?>> expressionValue = () => ((decimal?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<decimal>> expressionValue = () => (decimal)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(bool) || property.Type == typeof(bool?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<bool?>> expressionValue = () => ((bool?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<bool>> expressionValue = () => (bool)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(string))
            {
                Expression<Func<string>> expressionValue = () => (string)compareObj;
                expression = Expression.GreaterThan(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(char))
            {
                Expression<Func<char>> expressionValue = () => (char)compareObj;
                expression = Expression.GreaterThan(property, expressionValue.Body);

                return expression;
            }

            if (property.Type == typeof(Guid) || property.Type == typeof(Guid?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<Guid?>> expressionValue = () => ((Guid?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<Guid>> expressionValue = () => (Guid)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTime) || property.Type == typeof(DateTime?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTime?>> expressionValue = () => ((DateTime?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTime>> expressionValue = () => (DateTime)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(DateTimeOffset) || property.Type == typeof(DateTimeOffset?))
            {
                if (property.Type.IsNullablePropType())
                {
                    Expression<Func<DateTimeOffset?>> expressionValue = () => ((DateTimeOffset?)compareObj).Value;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }
                else
                {
                    Expression<Func<DateTimeOffset>> expressionValue = () => (DateTimeOffset)compareObj;
                    expression = Expression.GreaterThan(property, expressionValue.Body);
                }

                return expression;
            }

            if (property.Type == typeof(byte[]))
            {
                Expression<Func<byte[]>> expressionValue = () => (byte[])compareObj;
                expression = Expression.GreaterThan(property, expressionValue.Body);

                return expression;
            }

            return null;
        }
    }
}
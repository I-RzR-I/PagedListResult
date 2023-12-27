// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-12-26 23:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-27 02:54
// ***********************************************************************
//  <copyright file="XmlExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System.Collections.Generic;
using System.Xml;

#endregion

namespace PagedListResult.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>An XML extensions.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    internal static class XmlExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>A T extension method that converts this object to a SOAP XML response.</summary>
        /// <remarks></remarks>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <returns>An XmlElement.</returns>
        /// =================================================================================================
        internal static XmlElement CastToSoapXmlResponse<T>(this IList<T> source)
        {
            var doc = source.SerializeToXmlDoc("SoapXmlPagedResultResponse", "PagedListResult.SoapXmlPagedResult");

            return doc.GetXmlElement();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A T extension method that converts this object to a SOAP XML response.</summary>
        /// <remarks></remarks>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="root">The root.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>An XmlElement.</returns>
        /// =================================================================================================
        internal static XmlElement CastToSoapXmlResponse<T>(this T source, string root, string nameSpace)
        {
            var doc = source.SerializeToXmlDoc(root, nameSpace);

            return doc.GetXmlElement();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>An XmlDocument extension method that gets XML element.</summary>
        /// <remarks></remarks>
        /// <param name="document">The document to act on.</param>
        /// <returns>The XML element.</returns>
        /// =================================================================================================
        internal static XmlElement GetXmlElement(this XmlDocument document)
            => document.IsNotNull() && document.DocumentElement.IsNotNull()
                ? document.DocumentElement
                : null;
    }
}
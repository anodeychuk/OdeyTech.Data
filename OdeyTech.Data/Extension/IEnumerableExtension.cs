// --------------------------------------------------------------------------
// <copyright file="IEnumerableExtension.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using OdeyTech.Data.Model.Interface;
using OdeyTech.ProductivityKit.Extension;

namespace OdeyTech.Data.Extension
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> collection) where T : IModel
        {
            var newCollection = new HashSet<T>();
            collection.ForEach(p => newCollection.Add((T)p.Clone()));
            return newCollection;
        }
    }
}

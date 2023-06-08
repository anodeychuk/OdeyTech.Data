// --------------------------------------------------------------------------
// <copyright file="IDependentModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

namespace OdeyTech.Data.Model.Interface
{
    /// <summary>
    /// Model that depends on another model.
    /// </summary>
    public interface IDependentModel : IModel
    {
        /// <summary>
        /// Identifier of the parent model.
        /// </summary>
        ulong ParentIdentifier { get; set; }
    }
}

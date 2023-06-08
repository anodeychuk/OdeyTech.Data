// --------------------------------------------------------------------------
// <copyright file="IModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;

namespace OdeyTech.Data.Model.Interface
{
    /// <summary>
    /// Model that supports cloning and copying functionality.
    /// </summary>
    public interface IModel : ICloneable
    {
        /// <summary>
        /// Identifier of the model.
        /// </summary>
        ulong Identifier { get; }

        /// <summary>
        /// Copies the properties of another model into this one.
        /// </summary>
        /// <param name="model">The model to copy from.</param>
        void CopyFrom(IModel model);
    }
}

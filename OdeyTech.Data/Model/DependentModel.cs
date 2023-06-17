// --------------------------------------------------------------------------
// <copyright file="DependentModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Model
{
    /// <summary>
    /// Model that depend on another model.
    /// </summary>
    public abstract class DependentModel : BasicModel, IDependentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependentModel"/> class.
        /// </summary>
        public DependentModel() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependentModel"/> class with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the model.</param>
        public DependentModel(ulong identifier) : base(identifier)
        { }

        /// <inheritdoc/>
        public ulong ParentIdentifier { get; set; }

        /// <summary>
        /// Copies the properties of the specified object to this dependent model.
        /// </summary>
        /// <param name="obj">The object to copy from.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not a dependent model.</exception>
        public override void CopyFrom(IModel obj)
        {
            if (obj is not IDependentModel parent)
            {
                throw new ArgumentException("Cannot copy from an object that is not a dependent model.", nameof(obj));
            }

            base.CopyFrom(obj);
            ParentIdentifier = parent.ParentIdentifier;
        }

        /// <summary>
        /// Joins the specified model as a dependent of this model.
        /// </summary>
        /// <param name="obj">The model to join.</param>
        protected void Join(IDependentModel obj) => obj.ParentIdentifier = Identifier;
    }
}

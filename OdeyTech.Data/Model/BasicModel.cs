// --------------------------------------------------------------------------
// <copyright file="BasicModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Model
{
    /// <summary>
    /// Model that implements the <see cref="IModel"/> interface and provides common functionality.
    /// </summary>
    public abstract class BasicModel : ObservableObject, IModel
    {
        private static ulong lastIdentifier = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicModel"/> class.
        /// </summary>
        public BasicModel()
        {
            Identifier = GetNewIdentifier();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicModel"/> class with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the model.</param>
        public BasicModel(ulong identifier)
        {
            UpdateIdentifierCounter(identifier);
            Identifier = identifier;
        }

        /// <inheritdoc/>
        public ulong Identifier { get; private set; }

        /// <inheritdoc/>
        public object Clone()
        {
            var obj = (BasicModel)Activator.CreateInstance(GetType(), new object[] { Identifier });
            obj.CopyFrom(this);
            return obj;
        }

        /// <summary>
        /// Copies the properties of the specified object to this model.
        /// </summary>
        /// <param name="obj">The object to copy from.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not a model.</exception>
        public virtual void CopyFrom(IModel obj)
        {
            if (obj is not IModel)
            {
                throw new ArgumentException("Cannot copy from an object that is not a model.", nameof(obj));
            }

            Identifier = obj.Identifier;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IModel model && Identifier == model.Identifier;

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();

        private ulong GetNewIdentifier() => ++lastIdentifier;

        private void UpdateIdentifierCounter(ulong identifier)
        {
            if (lastIdentifier < identifier)
            {
                lastIdentifier = identifier;
            }
        }
    }
}

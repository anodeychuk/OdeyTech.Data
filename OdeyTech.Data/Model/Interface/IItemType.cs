// --------------------------------------------------------------------------
// <copyright file="IItemType.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

namespace OdeyTech.Data.Model.Interface
{
    /// <summary>
    /// Item type model.
    /// </summary>
    public interface IItemType : IModel
    {
        /// <summary>
        /// Display name of the item type.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Tool tip of the item type.
        /// </summary>
        string ToolTip { get; }
    }
}

// <copyright file="IEditAction.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface to hold all of the editing actions able to be performed.
    /// </summary>
    public interface IEditAction
    {
        /// <summary>
        /// The redo action for editing.
        /// </summary>
        void Redo();

        /// <summary>
        /// The undo action for editing.
        /// </summary>
        void Undo();

        /// <summary>
        /// Get the description for an action.
        /// </summary>
        /// <returns>The action description.</returns>
        string GetActDesc();
    }
}

// <copyright file="UndoRedoCollection.cs" company="Josh Abbott">
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
    /// A class to help create actions and store information.
    /// </summary>
    public class UndoRedoCollection
    {
        private Stack<IEditAction> undoActions = new Stack<IEditAction>();
        private Stack<IEditAction> redoActions = new Stack<IEditAction>();

        /// <summary>
        /// Creates a new action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddAction(IEditAction action)
        {
            this.undoActions.Push(action);
            this.redoActions.Clear();
        }

        /// <summary>
        /// A function to undo the recent action.
        /// </summary>
        public void Undo()
        {
            if (this.undoActions.Count > 0)
            {
                IEditAction act = this.undoActions.Pop();
                act.Undo();
                this.redoActions.Push(act);
            }
        }

        /// <summary>
        /// A function to redo the recent action.
        /// </summary>
        public void Redo()
        {
            if (this.redoActions.Count > 0)
            {
                IEditAction act = this.redoActions.Pop();
                act.Redo();
                this.undoActions.Push(act);
            }
        }
    }
}

﻿using System;
using System.ComponentModel;

namespace KPreisser.UI
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class TaskDialogControl
    {
        // Disallow inheritance by specifying a private protected constructor.
        private protected TaskDialogControl()
            : base()
        {
        }


        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get;
            set;
        }


        internal TaskDialogContents BoundTaskDialogContents
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates if the current state of this control
        /// allows it to be created in a task dialog when binding it.
        /// </summary>
        internal virtual bool IsCreatable
        {
            get => true;
        }

        /// <summary>
        /// Gets or sets a value that indicates if this control has been created
        /// in a bound task dialog.
        /// </summary>
        internal bool IsCreated
        {
            get;
            private set;
        }


        internal TaskDialogFlags Bind(TaskDialogContents contents)
        {
            this.BoundTaskDialogContents = contents ??
                    throw new ArgumentNullException(nameof(contents));

            // Use the current value of IsCreatable to determine if the control is
            // created. This is important because IsCreatable can change while the
            // control is displayed (e.g. if it depends on the Text property).
            this.IsCreated = this.IsCreatable;

            return this.IsCreated ? this.GetFlagsCore() : default;
        }

        internal virtual void Unbind()
        {
            this.IsCreated = false;
            this.BoundTaskDialogContents = null;
        }

        /// <summary>
        /// Applies initialization after the task dialog is displayed or navigated.
        /// </summary>
        internal void ApplyInitialization()
        {
            // Only apply the initialization if the control is actually created.
            if (this.IsCreated)
                this.ApplyInitializationCore();
        }


        /// <summary>
        /// When overridden in a subclass, gets additional flags to be specified before
        /// the task dialog is displayed or navigated.
        /// </summary>
        /// <remarks>
        /// This method will only be called if <see cref="IsCreatable"/> returns <c>true</c>.
        /// </remarks>
        /// <returns></returns>
        private protected virtual TaskDialogFlags GetFlagsCore()
        {
            return default;
        }

        /// <summary>
        /// When overridden in a subclass, applies initialization after the task dialog
        /// is displayed or navigated.
        /// </summary>
        /// <remarks>
        /// This method will only be called if <see cref="IsCreatable"/> returns <c>true</c>.
        /// </remarks>
        private protected virtual void ApplyInitializationCore()
        {
        }

        private protected void DenyIfBound()
        {
            this.BoundTaskDialogContents?.DenyIfBound();
        }

        private protected void DenyIfNotBound()
        {
            if (this.BoundTaskDialogContents == null)
                throw new InvalidOperationException(
                        "This control is not currently bound to a task dialog.");
        }

        private protected void DenyIfBoundAndNotCreated()
        {
            if (this.BoundTaskDialogContents != null && !this.IsCreated)
                throw new InvalidOperationException("The control has not been created.");
        }
    }
}

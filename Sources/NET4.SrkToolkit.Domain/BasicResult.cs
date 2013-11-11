﻿// -----------------------------------------------------------------------
// <copyright file="BasicResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    public class BasicResult
    {
        private IList<BasicResultError> errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IList<BasicResultError> Errors
        {
            get { return this.errors ?? (this.errors = new List<BasicResultError>()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeed { get; set; }
    }
}

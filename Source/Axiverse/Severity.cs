using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Log severity.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// The system is unusable.
        /// </summary>
        Emergency = 1,

        /// <summary>
        /// Action must be taken immediately
        /// </summary>
        Alert = 2,

        /// <summary>
        /// Critical conditions.
        /// </summary>
        Critical = 3,

        /// <summary>
        /// Error conditions.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Warning conditions.
        /// </summary>
        Warning = 5,

        /// <summary>
        /// Normal but significant conditions.
        /// </summary>
        Notice = 6,

        /// <summary>
        /// Informational messages.
        /// </summary>
        Informational = 6,

        /// <summary>
        /// Debug level messages.
        /// </summary>
        Debug = 7,
    }
}

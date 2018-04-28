using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Logging class
    /// </summary>
    public sealed class Logger
    {
        /// <summary>
        /// Logs an error value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public void LogError(
            object value,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var frame = new StackFrame(1, false);
            Log(value.ToString(), Severity.Error, frame, filePath, lineNumber);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public void LogError(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var frame = new StackFrame(1, false);
            Log(message, Severity.Error, frame, filePath, lineNumber);
        }

        /// <summary>
        /// Logs an error message given the a condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public void LogErrorIf(
            bool condition,
            object value,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (condition)
            {
                var frame = new StackFrame(1, false);
                Log(value.ToString(), Severity.Error, frame, filePath, lineNumber);
            }
        }

        /// <summary>
        /// Logs an error message given the a condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public void LogErrorIf(
            bool condition,
            string message,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (condition)
            {
                var frame = new StackFrame(1, false);
                Log(message, Severity.Error, frame, filePath, lineNumber);
            }
        }
        
        private void Log(
            string message,
            Severity severity,
            StackFrame frame,
            string filePath,
            int lineNumber)
        {
            var method = frame.GetMethod();
            var className = method.DeclaringType.Name;
            var methodName = method.Name;

            var log = $"{message}\n\t{className}.{methodName} @ {filePath}:{lineNumber}";
            Debug.WriteLine(log);
        }
    }
}

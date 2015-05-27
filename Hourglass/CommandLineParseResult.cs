// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineParseResult.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// The type of <see cref="CommandLineParseResult"/>.
    /// </summary>
    public enum CommandLineParseResultType
    {
        /// <summary>
        /// Indicates that the command-line arguments were parsed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates that the command-line arguments were not parsed successfully.
        /// </summary>
        Failure,

        /// <summary>
        /// Indicates that the user has requested command-line usage to be shown.
        /// </summary>
        UsageRequested
    }

    /// <summary>
    /// The result of calling <see cref="CommandLineArguments.Parse"/>.
    /// </summary>
    public class CommandLineParseResult
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CommandLineParseResult"/> class from being created.
        /// </summary>
        private CommandLineParseResult()
        {
        }

        /// <summary>
        /// Gets the type of result.
        /// </summary>
        public CommandLineParseResultType Type { get; private set; }

        /// <summary>
        /// Gets parsed command-line arguments.
        /// </summary>
        public CommandLineArguments Arguments { get; private set; }

        /// <summary>
        /// Gets an error message if the command-line arguments were not successfully parsed, or <c>null</c> otherwise.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Returns a new instance of the <see cref="CommandLineParseResult"/> class for a <see
        /// cref="CommandLineArguments"/> instance.
        /// </summary>
        /// <param name="arguments">The parsed command-line arguments.</param>
        /// <returns>A new instance of the <see cref="CommandLineParseResult"/> class.</returns>
        public static CommandLineParseResult ForCommandLineArguments(CommandLineArguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            return new CommandLineParseResult
            {
                Type = CommandLineParseResultType.Success,
                Arguments = arguments
            };
        }

        /// <summary>
        /// Returns a new instance of the <see cref="CommandLineParseResult"/> class for an <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/>.</param>
        /// <returns>A new instance of the <see cref="CommandLineParseResult"/> class.</returns>
        public static CommandLineParseResult ForException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return new CommandLineParseResult
            {
                Type = CommandLineParseResultType.Failure,
                ErrorMessage = exception.Message
            };
        }

        /// <summary>
        /// Returns a new instance of the <see cref="CommandLineParseResult"/> class for a help request.
        /// </summary>
        /// <returns>A new instance of the <see cref="CommandLineParseResult"/> class.</returns>
        public static CommandLineParseResult ForHelpRequest()
        {
            return new CommandLineParseResult
            {
                Type = CommandLineParseResultType.UsageRequested
            };
        }
    }
}

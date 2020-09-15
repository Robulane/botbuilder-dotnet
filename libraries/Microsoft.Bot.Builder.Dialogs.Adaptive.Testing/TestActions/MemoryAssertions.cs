﻿// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Testing.TestActions
{
    /// <summary>
    /// Run assertions against memory.
    /// </summary>
    public class MemoryAssertions : TestAction
    {
        /// <summary>
        /// Kind for the serialization.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.Test.MemoryAssertions";

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryAssertions"/> class.
        /// </summary>
        /// <param name="path">Path to source.</param>
        /// <param name="line">Line number in source.</param>
        [JsonConstructor]
        public MemoryAssertions([CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            RegisterSourcePath(path, line);
        }

        /// <summary>
        /// Gets the assertions.
        /// </summary>
        /// <value>The assertion expressions.</value>
        [JsonProperty("assertions")]
        public List<string> Assertions { get; } = new List<string>();

        /// <inheritdoc/>
        public async override Task ExecuteAsync(TestAdapter adapter, BotCallbackHandler callback)
        {
            var activity = new Activity();
            activity.ApplyConversationReference(adapter.Conversation, isIncoming: true);
            activity.Type = "event";
            activity.Name = "MemoryAssertions";
            activity.Value = Assertions;
            await adapter.ProcessActivityAsync(activity, callback, default).ConfigureAwait(false);
            Trace.TraceInformation($"[Turn Ended => MemoryAssertions passed");
        }
    }
}

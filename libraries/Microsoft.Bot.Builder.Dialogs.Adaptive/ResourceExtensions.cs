﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive
{
    /// <summary>
    /// Implements extension methods for resource explorer.
    /// </summary>
    public static class ResourceExtensions
    {
        /// <summary>
        /// Register ResourceExplorer and optionally register more types.
        /// </summary>
        /// <param name="dialogManager">BotAdapter to add middleware to.</param>
        /// <param name="resourceExplorer">resourceExplorer to use.</param>
        /// <returns>The bot adapter.</returns>
        public static DialogManager UseResourceExplorer(this DialogManager dialogManager, ResourceExplorer resourceExplorer)
        {
            if (resourceExplorer == null)
            {
                throw new ArgumentNullException(nameof(resourceExplorer));
            }

            dialogManager.InitialTurnState.Add(resourceExplorer);
            if (dialogManager.RootDialog != null)
            {
                var visitedDialogs = new List<string>() { dialogManager.RootDialog.Id };
                dialogManager.AddReferencedDialog(dialogManager.RootDialog, resourceExplorer, visitedDialogs);
            }
            
            return dialogManager;
        }

        private static DialogManager AddReferencedDialog(this DialogManager dialogManager, Dialog dialog, ResourceExplorer resourceExplorer, List<string> visited)
        {
            if (dialog is DialogContainer container)
            {
                foreach (var refDialog in container.RefereceDialogs)
                {
                    if (!visited.Contains(refDialog))
                    {
                        visited.Add(refDialog);
                        var resource = resourceExplorer.GetResource($"{refDialog}.dialog");
                        var loadedDialog = resourceExplorer.LoadType<Dialog>(resource);
                        dialogManager.Dialogs.Add(loadedDialog);
                        AddReferencedDialog(dialogManager, loadedDialog, resourceExplorer, visited);
                    }
                }
            }

            return dialogManager;
        }
    }
}

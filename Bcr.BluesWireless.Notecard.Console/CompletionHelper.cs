using PrettyPrompt;
using PrettyPrompt.Completion;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;

namespace Bcr.BluesWireless.Notecard.Console;

public class CompletionHelper
{
    class NotecardCompletionItem
    {
        public string? Request { get; init; }
        public string? Json { get; init; }
        public string? Description { get; init; }
    }

    static List<NotecardCompletionItem> _notecardCompletionItems = new () {
        new NotecardCompletionItem { Request = "card.status", Json = "{\"req\":\"card.status\"}", Description = "Returns general information about the Notecard's operating status." },
        new NotecardCompletionItem { Request = "card.temp", Json = "{\"req\":\"card.temp\"}", Description = "Get the current temperature from the Notecard's onboard calibrated temperature sensor." },
        new NotecardCompletionItem { Request = "note.add" },
        new NotecardCompletionItem { Request = "note.changes" },
    };

    static List<CompletionItem> _requestCompletionItems = new ();

    private class NotecardPromptCallbacks : PromptCallbacks
    {
        protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<CompletionItem>>(_requestCompletionItems);
        }
    }

    static void BuildRequestCompletionItems()
    {
        foreach (var item in _notecardCompletionItems)
        {
            _requestCompletionItems.Add(new CompletionItem(
                item.Json ?? item.Request ?? "", item.Request, getExtendedDescription: ((item.Description != null) ? ((_) => Task.FromResult<FormattedString>(item.Description)) : null)
            ));
        }
    }

    public static PromptCallbacks GetPromptCallbacks()
    {
        if (_requestCompletionItems.Count == 0)
        {
            BuildRequestCompletionItems();
        }
        return new NotecardPromptCallbacks();
    }
}

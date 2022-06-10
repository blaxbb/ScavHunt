using Markdig;
using Microsoft.AspNetCore.Components;

namespace ScavHunt.Data.Services
{
    public class MarkdownService
    {
        MarkdownPipeline pipeline { get; }
        public MarkdownService()
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseEmojiAndSmiley(false)
                .UseAdvancedExtensions()
                .UseBootstrap()
                .Build();
        }

        public MarkupString Render(string text)
        {
            return (MarkupString)Markdown.ToHtml(text, pipeline);
        }
    }
}

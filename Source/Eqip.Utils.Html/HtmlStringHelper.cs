using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Eqip.Utils.Html
{
    public class HtmlStringHelper
    {
        private HtmlDocument _document { get; set; }
        private List<string> WhiteListedTags { get; set; }
        private Dictionary<string, bool> BlackListedTags { get; set; }
        private List<string> WhiteListedAttributes { get; set; }
        private List<string> BlackListedAttributes { get; set; }

        private bool _removeComments { get; set; }
        private bool _removeNonPrintableCharacters { get; set; }
        private bool _removeJavaScript { get; set; }
        private bool _removeMultipleSpaces { get; set; }


        public HtmlStringHelper(string source)
        {
            this.WhiteListedTags = new List<string>();
            this.BlackListedTags = new Dictionary<string, bool>();
            this.WhiteListedAttributes = new List<string>();
            this.BlackListedAttributes = new List<string>();
            _document = GetHtml(source);

        }

        #region Remove Methods
        public HtmlStringHelper RemoveComments()
        {
            return RemoveComments(true);
        }

        public HtmlStringHelper RemoveComments(bool condition)
        {
            this._removeComments = condition;
            return this;
        }

        public HtmlStringHelper RemoveNonPrintableCharacters()
        {
            return RemoveNonPrintableCharacters(true);
        }

        public HtmlStringHelper RemoveNonPrintableCharacters(bool condition)
        {
            this._removeNonPrintableCharacters = condition;
            return this;
        }

        public HtmlStringHelper RemoveMultipleSpaces()
        {
            return RemoveMultipleSpaces(true);
        }

        public HtmlStringHelper RemoveMultipleSpaces(bool condition)
        {
            this._removeMultipleSpaces = condition;
            return this;
        }

        public HtmlStringHelper RemoveJavaScript()
        {
            return RemoveJavaScript(true);
        }

        public HtmlStringHelper RemoveJavaScript(bool condition)
        {
            this._removeJavaScript = condition;
            return this;
        }
        #endregion


        public HtmlStringHelper StripTags()
        {
            return this;
        }

        public HtmlStringHelper StripTags(IEnumerable<string> whitelist)
        {
            WhiteListedTags.AddRange(whitelist);
            return this;
        }

        public HtmlStringHelper StripTags(IEnumerable<string> whitelist, IEnumerable<string> blacklist)
        {
            WhiteListedTags.AddRange(whitelist);
            foreach (var b in blacklist)
            {
                BlackListedTags.Add(b, true);
            }
            return this;
        }

        public HtmlStringHelper StripAttributes()
        {
            return this;
        }

        public HtmlStringHelper StripAttributes(IEnumerable<string> whitelist)
        {
            WhiteListedAttributes.AddRange(whitelist);
            return this;
        }

        public HtmlStringHelper StripAttributes(IEnumerable<string> whitelist, IEnumerable<string> blacklist)
        {
            WhiteListedAttributes.AddRange(whitelist);
            BlackListedAttributes.AddRange(blacklist);
            return this;
        }


        public override string ToString()
        {
            SanitizeNode(_document.DocumentNode);
            var output = _removeNonPrintableCharacters ? RemoveNonPrintableCharacters(_document.DocumentNode.WriteTo()) : _document.DocumentNode.WriteTo();
            output = _removeMultipleSpaces ? RemoveMultipleSpaces(output) : output;
            return output;
        }

        private void SanitizeNode(HtmlNode node)
        {
            RemoveComments(node);
            if (node.NodeType == HtmlNodeType.Element)
            {
                RemoveTags(node);
                RemoveAttributes(node);
                RemoveJavaScript(node);
            }

            // Look through child nodes recursively
            if (node.HasChildNodes)
            {
                for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                {
                    SanitizeNode(node.ChildNodes[i]);
                }
            }

        }

        private void RemoveTags(HtmlNode node)
        {
            if (BlackListedTags.ContainsKey(node.Name) || !WhiteListedTags.Contains(node.Name))
            {
                if (!node.HasChildNodes)
                {
                    node.ParentNode.RemoveChild(node);
                }
                else
                {
                    for (var i = node.ChildNodes.Count - 1; i >= 0; i--)
                    {
                        var child = node.ChildNodes[i];
                        node.ParentNode.InsertAfter(child, node);
                    }
                    node.ParentNode.RemoveChild(node);
                }
            }
        }

        private void RemoveAttributes(HtmlNode node)
        {
            if (!node.HasAttributes)
            {
                return;
            }

            for (int i = node.Attributes.Count - 1; i >= 0; i--)
            {
                HtmlAttribute currentAttribute = node.Attributes[i];
                var attr = currentAttribute.Name.ToLower();
                if (this.BlackListedAttributes.Contains(attr) || !WhiteListedAttributes.Contains(attr))
                {
                    node.Attributes.Remove(currentAttribute);
                }
            }
        }

        private void RemoveComments(HtmlNode node)
        {
            if (_removeComments)
            {
                if (node.NodeType == HtmlNodeType.Comment)
                {
                    var parentNode = node.ParentNode;
                    node.Remove();
                    if (parentNode.Attributes.Count == 0 && (parentNode.InnerText == null || parentNode.InnerText == string.Empty))
                    {
                        parentNode.Remove();
                    }
                    return;
                }
            }
        }

        private void RemoveJavaScript(HtmlNode node)
        {
            if (_removeJavaScript)
            {
                // remove CSS Expressions and embedded script links
                if (node.Name == "style")
                {
                    if (string.IsNullOrEmpty(node.InnerText))
                    {
                        if (node.InnerHtml.Contains("expression") || node.InnerHtml.Contains("javascript:"))
                            node.ParentNode.RemoveChild(node);
                    }
                }

                // remove script attributes
                if (node.HasAttributes)
                {

                    for (int i = node.Attributes.Count - 1; i >= 0; i--)
                    {
                        HtmlAttribute currentAttribute = node.Attributes[i];

                        var attr = currentAttribute.Name.ToLower();
                        var val = currentAttribute.Value.ToLower();

                        if (this.BlackListedAttributes.Contains(attr))
                        {
                            node.Attributes.Remove(currentAttribute);
                            continue;
                        }

                        // remove event handlers
                        if (attr.StartsWith("on"))
                            node.Attributes.Remove(currentAttribute);

                        // remove script links
                        else if ((attr == "href" || attr == "src" || attr == "dynsrc" || attr == "lowsrc") &&
                                 val != null &&
                                 val.Contains("javascript:"))
                            node.Attributes.Remove(currentAttribute);

                        // Remove CSS Expressions
                        else if (attr == "style" &&
                                 val != null &&
                                 val.Contains("expression") || val.Contains("javascript:") || val.Contains("vbscript:"))
                            node.Attributes.Remove(currentAttribute);
                    }

                }
            }

        }

        private string RemoveNonPrintableCharacters(string text)
        {
            return Regex.Replace(text, "[\n\r\t\f]", " ");
        }

        private string RemoveMultipleSpaces(string text)
        {
            var output = Regex.Replace(text, @"&nbsp;", " ");
            return Regex.Replace(output, @"[ ]{2,}", @" ", RegexOptions.None);
        }

        /// <summary>
        /// Helper function that returns an HTML document from text
        /// </summary>
        private static HtmlDocument GetHtml(string source)
        {
            HtmlDocument html = new HtmlDocument();
            html.OptionFixNestedTags = true;
            html.OptionAutoCloseOnEnd = true;
            html.OptionDefaultStreamEncoding = Encoding.UTF8;

            html.LoadHtml(source);

            return html;
        }
    }
}

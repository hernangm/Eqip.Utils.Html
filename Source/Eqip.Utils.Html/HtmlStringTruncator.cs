using System;
using System.Xml;

namespace Eqip.Utils.Html
{
    public static class HtmlTruncatorStringExtensions
    {

        public static string LimitHtmlOnWordBoundary(this string str, int maxLength, string ellipses = "...")
        {
            XmlDocument doc = new XmlDocument();

            XmlParserContext context = new XmlParserContext(doc.NameTable, new XmlNamespaceManager(doc.NameTable), null, XmlSpace.Preserve);
            XmlTextReader reader = new XmlTextReader("<xml>" + str + "</xml>", XmlNodeType.Document, context);
            bool shouldWriteEllipses;
            using (var writer = doc.CreateNavigator().AppendChild())
            {
                LimitHtmlOnWordBoundary(writer, reader, maxLength, out shouldWriteEllipses);
                writer.Flush();
            }

            return doc.DocumentElement.InnerXml + (shouldWriteEllipses ? ellipses : "");
        }

        private static void LimitHtmlOnWordBoundary(XmlWriter writer, XmlReader reader, int maxLength, out bool shouldWriteEllipses)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            int elementCount = 0;

            int currentLength = 0;
            shouldWriteEllipses = false;

            int magicMinimumLength = Math.Min(5, (maxLength + 1) / 2);

            int num = (reader.NodeType == XmlNodeType.None) ? -1 : reader.Depth;
            do
            {
                bool done = false;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        elementCount++;
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        writer.WriteAttributes(reader, false);
                        if (reader.IsEmptyElement)
                        {
                            elementCount--;
                            writer.WriteEndElement();
                        }
                        break;

                    case XmlNodeType.Text:
                        string value = reader.Value;
                        int strLen = value.Length;

                        if (currentLength + strLen > maxLength)
                        {
                            string almost = value.Substring(0, maxLength - currentLength + 1);
                            int lastSpace = almost.LastIndexOf(' ');
                            if (lastSpace < 0)
                            {
                                if (currentLength < magicMinimumLength)
                                {
                                    value = value.Substring(0, maxLength - currentLength);
                                }
                                else
                                {
                                    value = null;
                                }
                            }
                            else if (lastSpace + currentLength < magicMinimumLength)
                            {
                                value = value.Substring(0, maxLength - currentLength);
                            }
                            else
                            {
                                value = value.Substring(0, lastSpace);
                            }
                            shouldWriteEllipses = true;
                            done = true;
                        }
                        if (value != null)
                        {
                            writer.WriteString(value);
                            currentLength += value.Length;
                        }
                        break;

                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        writer.WriteString(reader.Value);
                        currentLength += reader.Value.Length;
                        break;

                    case XmlNodeType.EndElement:
                        elementCount--;
                        writer.WriteFullEndElement();
                        break;

                    case XmlNodeType.CDATA:
                        //writer.WriteCData(reader.Value);
                        break;

                    case XmlNodeType.EntityReference:
                        writer.WriteEntityRef(reader.Name);
                        currentLength++;
                        break;

                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.XmlDeclaration:
                        //writer.WriteProcessingInstruction(reader.Name, reader.Value);
                        break;

                    case XmlNodeType.Comment:
                        //writer.WriteComment(reader.Value);
                        break;

                    case XmlNodeType.DocumentType:
                        //writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
                        break;
                }
                if (done) break;
            }
            while (reader.Read() && ((num < reader.Depth) || ((num == reader.Depth) && (reader.NodeType == XmlNodeType.EndElement))));

            while (elementCount > 0)
            {
                writer.WriteFullEndElement();
                elementCount--;
            }
        }

    }
}

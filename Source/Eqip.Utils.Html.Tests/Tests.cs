using System;
using Eqip.Utils.Html;
using NUnit.Framework;

namespace Eqip.Utils.Html.tests
{
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var expected = "Introducción: ";
            var test = @"<p><strong>Introducci&oacute;n</strong>: </p>";
            var replaced = test.AsHtmlString().RemoveComments().RemoveJavaScript().RemoveNonPrintableCharacters().RemoveMultipleSpaces().Encode().ToString();
            Assert.AreEqual(expected, replaced);
        }
    }
}

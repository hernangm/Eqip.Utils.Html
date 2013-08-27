
namespace Eqip.Utils.Html
{
    public static class HtmlStringExtensions
    {

        public static HtmlStringHelper AsHtmlString(this string str)
        {
            return new HtmlStringHelper(str);
        }

    }
}

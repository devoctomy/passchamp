using Microsoft.VisualBasic;

namespace devoctomy.Passchamp.Maui.Services;

public class XamlHelperService : IXamlHelperService
{
    public Page GetParentPage(Element element)
    {
        while (element != null)
        {
            if (element is Page page)
            {
                return page;
            }

            element = element.Parent;
        }

        return null;
    }
}

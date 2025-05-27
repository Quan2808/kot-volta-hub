using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Configuration;

// Custom convention to apply /api prefix to all controllers
public class ApiPrefixConvention(string prefix) : IApplicationModelConvention
{
    private readonly string _prefix = prefix.Trim('/'); // Ensure no leading/trailing slashes

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    // Combine existing route with prefix
                    selector.AttributeRouteModel.Template =
                        AttributeRouteModel.CombineTemplates(_prefix, selector.AttributeRouteModel.Template);
                }
                else
                {
                    // Apply prefix to controllers without explicit routes
                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = $"{_prefix}/[controller]"
                    };
                }
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace F2.Testing
{
    /// <summary>
    /// Code of https://github.com/kobake/AspNetCore.RouteAnalyzer/blob/01f0194f15e724ddbbb4a670e5f94f73cf313b40/AspNetCore.RouteAnalyzer/Inner/RouteAnalyzerImpl.cs
    /// </summary>
    public class RouteAnalyzer
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public RouteAnalyzer(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IActionResult Retrieve<TController>(Expression<Func<TController, IActionResult>> navigationPropertyPath) where TController : ControllerBase
            => Retrieve<TController, IActionResult>(navigationPropertyPath);

        /// <summary>
        /// WIP
        /// </summary>
        public TProperty Retrieve<TController, TProperty>(Expression<Func<TController, TProperty>> navigationPropertyPath) where TController : ControllerBase
        {
            if (navigationPropertyPath is null)
            {
                throw new ArgumentNullException(nameof(navigationPropertyPath));
            }

            // https://stackoverflow.com/a/47031653/8801075

            var controller = navigationPropertyPath.Parameters[0];
            var method = navigationPropertyPath.Body.GetType();

            return default;
        }

        public IEnumerable<RouteInformation> GetAllRouteInformations()
        {
            var ret = new List<RouteInformation>();

            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items;
            foreach (ActionDescriptor actionDescriptor in routes)
            {
                var info = new RouteInformation();

                // Area
                if (actionDescriptor.RouteValues.ContainsKey("area"))
                {
                    info.Area = actionDescriptor.RouteValues["area"];
                }

                // Path and Invocation of Razor Pages
                if (actionDescriptor is PageActionDescriptor e)
                {
                    info.Path = e.ViewEnginePath;
                    info.Invocation = e.RelativePath;
                }

                // Path of Route Attribute
                if (actionDescriptor.AttributeRouteInfo != null)
                {
                    info.Path = $"/{actionDescriptor.AttributeRouteInfo.Template}";
                }

                // Path and Invocation of Controller/Action
                if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    if (string.IsNullOrEmpty(info.Path))
                    {
                        info.Path = $"/{controllerActionDescriptor.ControllerName}/{controllerActionDescriptor.ActionName}";
                    }

                    info.Invocation = $"{controllerActionDescriptor.ControllerName}Controller.{controllerActionDescriptor.ActionName}";
                }

                // Extract HTTP Verb
                if (actionDescriptor.ActionConstraints != null && actionDescriptor.ActionConstraints.Select(t => t.GetType()).Contains(typeof(HttpMethodActionConstraint)))
                {
                    var httpMethodAction = actionDescriptor.ActionConstraints.FirstOrDefault(a => a.GetType() == typeof(HttpMethodActionConstraint)) as HttpMethodActionConstraint;

                    if (httpMethodAction != null)
                    {
                        info.HttpMethod = string.Join(",", httpMethodAction.HttpMethods);
                    }
                }

                // Additional information of invocation
                info.Invocation += $" ({actionDescriptor.DisplayName})";

                // Generating List
                ret.Add(info);
            }

            // Result
            return ret;
        }
    }
}
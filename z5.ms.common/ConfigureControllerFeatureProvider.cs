using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace z5.ms.common
{
    //TODO consider making into interface
    /// <summary> FeatureProvider that can be used to add template controllers.</summary>
    public class ConfigureControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IList<TypeInfo> _controllers;

        /// <inheritdoc/>
        public ConfigureControllerFeatureProvider(IList<TypeInfo> controllers = null)
        {
            controllers = controllers ?? new List<TypeInfo>();
            _controllers = controllers;
        }

        /// <summary> Adds controller. </summary>
        /// <param name="containsCheck">Perform contains check before adding</param>
        public void AddController<TController>(bool containsCheck = false) where TController : Controller
        {
            if (containsCheck && !ContainsController<TController>() || !containsCheck)
                _controllers.Add(typeof(TController).GetTypeInfo());
        }
        
        /// <inheritdoc />
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.Clear();
            foreach (var controller in _controllers)
                feature.Controllers.Add(controller);
        }
        
        private bool ContainsController<TController>() where TController : Controller
        {
            return _controllers.Contains(typeof(TController).GetTypeInfo());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CronQuery.Mvc.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace CronQuery.API.Mvc.Jobs
{
    public sealed class JobCollection : IEnumerable<ServiceDescriptor>
    {
        private readonly IList<ServiceDescriptor> _descriptors;

        public JobCollection(IList<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _descriptors
                .Where(service => typeof(IJob).IsAssignableFrom(service.ServiceType))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

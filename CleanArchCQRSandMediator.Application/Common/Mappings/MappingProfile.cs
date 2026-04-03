using AutoMapper;
using System.Reflection;

namespace CleanArchCQRSandMediator.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            // Gets all types that implement IMapFrom<>
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");

                // Invokes the Mapping method of each DTO, passing it this profile
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}

using AutoMapper;

namespace CleanArchCQRSandMediator.Application.Common.Mappings
{
    /// <summary>
    /// profile.CreateMap(typeof(T), GetType()), which registers a mapping from type T to the type of the class that implements the interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}

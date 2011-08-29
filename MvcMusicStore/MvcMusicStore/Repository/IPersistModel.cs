using System.Collections.Generic;
using MvcMusicStore.Helpers;
using MvcMusicStore.Models;

namespace MvcMusicStore.Repository
{
    public interface IPersistModel<T> where T : class, ICanBeStored
    {
        void Store(T model);
        void Store(List<T> models);
        void Delete(string id);
        Maybe<T> Load(string id);
        List<T> LoadAll();
        List<Maybe<T>> Load(IEnumerable<string> ids);
    }
}
using System.Collections.Generic;
using System.Linq;
using MvcMusicStoreAdfs.Helpers;
using MvcMusicStoreAdfs.Models;
using Raven.Client;

namespace MvcMusicStoreAdfs.Repository
{
    public class RavenPersister<T> : IPersistModel<T> where T : class, ICanBeStored
    {
        private IDocumentSession _session;

        public RavenPersister(IDocumentSession session)
        {
            _session = session;
        }
        public void Store(T model)
        {
            var existingModel = Load(model.Id);
            if (existingModel.HasValue)
            {
                _session.Advanced.Evict(existingModel.Value);
            }
            _session.Store(model);
            _session.SaveChanges();
            WaitForIndexUpdate();
        }

        public void Store(List<T> models)
        {
            var existingModels = LoadAllAsIEnumerable().Where( m =>  (models.Select(model => model.Name).Contains(m.Name))).ToList();

            foreach (var model in models.Where(model => !existingModels.Select(m => m.Name).Contains(model.Name)))
            {
                _session.Store(model);
            }
            _session.SaveChanges();
            WaitForIndexUpdate();
        }

        public void Delete(string id)
        {
            var model = Load(id);
            if (model.ValueMissing)
            {
                return;
            }
            _session.Delete(model.Value);
            _session.SaveChanges();
        }

        public Maybe<T> Load(string id)
        {
            return new Maybe<T>(_session.Load<T>(id));
        }

        public List<Maybe<T>> Load(IEnumerable<string> ids)
        {
            return _session.Load<T>(ids).Select(model => new Maybe<T>(model)).ToList();
        }

        public virtual List<T> LoadAll()
        {
            return _session.Query<T>().ToList();
        }

        public IEnumerable<T> LoadAllAsIEnumerable()
        {
            return _session.Query<T>();
        }

        private void WaitForIndexUpdate()
        {
            _session.Query<T>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).ToList();
        }
    }
}
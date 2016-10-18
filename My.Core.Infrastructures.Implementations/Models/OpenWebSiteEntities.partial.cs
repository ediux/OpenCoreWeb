using My.Core.Infrastructures.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class OpenWebSiteEntities : IUnitofWork
    {
        private bool _requireuniqueEmail;
        public bool RequireUniqueEmail { get { return _requireuniqueEmail; } set { _requireuniqueEmail = value; } }

        private bool _disposed;
        private Hashtable _repositories;

        public static OpenWebSiteEntities Create()
        {
            return new OpenWebSiteEntities();
        }

        public System.Data.Entity.IDbSet<TEntity> GetEntity<TEntity>() where TEntity : class
        {
            return this.GetEntity<TEntity>();
        }

        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(IRepositoryBase<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(TEntity)), this);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryBase<TEntity>)_repositories[type];
        }

    }
}

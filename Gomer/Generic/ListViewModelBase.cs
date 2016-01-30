using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Gomer.DataAccess;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.Generic
{
    public class ListViewModelBase<TRepository, TModel> : BindableBase
        where TRepository : IRepository<TModel>
        where TModel : ModelBase<TModel>
    {
        protected TRepository Repository { get; private set; }

        public ObservableCollection<TModel> Models { get; private set; }
        
        public RelayCommand<TModel> OpenCommand { get; set; }

        public ListViewModelBase(TRepository repository)
        {
            Repository = repository;
            Models = new ObservableCollection<TModel>();

            OpenCommand = new RelayCommand<TModel>(OpenCommandImpl);
        }

        public void Refresh()
        {
            RefreshLookupData();
            RefreshData();
        }

        public virtual void RefreshLookupData()
        {
        }

        public virtual void RefreshData()
        {
            Models.Clear();
            var models = Repository.GetAll().ToList();
            foreach (var model in models)
            {
                Models.Add(model);
            }
        }

        #region Events

        public event EventHandler<ModelEventArgs<TModel>> Open;

        private void OnOpening(TModel model)
        {
            var args = new ModelEventArgs<TModel> { Model = model };
            if (Open != null)
            {
                Open(this, args);
            }
        }

        #endregion

        private void OpenCommandImpl(TModel model)
        {
            OnOpening(model);
        }

        public virtual bool Filter(TModel model)
        {
            return true;
        }
    }
}

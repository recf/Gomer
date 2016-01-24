using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;

namespace Gomer.Generic
{
    public class ListViewModelBase<TModel> : ViewModelBase
    {
        public ObservableCollection<TModel> Models { get; private set; }

        private ObservableCollection<TModel> _filteredModels;
        public ReadOnlyObservableCollection<TModel> FilteredModels { get; private set; }
        
        public RelayCommand<TModel> OpenCommand { get; set; }

        public ListViewModelBase(ObservableCollection<TModel> models)
        {
            Models = models;

            _filteredModels = new ObservableCollection<TModel>(Models);
            FilteredModels = new ReadOnlyObservableCollection<TModel>(_filteredModels);

            OpenCommand = new RelayCommand<TModel>(OpenCommandImpl);

            ApplyFilter();
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

        public void ApplyFilter()
        {
            foreach (var model in Models)
            {
                var shouldBeInFilter = Filter(model);

                var inFiltered = FilteredModels.Contains(model);

                if (shouldBeInFilter && !inFiltered)
                {
                    _filteredModels.Add(model);
                }

                if (!shouldBeInFilter && inFiltered)
                {
                    _filteredModels.Remove(model);
                }
            }
        }
    }
}

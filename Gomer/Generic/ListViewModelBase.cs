using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;

namespace Gomer.Generic
{
    public class ListViewModelBase<TModel> : ViewModelBase
    {
        public ObservableCollection<TModel> Models { get; private set; }

        public RelayCommand<TModel> OpenCommand { get; set; }

        public ListViewModelBase(ObservableCollection<TModel> models)
        {
            Models = models;

            OpenCommand = new RelayCommand<TModel>(OpenCommandImpl);
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
    }
}

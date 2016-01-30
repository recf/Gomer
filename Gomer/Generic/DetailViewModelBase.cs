using System;
using System.ComponentModel;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.Generic
{
    public class DetailViewModelBase<TModel> : BindableBase
        where TModel : ModelBase<TModel>, new()
    {
        private TModel _model;
        public virtual TModel Model
        {
            get { return _model; }
            set
            {
                if (_model != null)
                {
                    _model.ErrorsChanged -= Model_ErrorsChanged;
                }

                TModel workingCopy = null;

                if (value != null)
                {
                    workingCopy = new TModel();
                    workingCopy.SetFrom(value);

                    workingCopy.ErrorsChanged += Model_ErrorsChanged;
                }

                SetProperty(ref _model, workingCopy);
                OnModelChanged();
            }
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        public DetailViewModelBase()
        {
            CancelCommand = new RelayCommand(OnCanceled);
            UpdateCommand = new RelayCommand(UpdateCommandImpl, () => Model != null && !Model.HasErrors);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
        }

        #region Events

        public event EventHandler<ModelEventArgs<TModel>> ModelChanged;

        private void OnModelChanged()
        {
            if (ModelChanged != null)
            {
                var args = new ModelEventArgs<TModel>()
                {
                    Model = Model
                };

                ModelChanged(this, args);
            }
        }

        public event EventHandler<ModelEventArgs<TModel>> Updated;

        private void OnUpdated()
        {
            if (Updated != null)
            {
                var args = new ModelEventArgs<TModel>()
                {
                    Model = Model
                };

                Updated(this, args);
            }
        }

        public event EventHandler<ModelEventArgs<TModel>> Removed;

        private void OnRemoved()
        {
            if (Removed != null)
            {
                var args = new ModelEventArgs<TModel>()
                {
                    Model = Model
                };

                Removed(this, args);
            }
        }

        public event EventHandler Canceled;

        private void OnCanceled()
        {
            if (Canceled != null)
            {
                Canceled(this, new EventArgs());
            }
        }

        #endregion
        
        private void UpdateCommandImpl()
        {
            OnUpdated();
        }

        private void RemoveCommandImpl()
        {
            OnRemoved();
        }
        
        private void Model_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            UpdateCommand.RaiseCanExecuteChanged();
        }

        public virtual void RefreshLookupData()
        {
        }
    }
}

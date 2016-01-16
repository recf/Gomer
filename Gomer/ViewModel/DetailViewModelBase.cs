using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;

namespace Gomer.ViewModel
{
    public class DetailViewModelBase<TModel> : ViewModelBase
        where TModel : new()
    {
        private TModel _model;
        public TModel Model
        {
            get { return _model; }
            set
            {
                var workingCopy = new TModel();

                Mapper.Map(value, workingCopy);

                Set(() => Model, ref _model, workingCopy);
            }
        }
        
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        public DetailViewModelBase()
        {
            CancelCommand = new RelayCommand(OnCanceled);
            UpdateCommand = new RelayCommand(UpdateCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
        }
        
        #region Events

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
    }
}

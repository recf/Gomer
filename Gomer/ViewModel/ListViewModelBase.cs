using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.ViewModel
{
    public class ListViewModelBase<TModel> : ViewModelBase
    {
        public ObservableCollection<TModel> Models { get; private set; }

        public RelayCommand<TModel> OpenCommand { get; set; }

        public ListViewModelBase()
        {
            Models = new ObservableCollection<TModel>();

            OpenCommand = new RelayCommand<TModel>(OpenCommandImpl);
        }

        public void Reset(ICollection<TModel> models)
        {
            Models.Clear();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.Generic
{
    public abstract class IndexViewModelBase<TModel, TModelList, TModelDetail> : ViewModelBase
        where TModel : ModelBase, new()
        where TModelList : ListViewModelBase<TModel>, new()
        where TModelDetail: DetailViewModelBase<TModel>, new()
    {

        private ICollection<TModel> _models;
        public ICollection<TModel> Models
        {
            get { return _models; }
            set
            {
                Set(() => Models, ref _models, value);
                List = new TModelList();
                List.Reset(Models);
            }
        }

        private TModelList _list;
        public TModelList List
        {
            get { return _list; }
            set
            {
                if (_list != null)
                {
                    _list.Open -= List_OnOpen;
                }
                Set(() => List, ref _list, value);
                List.Open += List_OnOpen;
            }
        }

        public bool HasSelectedDetail
        {
            get { return SelectedDetail != null; }
        }

        private TModelDetail _selectedDetail;
        public TModelDetail SelectedDetail
        {
            get { return _selectedDetail; }
            set
            {
                if (_selectedDetail != null)
                {
                    SelectedDetail.Canceled -= SelectedDetail_OnCanceled;
                    SelectedDetail.Updated -= SelectedDetail_OnUpdated;
                    SelectedDetail.Removed -= SelectedDetail_OnRemoved;
                }

                Set(() => SelectedDetail, ref _selectedDetail, value);
                RaisePropertyChanged(() => HasSelectedDetail);
                NewCommand.RaiseCanExecuteChanged();

                if (_selectedDetail != null)
                {
                    SelectedDetail.Canceled += SelectedDetail_OnCanceled;
                    SelectedDetail.Updated += SelectedDetail_OnUpdated;
                    SelectedDetail.Removed += SelectedDetail_OnRemoved;

                    InitializeSelectedDetail(SelectedDetail);
                }
            }
        }

        public virtual void InitializeSelectedDetail(TModelDetail selectedDetail)
        {
        }

        public RelayCommand NewCommand { get; set; }

        protected IndexViewModelBase()
        {
            NewCommand = new RelayCommand(NewCommandImpl, () => !HasSelectedDetail);
        }

        #region Events

        public event EventHandler DataChanged;

        private void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        private void NewCommandImpl()
        {
            Open(new TModel());
        }

        private void Open(TModel model)
        {
            SelectedDetail = new TModelDetail()
            {
                Model = model
            };
        }

        #region Event Handlers
        
        private void List_OnOpen(object sender, ModelEventArgs<TModel> e)
        {
            Open(e.Model);
        }

        private void SelectedDetail_OnCanceled(object sender, EventArgs e)
        {
            SelectedDetail = null;
        }

        private void SelectedDetail_OnUpdated(object sender, ModelEventArgs<TModel> e)
        {
            var existing = Models.FirstOrDefault(m => m.Id == e.Model.Id);
            if (existing != null)
            {
                Models.Remove(existing);
            }
            
            Models.Add(e.Model);
            List.Reset(Models);

            SelectedDetail = null;
            OnDataChanged();
        }

        private void SelectedDetail_OnRemoved(object sender, ModelEventArgs<TModel> e)
        {
            var existing = Models.FirstOrDefault(m => m.Id == e.Model.Id);
            if (existing != null)
            {
                Models.Remove(existing);
            }

            SelectedDetail = null;
            List.Reset(Models);
            OnDataChanged();
        }

        #endregion
    }
}
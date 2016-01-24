using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.Generic
{
    public abstract class IndexViewModelBase<TModel, TModelList, TModelDetail> : ViewModelBase
        where TModel : ModelBase<TModel>, new()
        where TModelList : ListViewModelBase<TModel>
        where TModelDetail: DetailViewModelBase<TModel>
    {
        public ObservableCollection<TModel> Models { get; private set; }

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

        private TModelDetail _selectedDetail;
        public TModelDetail SelectedDetail
        {
            get { return _selectedDetail; }
            set
            {
                if (_selectedDetail != null)
                {
                    SelectedDetail.ModelChanged -= SelectedDetail_ModelChanged;
                    SelectedDetail.Canceled -= SelectedDetail_OnCanceled;
                    SelectedDetail.Updated -= SelectedDetail_OnUpdated;
                    SelectedDetail.Removed -= SelectedDetail_OnRemoved;
                }

                Set(() => SelectedDetail, ref _selectedDetail, value);

                if (_selectedDetail != null)
                {
                    SelectedDetail.ModelChanged += SelectedDetail_ModelChanged;
                    SelectedDetail.Canceled += SelectedDetail_OnCanceled;
                    SelectedDetail.Updated += SelectedDetail_OnUpdated;
                    SelectedDetail.Removed += SelectedDetail_OnRemoved;
                }
            }
        }

        public RelayCommand NewCommand { get; set; }

        protected IndexViewModelBase(ObservableCollection<TModel> models, TModelList list, TModelDetail detail)
        {
            Models = models;
            List = list;
            SelectedDetail = detail;

            NewCommand = new RelayCommand(NewCommandImpl, () => SelectedDetail.Model == null);
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
            SelectedDetail.Model = model;
        }

        #region Event Handlers
        
        private void List_OnOpen(object sender, ModelEventArgs<TModel> e)
        {
            Open(e.Model);
        }

        #endregion

        #region Detail Event Handlers

        void SelectedDetail_ModelChanged(object sender, ModelEventArgs<TModel> e)
        {
            NewCommand.RaiseCanExecuteChanged();
        }

        private void SelectedDetail_OnCanceled(object sender, EventArgs e)
        {
            SelectedDetail.Model = null;
        }

        private void SelectedDetail_OnUpdated(object sender, ModelEventArgs<TModel> e)
        {
            var existing = Models.FirstOrDefault(m => m.Id == e.Model.Id);
            if (existing== null)
            {
                Models.Add(e.Model);
            }
            else
            {
                existing.SetFrom(e.Model);
            }

            SelectedDetail.Model = null;
            OnDataChanged();
        }

        private void SelectedDetail_OnRemoved(object sender, ModelEventArgs<TModel> e)
        {
            var existing = Models.FirstOrDefault(m => m.Id == e.Model.Id);
            if (existing != null)
            {
                Models.Remove(existing);
            }

            SelectedDetail.Model = null;
            OnDataChanged();
        }

        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Gomer.DataAccess;
using Gomer.Events;
using Gomer.Models;

namespace Gomer.Generic
{
    public abstract class IndexViewModelBase<TRepository, TModel, TModelList, TModelDetail> : ViewModelBase
        where TRepository : IRepository<TModel>
        where TModel : ModelBase<TModel>, new()
        where TModelList : ListViewModelBase<TRepository, TModel>
        where TModelDetail: DetailViewModelBase<TModel>
    {
        protected TRepository Repository { get; private set; }

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
                SetProperty(ref _list, value);
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

                SetProperty(ref _selectedDetail, value);

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

        protected IndexViewModelBase(TRepository repository, TModelList list, TModelDetail detail)
        {
            Repository = repository;
            List = list;
            SelectedDetail = detail;

            NewCommand = new RelayCommand(NewCommandImpl, () => SelectedDetail.Model == null);
        }
        
        public override void Refresh()
        {
            List.Refresh();
            SelectedDetail.Refresh();
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

        public virtual bool SupportsNew
        {
            get { return true; }
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
            if (e.Model.IsNew)
            {
                Repository.Add(e.Model);
            }
            else
            {
                Repository.Update(e.Model);
            }

            SelectedDetail.Model = null;
            List.Refresh();

            OnDataChanged();
        }

        private void SelectedDetail_OnRemoved(object sender, ModelEventArgs<TModel> e)
        {
            Repository.Remove(e.Model);

            SelectedDetail.Model = null;
            List.Refresh();

            OnDataChanged();
        }

        #endregion
    }
}
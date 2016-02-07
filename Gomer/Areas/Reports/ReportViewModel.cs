﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.DataAccess;
using Gomer.Models;

namespace Gomer.Areas.Reports
{
    public class ReportViewModel : BindableBase
    {
        private readonly IGameRepository _gamesRepository;

        private ReportTimeFrame _selectedTimeFrame;
        public ReportTimeFrame SelectedTimeFrame
        {
            get { return _selectedTimeFrame; }
            set
            {
                SetProperty(ref _selectedTimeFrame, value);
            }
        }

        public ObservableCollection<ReportTimeFrame> TimeFrames { get; private set; }
        
        private string _reportText;

        public string ReportText
        {
            get { return _reportText; }
            set
            {
                SetProperty(ref _reportText, value);
            }
        }

        public RelayCommand RunReportCommand { get; private set; }

        public ReportViewModel(IGameRepository gamesGamesRepository)
        {
            _gamesRepository = gamesGamesRepository;

            TimeFrames = new ObservableCollection<ReportTimeFrame>();

            var today = DateTime.Today;
            var startOfMonth = today.AddDays(-today.Day + 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var startOfYear = startOfMonth.AddMonths(-startOfMonth.Month + 1);
            var endOfYear = startOfYear.AddYears(1).AddDays(-1);

            var tfThisMonth = new ReportTimeFrame
            {
                Name = "This Month",
                StartOn = startOfMonth,
                EndOn = endOfMonth
            };

            var tfLastMonth = new ReportTimeFrame
            {
                Name = "Last Month",
                StartOn = startOfMonth.AddMonths(-1),
                EndOn = startOfMonth.AddDays(-1)
            };

            var tfThisYear = new ReportTimeFrame
            {
                Name = "This Year",
                StartOn = startOfYear,
                EndOn = endOfYear
            };

            var tfLastYear = new ReportTimeFrame
            {
                Name = "Last Year",
                StartOn = startOfYear.AddYears(-1),
                EndOn = startOfYear.AddDays(-1)
            };

            SelectedTimeFrame = tfThisMonth;

            TimeFrames.Add(tfThisMonth);
            TimeFrames.Add(tfLastMonth);
            TimeFrames.Add(tfThisYear);
            TimeFrames.Add(tfLastYear);

            RunReportCommand = new RelayCommand(RunReportImpl);
        }

        private void RunReportImpl()
        {
            var sb = new StringBuilder();

            var added = _gamesRepository.Find(x => InTimeFrame(x.AddedOn, SelectedTimeFrame)).ToList();
            
            AppendList(sb, "Added", added);

            ReportText = sb.ToString();
        }

        private static bool InTimeFrame(DateTime? date, ReportTimeFrame timeFrame)
        {
            if (!date.HasValue)
            {
                return false;
            }

            return timeFrame.StartOn <= date.Value && timeFrame.EndOn >= date.Value;
        }
        
        private static void AppendList(StringBuilder sb, string title, List<GameModel> gameModels)
        {
            sb.AppendLine(string.Format("[b]{0}[/b]", title));

            foreach (var game in gameModels)
            {
                var platforms = string.Join(", ", game.Platforms.Select(p => p.Name));
                var line = string.Format("[*] {0} ({1})", game.Name, platforms);

                sb.AppendLine(line);
            }

            sb.AppendLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public class StatusHistoryModel : ModelBase<StatusHistoryModel>
    {
        private StatusModel _status;
        [Required]
        public StatusModel Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private DateTime _statusDate;
        public DateTime StatusDate
        {
            get { return _statusDate; }
            set
            {
                SetProperty(ref _statusDate, value);
            }
        }

        public override void SetFrom(StatusHistoryModel other)
        {
            Id = other.Id;
            Status = other.Status;
            StatusDate = other.StatusDate;
        }
    }
}

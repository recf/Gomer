using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Areas.Reports
{
    public class ReportTimeFrame : BindableBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private DateTime _startOn;
        public DateTime StartOn
        {
            get { return _startOn; }
            set
            {
                SetProperty(ref _startOn, value);
            }
        }

        private DateTime _endOn;
        public DateTime EndOn
        {
            get { return _endOn; }
            set
            {
                SetProperty(ref _endOn, value);
            }
        }
    }
}

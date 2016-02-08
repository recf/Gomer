using System;
using Newtonsoft.Json;

namespace Gomer.ImportCsv
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GrouveeCsvDatesField
    {
        private string _dateStartedValue;
        [JsonProperty("date_started", Required = Required.Always)]
        public string DateStartedValue
        {
            get { return _dateStartedValue; }
            set
            {
                _dateStartedValue = value;

                DateStarted = null;
                DateTime dateValue;
                if (DateTime.TryParse(value, out dateValue))
                {
                    DateStarted = dateValue;
                }
            }
        }

        private string _dateFinishedValue;
        [JsonProperty("date_finished", Required = Required.Always)]
        public string DateFinishedValue
        {
            get { return _dateFinishedValue; }
            set
            {
                _dateFinishedValue = value;

                DateFinished = null;
                DateTime dateValue;
                if (DateTime.TryParse(value, out dateValue))
                {
                    DateFinished = dateValue;
                }
            }
        }

        //[JsonProperty("level_of_completion", Required = Required.Always)]
        //public string LevelOfCompletion { get; set; }

        [JsonProperty("seconds_played", Required = Required.Always)]
        public int SecondsPlayed { get; set; }

        public DateTime? DateStarted { get; private set; }

        public DateTime? DateFinished { get; private set; }
    }
}

using System;
using System.Threading;

namespace ScheduleRepeater
{
    public class Repeater
    {
        private DateTime _RepeaterDate;
        private DateTime _StartDate;
        private DateTime? _EndDate;
        private bool _Debug;
        private bool _Monthly;
        private bool _BiWeekly;
        private bool _Weekly;
        private bool _Daily;
        private bool _Hourly;
        private bool _Minutely;
        private int _Minutes;

        private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();
        public Repeater()
        {
            _RepeaterDate = DateTime.Now;
            _StartDate = DateTime.Now;
            _EndDate = null;
            _Debug = true;
            ConfigureLogger();
        }

        public void DebuggingOff()
        {
            _Debug = false;
        }

        public void StartDateTimeForRepeater(int year, int month, int day, int hour, int minute, int second)
        {
            _StartDate = new DateTime(year, month, day, hour, minute, second);

            if(DateTime.Now > _StartDate)
            {
                _StartDate = DateTime.Now;
            }
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|The _StartDate has been setup as: {_StartDate}", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|The _StartDate has to come after the current date, defaulting to DateTime.Now.");
        }

        public void EndDateTimeForRepeater(int year, int month, int day, int hour, int minute, int second)
        {
            _EndDate = new DateTime(year, month, day, hour, minute, second);

            if(DateTime.Now > _EndDate || _RepeaterDate > _EndDate)
            {
                _EndDate = null;
            }
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|The _EndDate has been setup as: {_EndDate}", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|The _EndDate has to come after the current date and Repeating date, defaulting to null.");
        }

        public void RepeatMonthly()
        {
            _Monthly = true;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every month.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public void RepeatBiWeekly()
        {
            _BiWeekly = true;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every 2 weeks.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public void RepeatWeekly()
        {
            _Weekly = true;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every week.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public void RepeatDaily()
        {
            _Daily = true;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every day.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public void RepeatHourly()
        {
            _Hourly = true;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every hour.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public void RepeatEveryNthMinutes(int minute)
        {
            _Minutely = true;
            _Minutes = minute;
            TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Setting the Repeat time to once every {minute} minute(s).", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
        }

        public TimeSpan SetupRepeater()
        {
            DateTime currentTime = DateTime.Now;

            if(currentTime < _StartDate)
            {
                double timeDifference2 = Math.Abs(_StartDate.Subtract(currentTime).TotalSeconds);
                TimeSpan timeDifference3 = _StartDate.Subtract(currentTime);
                TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Currently Sleeping for {timeDifference3} until ready to start.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                Thread.Sleep((int)timeDifference2 * 1000);
            }

            if(currentTime > _RepeaterDate)
            {
                double timeDifference = Math.Abs(_RepeaterDate.Subtract(currentTime).TotalSeconds);
                _RepeaterDate = _RepeaterDate.AddSeconds(timeDifference);

                if(_Minutely)
                {
                    _RepeaterDate = _RepeaterDate.AddMinutes(_Minutes);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding {_Minutes} minute(s) to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else if(_Hourly)
                {
                    _RepeaterDate = _RepeaterDate.AddHours(1);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding 1 hour to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else if(_Daily)
                {
                    _RepeaterDate = _RepeaterDate.AddDays(1);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding 1 day to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else if(_Weekly)
                {
                    _RepeaterDate = _RepeaterDate.AddDays(7);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding 7 days to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else if(_BiWeekly)
                {
                    _RepeaterDate = _RepeaterDate.AddDays(14);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding 14 days to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else if(_Monthly)
                {
                    _RepeaterDate = _RepeaterDate.AddMonths(1);
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Adding 1 month to the sleep timer.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
            }
            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second, 0);
            _RepeaterDate = new DateTime(_RepeaterDate.Year, _RepeaterDate.Month, _RepeaterDate.Day, _RepeaterDate.Hour, _RepeaterDate.Minute, _RepeaterDate.Second, 0);

            TimeSpan tickTimer = _RepeaterDate.Subtract(currentTime);

            if(_EndDate != null)
            {
                if(_RepeaterDate < _EndDate)
                {
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|tickTimer: {tickTimer} = _ReapeaterDate {_RepeaterDate} - currentTime {currentTime}", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else
                {
                    tickTimer = (DateTime)_EndDate - currentTime;
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|tickTimer: {tickTimer} = _ReapeaterDate {_RepeaterDate} - currentTime {_EndDate}", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
            }
            else
            {
                TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|tickTimer: {tickTimer} = _ReapeaterDate {_RepeaterDate} - currentTime {currentTime}", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
            }

            return tickTimer;
        }

        public void RepeatedMethod(Action method)
        {
            while(true)
            {
                if(DateTime.Now > _EndDate)
                {
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Ending Time: {DateTime.Now}.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                    break;
                }
                TimeSpan sleepTime = SetupRepeater();
                double daystosleep = Math.Abs((double)sleepTime.TotalMilliseconds);

                if(daystosleep > Int32.MaxValue)
                {    
                    while(daystosleep > Int32.MaxValue)
                    {
                        method();

                        daystosleep = daystosleep - 86400000;
                        TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Milliseconds left after taking a days worth away: 86400000", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                    
                        Thread.Sleep(86400000);
                    }
                    TryCatchHelper($"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Milliseconds left after leaving while loop: {daystosleep}.", $"{System.Reflection.MethodBase.GetCurrentMethod().Name}|Error - ");
                }
                else
                {
                    method();
                }
                Thread.Sleep((int)daystosleep);
            }
        }

        private void TryCatchHelper(string info, string error)
        {
            if(_Debug)
            {
                try
                {
                    _Logger.Info(info);
                }
                catch (Exception ex)
                {
                    _Logger.Error($"Error: {error}");
                    _Logger.Error($"Error: {ex}");
                }
            }
        }

        private void ConfigureLogger()
        {
            DateTime today = DateTime.Now;
            
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"../../../Logs/CalendarRepeater/{today.Year}/{today.ToString("MMM")}/{today.Day.ToString("00")}.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("console");
            
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
            NLog.Common.InternalLogger.LogToConsole = true;
            NLog.LogManager.Configuration = config;
        }
    }
}

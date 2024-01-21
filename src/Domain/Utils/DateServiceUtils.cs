namespace Domain.Utils
{
    public static class DateServiceUtils
    {
        private const int DAY_ONE = 1;
        private const int CONSIDER_CURRENT_MONTH = 0;
        private const int CONSIDER_NEXT_MONTH = 1;

        public static string GetYearMonthPortugueseByDateTime(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = MonthNamePortuguese(dateTime.Month);

            var mesAno = string.Concat(month, "/", year);

            return mesAno;
        }

        public static bool IsCurrentMonth(string? initialYearMonth, string? finallyYearMonth)
        {
            if (initialYearMonth is null)
            {
                return false;
            }

            var currentDateTime = GetDateTimeByYearMonthBrazilian(initialYearMonth);

            var currentNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DAY_ONE, 0, 0, 0, kind: DateTimeKind.Utc);

            if (currentDateTime == currentNow)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetMonthsByDateTime(DateTime? initialDate, DateTime? finallyDate = null)
        {
            if (initialDate == null)
            {
                return 0;
            }

            TimeSpan difference;
            DateTime finallyDateConsider;

            if (initialDate == finallyDate)
            {
                finallyDate = finallyDate.Value.AddDays(1);
            }

            if (finallyDate != null)
            {
                finallyDateConsider = finallyDate.Value;
                difference = (finallyDateConsider - initialDate.Value);
            }
            else
            {
                finallyDateConsider = DateTime.Now;
                difference = (initialDate.Value - finallyDateConsider);
            }

            var totalDays = Math.Floor(difference.TotalDays);
            var months = (totalDays / 30);
            var totalMonths = int.Parse(Math.Ceiling(months).ToString());

            return totalMonths;
        }

        private static string MonthNamePortuguese(int month)
        {
            var monthBrazilian = string.Empty;

            switch (month)
            {
                case 1:
                    monthBrazilian = "Janeiro";
                    break;
                case 2:
                    monthBrazilian = "Fevereiro";
                    break;
                case 3:
                    monthBrazilian = "Março";
                    break;
                case 4:
                    monthBrazilian = "Abril";
                    break;
                case 5:
                    monthBrazilian = "Maio";
                    break;
                case 6:
                    monthBrazilian = "Junho";
                    break;
                case 7:
                    monthBrazilian = "Julho";
                    break;
                case 8:
                    monthBrazilian = "Agosto";
                    break;
                case 9:
                    monthBrazilian = "Setembro";
                    break;
                case 10:
                    monthBrazilian = "Outubro";
                    break;
                case 11:
                    monthBrazilian = "Novembro";
                    break;
                case 12:
                    monthBrazilian = "Dezembro";
                    break;
                default:
                    monthBrazilian = $"Não identificado: [{month}]";
                    break;
            }

            return monthBrazilian;
        }

        private static int Month(string monthNamePortugeuese)
        {
            var mes = int.MinValue;

            switch (monthNamePortugeuese)
            {
                case "Janeiro":
                    mes = 1;
                    break;
                case "Fevereiro":
                    mes = 2;
                    break;
                case "Março":
                    mes = 3;
                    break;
                case "Abril":
                    mes = 4;
                    break;
                case "Maio":
                    mes = 5;
                    break;
                case "Junho":
                    mes = 6;
                    break;
                case "Julho":
                    mes = 7;
                    break;
                case "Agosto":
                    mes = 8;
                    break;
                case "Setembro":
                    mes = 9;
                    break;
                case "Outubro":
                    mes = 10;
                    break;
                case "Novembro":
                    mes = 11;
                    break;
                case "Dezembro":
                    mes = 12;
                    break;
                default:
                    break;
            }

            return mes;
        }

        public static DateTime? GetDateTimeByYearMonthBrazilian(string? yearMonth, int addDays = 0, int addMonths = 0)
        {
            if (yearMonth is null)
            {
                return null;
            }

            var split = yearMonth.Split("/");

            var month = Month(split[0]);
            int year = Convert.ToInt32(split[1]);

            var dateTime = new DateTime(year, month, DAY_ONE, 0, 0, 0, kind: DateTimeKind.Utc)
                .AddDays(addDays)
                .AddMonths(addMonths);

            return dateTime;
        }

        public static Dictionary<string, DateTime> GetNextYearMonthAndDateTime(
            DateTime? dateTime, int qtdMonthAdd, int? bestPayDay, bool currentMonth = false, bool addFirstNextMonth = false)
        {
            Dictionary<string, DateTime> dictionary = new();

            DateTime newDatetime;

            var now = DateTime.Now;

            if (dateTime is null)
            {
                newDatetime = new DateTime(now.Year, now.Month, bestPayDay!.Value, 0, 0, 0, kind: DateTimeKind.Utc);
            }
            else
            {
                newDatetime = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, 0, 0, 0, kind: DateTimeKind.Utc);
            }

            int initialMonth = CONSIDER_NEXT_MONTH;

            if (currentMonth)
            {
                initialMonth = CONSIDER_CURRENT_MONTH;
            }

            for (int month = initialMonth; month <= qtdMonthAdd; month++)
            {
                DateTime dateTimeNext;

                if (addFirstNextMonth)
                {
                    dateTimeNext = newDatetime.AddMonths(month + 1);
                }
                else
                {
                    dateTimeNext = newDatetime.AddMonths(month);
                }

                var dateTimeNextMonth = newDatetime.AddMonths(month);
                string yearMonth;

                yearMonth = GetYearMonthPortugueseByDateTime(dateTimeNextMonth);

                dictionary.Add(yearMonth, dateTimeNext);
            }

            return dictionary;
        }

        public static Dictionary<int, string> GetListYearMonthsByThreeMonthsBeforeAndTwentyFourAfter()
        {
            var dateTimeCorteInitial = DateTime.Now.AddMonths(-3);

            DateTime firstDate = new(dateTimeCorteInitial.Year, dateTimeCorteInitial.Month, 1, 0, 0, 0, DateTimeKind.Local);

            Dictionary<int, string> yearMonths = new();

            for (int i = 0; i < 27; i++)
            {
                var dateAdd = firstDate.AddMonths(i);

                var yearMonthAdd = DateServiceUtils.GetYearMonthPortugueseByDateTime(dateAdd);

                yearMonths.Add(i, yearMonthAdd);
            }

            return yearMonths;
        }

        public static DateTime? GetDateTimeOfString(string? dateTime)
        {
            DateTime? dateTimeResult;

            if (dateTime is null)
            {
                return null;
            }

            try
            {
                dateTimeResult = Convert.ToDateTime(dateTime);
            }
            catch
            {
                dateTimeResult = null;
            }

            return dateTimeResult;
        }
    }
}
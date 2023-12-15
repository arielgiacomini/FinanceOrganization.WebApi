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

        public static bool IsCurrentMonth(string? initialYearMonth)
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

        public static double GetMonthsByDateTime(DateTime date)
        {
            var difference = (date - DateTime.Now);

            var totalDays = Math.Floor(difference.TotalDays);
            var totalMonths = Math.Floor((totalDays / 30));

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

        public static DateTime GetDateTimeByYearMonthBrazilian(string yearMonth)
        {
            var split = yearMonth.Split("/");

            var month = Month(split[0]);
            int year = Convert.ToInt32(split[1]);

            var dateTime = new DateTime(year, month, DAY_ONE, 0, 0, 0, kind: DateTimeKind.Utc);

            return dateTime;
        }

        public static Dictionary<string, DateTime>? GetNextYearMonthAndDateTime(
            DateTime? dateTime, int qtdMonthAdd, int? bestPayDay, bool currentMonth = false)
        {
            var newDatetime = DateTime.MinValue;
            var now = DateTime.Now;

            if (dateTime is null && bestPayDay is null)
            {
                return null;
            }

            if (bestPayDay.HasValue)
            {
                newDatetime = new DateTime(now.Year, now.Month, bestPayDay.Value, 0, 0, 0, kind: DateTimeKind.Utc);
            }

            if (dateTime.HasValue && !bestPayDay.HasValue)
            {
                newDatetime = dateTime.Value;
            }

            var dictionary = new Dictionary<string, DateTime>();

            int initialMonth = CONSIDER_NEXT_MONTH;

            if (currentMonth)
            {
                initialMonth = CONSIDER_CURRENT_MONTH;
            }

            for (int month = initialMonth; month <= qtdMonthAdd; month++)
            {
                var dateTimeNextMonth = newDatetime.AddMonths(month);

                var yearMonth = GetYearMonthPortugueseByDateTime(dateTimeNextMonth);

                dictionary.Add(yearMonth, dateTimeNextMonth);
            }

            return dictionary;
        }
    }
}
using Domain.Utils;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using Xunit;

namespace FinanceOrganization.UnitTests.Domain.Utils
{
    [Collection(nameof(UnitTestCollection))]
    public class DateServiceUtilsTests
    {
        public DateServiceUtilsTests()
        {
        }

        [Fact]
        public void Utils_GetMonthsByDateTime_Envio2DataEsperadoRetornoMeses_Success()
        {
            DateTime inicio = new(2024, 01, 01, 0, 0, 0, kind: DateTimeKind.Local);

            DateTime fim = new(2024, 01, 02, 0, 0, 0, kind: DateTimeKind.Local);

            var result = DateServiceUtils.GetMonthsByDateTime(inicio, fim);

            Assert.Equal(1, result);
        }

        [Fact]
        public void Utils_GetNextYearMonthAndDateTime_GarantirQueSeraAddMesParaDataPoremMesAnoContinuaMesmo_Success()
        {
            var result = DateServiceUtils.GetNextYearMonthAndDateTime(null, 1, 9, false, true);

            var firstKey = result.Keys.FirstOrDefault();
            var firstValue = result.Values.FirstOrDefault();

            var dataKey = DateServiceUtils.GetDateTimeByYearMonthBrazilian(firstKey);

            Assert.True(dataKey < firstValue);
        }
    }
}
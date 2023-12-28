using Domain.Utils;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Xunit;

namespace FinanceOrganization.UnitTests.Domain.Utils
{
    [Collection(nameof(UnitTestCollection))]
    public class DateServiceUtilsTests
    {
        private readonly ModelFixture _modelFixture;

        public DateServiceUtilsTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
        }

        [Fact]
        public void Utils_GetMonthsByDateTime_Envio2DataEsperadoRetornoMeses_Success()
        {
            DateTime inicio = new(2024, 01, 01, 0, 0, 0, kind: DateTimeKind.Local);

            DateTime fim = new(2024, 01, 02, 0, 0, 0, kind: DateTimeKind.Local);

            var result = DateServiceUtils.GetMonthsByDateTime(inicio, fim);

            Assert.Equal(1, result);
        }
    }
}
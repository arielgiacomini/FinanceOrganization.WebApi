using Application.Feature;
using Application.Feature.BillToPayRegistration.SearchBillToPayRegistration;
using Domain.Interfaces;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Moq;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.SearchBillToPayRegistration
{
    [Collection(nameof(UnitTestCollection))]
    public class SearchBillToPayRegistrationHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IBillToPayRegistrationRepository> _mockBillToPayRegistrationRepository;

        public SearchBillToPayRegistrationHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockBillToPayRegistrationRepository = new Mock<IBillToPayRegistrationRepository>();
        }

        [Fact]
        public async Task Hanle_DeveExecutarBuscaDeBillToPayRegistration_Sucesso()
        {
            // Setup

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            // Action

            var handle =
                new SearchBillToPayRegistrationHandler(_mockBillToPayRegistrationRepository.Object);

            var result = await handle.Handle();

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }
    }
}
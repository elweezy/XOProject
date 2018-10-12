using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace XOProject.Tests
{
    class TradeControllerTests
    {
        Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        int portfolioId = 1;

        public TradeControllerTests()
        {
            _portfolioRepositoryMock
                .Setup(x => x.GetAsync(It.Is<int>(id => id == portfolioId)))
                .Returns<int>(x => Task.FromResult(new Portfolio() { Id = portfolioId, Name = "John Doe" }));

            _shareRepositoryMock.Setup(x => x.GetBySymbol(It.Is<string>(s => s.Equals("REL"))))
                .Returns(Task.FromResult(new List<HourlyShareRate>(new[]
                    {
                        new HourlyShareRate() { Symbol = "REL", Rate = 50, TimeStamp = DateTime.Now.AddDays(-1) },
                        new HourlyShareRate() { Symbol = "REL", Rate = 100, TimeStamp = DateTime.Now },
                        new HourlyShareRate() { Symbol = "REL", Rate = 150, TimeStamp = DateTime.Now.AddDays(-2) },
                    }))
                );

            _tradeRepositoryMock.Setup(x => x.GetAllTradings(portfolioId))
                .Returns(Task.FromResult(new List<Trade>(new[]
                    {
                        new Trade() { Action = "BUY", NoOfShares = 120, PortfolioId = portfolioId, Price = 12000, Symbol = "REL" },
                        new Trade() { Action = "SELL", NoOfShares = 40, PortfolioId = portfolioId, Price = 4000, Symbol = "REL" }
                    }))
                );
        }

        [Test]
        public async Task Get_ShouldReturnAllTradings()
        {
            // Arrange
            var tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);

            // Act
            var result = await tradeController.GetAllTradings(portfolioId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            var resultList = result.Value as List<Trade>;

            Assert.NotZero(resultList.Count);
        }

        public async Task Get_ShouldReturnAnalysis()
        {
            // Arrange
            var tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);

            // Act
            var result = await tradeController.GetAnalysis("REL") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var resultList = result.Value as List<TradeAnalysis>;

            Assert.NotZero(resultList.Count);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace XOProject.Controller
{
    [Route("api/Trade")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portfolioId}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portfolioId)
        {
            var trade = await _tradeRepository.GetAllTradings(portfolioId);
            return Ok(trade);
        }


        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, 
        /// average and Sum of all the trades for that share individually grouped into Buy trade and Sell trade.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([FromRoute]string symbol)
        {
            var list = new List<TradeAnalysis>();

            //group actions
            var groupActions = _tradeRepository.Query().Where(x => x.Symbol.Equals(symbol)).GroupBy(x => x.Action);

            foreach (var ga in groupActions)
            {
                var ta = new TradeAnalysis
                {
                    Maximum = ga.Max(x => x.NoOfShares),
                    Minimum = ga.Min(x => x.NoOfShares),
                    Average = (decimal)ga.Average(x => x.NoOfShares),
                    Sum = ga.Sum(x => x.NoOfShares),
                    Action = ga.Select(x => x.Action).FirstOrDefault()
                };
               
                list.Add(ta);
            }

            return Ok(list);
        }
    }
}

using Cap.OpenTelemetry;
using DataAccess.EFCore.CCA;
using DTO.Requests;
using DTO.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BusinessLogic
{
    public class Card
    {
        CCAContext cCAContext;
        Span activitySpan;
        ILogger<Card> _logger;

        public Card(ILogger<Card> logger, CCAContext context, Span span)
        {
            _logger = logger;
            cCAContext = context;
            activitySpan = span;
        }

        public CardResponse GetCard(CardRequest request)
        {
            try
            {
                _logger.LogInformation("[GetCard] method of card called.");
                activitySpan.startWithContext("Card service invoked");

                //Step 1 - use CCA context to get Customer ...
                //Get Customer to verify customer ID

                var customer = cCAContext.Tblcustomers.Where(x => x.Customerid == request.CustomerID);



                //Step 2- Get Card if valid
                var entity = this.GetCustomerCard(cCAContext, request.CustomerID);

                if (entity!=null)
                {
                    activitySpan.setTag("cardNumber", entity.Cardnumber);
                    activitySpan.addEventLog("Card service [GETCard] completed");
                    activitySpan.Stop(SpanStatus.Ok);
                }
                else
                {
                    activitySpan.Stop(SpanStatus.Error);
                }

                return new CardResponse
                {
                    CardNumber = entity?.Cardnumber,
                    NameOnCard = entity?.Cardname,
                    MaskedCard = this.maskCard(entity?.Cardnumber)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private CardEntity GetCustomerCard(CCAContext context, string customerId)
        {
            //use CCA context to get CardEntity ...
            var card = context.Tbldebitcards.Where(x => x.Customerid == customerId).FirstOrDefault();
            return card;
        }

        private string maskCard(string cardNumber)
        {
            return cardNumber;
        }
    }
}

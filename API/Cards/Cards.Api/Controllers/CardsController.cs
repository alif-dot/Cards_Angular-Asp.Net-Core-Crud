using Microsoft.AspNetCore.Mvc;
using Cards.Api.Data;
using Microsoft.EntityFrameworkCore;
using Cards.Api.Models;
using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Cards.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly CardDbContext cardDbContext;

        public CardsController(CardDbContext cardDbContext)
        {
            this.cardDbContext = cardDbContext;
        }

        /// <summary>
        /// Gets all the cards.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Card[]), 200)]
        public async Task<ActionResult<Card[]>> GetAllCards()
        {
            var cards = await cardDbContext.Card.ToArrayAsync();
            return Ok(cards);
        }

        /// <summary>
        /// Gets a single card by ID.
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetCard")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Card>> GetCard(Guid id)
        {
            var card = await cardDbContext.Card.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                return Ok(card);
            }
            return NotFound("Card not found");
        }

        /// <summary>
        /// Adds a new card.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Card), 201)]
        public async Task<ActionResult<Card>> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await cardDbContext.Card.AddAsync(card);
            await cardDbContext.SaveChangesAsync();
            return CreatedAtRoute("GetCard", new { id = card.Id }, card);
        }

        /// <summary>
        /// Updates an existing card.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Card>> UpdateCard(Guid id, [FromBody] Card card)
        {
            var existingCard = await cardDbContext.Card.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                existingCard.CardholderName = card.CardholderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC = card.CVC;
                await cardDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }

        /// <summary>
        /// Deletes an existing card.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Card>> DeleteCard(Guid id)
        {
            var existingCard = await cardDbContext.Card.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                cardDbContext.Remove(existingCard);
                await cardDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }
    }
}

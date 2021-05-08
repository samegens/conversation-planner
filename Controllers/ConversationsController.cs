using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConversationPlanner.Data;
using ConversationPlanner.Models;
using ConversationPlanner.Logic;

namespace ConversationPlanner.Controllers
{
    public class ConversationsController : Controller
    {
        private readonly ConversationPlannerContext _context;
        private readonly RoundGenerator _roundGenerator = new RoundGenerator();

        public ConversationsController(ConversationPlannerContext context)
        {
            _context = context;
        }

        // GET: Conversations
        public async Task<IActionResult> Index()
        {
            var viewModels = await GetConversationListItemViewModels();
            return View(viewModels);
        }

        private async Task<List<ConversationListItemViewModel>> GetConversationListItemViewModels()
        {
            var viewModels = new List<ConversationListItemViewModel>();

            var conversations = await _context.Conversation
                .Include(c => c.Participant1)
                .Include(c => c.Participant2)
                .OrderByDescending(c => c.Timestamp)
                .ToListAsync();

            DateTime currentTimestamp = DateTime.MinValue;
            bool isEvenRound = true;
            foreach (var conversation in conversations)
            {
                if (conversation.Timestamp != currentTimestamp)
                {
                    isEvenRound = !isEvenRound;
                    currentTimestamp = conversation.Timestamp;
                }

                var viewModel = ConversationListItemViewModel.FromConversation(conversation, isEvenRound);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        // GET: Conversations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation
                .Include(c => c.Participant1)
                .Include(c => c.Participant2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // GET: Conversations/Create
        public IActionResult Create()
        {
            ViewData["Participants"] = new SelectList(_context.Participant, "Id", "Name");
            return View();
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id", "Participant1Id", "Participant2Id", "Timestamp")] ConversationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var conversation = viewModel.ToConversation(_context);
                _context.Add(conversation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Conversations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation
                .FindAsync(id);
            _context.Entry(conversation).Reference(c => c.Participant1).Load();
            _context.Entry(conversation).Reference(c => c.Participant2).Load();

            if (conversation == null)
            {
                return NotFound();
            }

            ViewData["Participants"] = new SelectList(_context.Participant, "Id", "Name");

            return View(ConversationViewModel.FromConversation(conversation));
        }

        // POST: Conversations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Participant1Id", "Participant2Id", "Timestamp")] ConversationViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var conversation = viewModel.ToConversation(_context);
                    _context.Update(conversation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversationExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Conversations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation
                .Include(c => c.Participant1)
                .Include(c => c.Participant2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conversation = await _context.Conversation.FindAsync(id);
            _context.Conversation.Remove(conversation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateNewRound(DateTime timestamp)
        {
            if (timestamp == DateTime.MinValue)
            {
                ModelState.AddModelError("Timestamp", "Please enter a valid timestamp.");
                var viewModels = await GetConversationListItemViewModels();
                return View("Index", viewModels);
            }
            var conversations = _context.Conversation.ToList();
            var newConversations = _roundGenerator.Generate(_context.Participant.ToList(), conversations, timestamp);
            await _context.AddRangeAsync(newConversations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConversationExists(int id)
        {
            return _context.Conversation.Any(e => e.Id == id);
        }
    }
}

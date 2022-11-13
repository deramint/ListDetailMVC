using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListDetailMVC.Data;
using ListDetailMVC.Models;

namespace ListDetailMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        /*
        public async Task<IActionResult> Index()
        {

            Perfecture.Initialize(_context);
            var applicationDbContext = _context.Book.Include(b => b.Author).Include(b => b.Publisher);
            return View(await applicationDbContext.ToListAsync());
        }
        */

        //上記の代わりにページング機能を追加した下記を追加
        public async Task<IActionResult> Index(int? page, string search)
        {
            Perfecture.Initialize(_context);

            if (page == null)
            {
                page = 0;
            }
            int max = 5;

            //下記は検索用
            var books = from m in _context.Book select m;
            //下記条件式は検索文字がnullなら検索をしないという意味
            if (!string.IsNullOrEmpty(search))
            {
                //書籍名・著者・出版社で検索
                books = books.Where(b => b.Title.Contains(search) || b.Author.Name.Contains(search) || b.Publisher.Title.Contains(search));
            }
            ViewData["search"] = search;

            //下記はページング用
            books = books
                .Skip(max * page.Value).Take(max)
                .Include(b => b.Author).Include(b => b.Publisher);

            if(page.Value > 0)
            {
                ViewData["prev"] = page.Value - 1;
            }
            if(books.Count() >= max)
            {
                ViewData["next"] = page.Value + 1;
                //次のページがあるか確認
                if(_context.Book.Skip(max * (page.Value + 1)).Take(max).Count() == 0)
                {
                    ViewData["next"] = null;
                }
            }
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {

            //BookオブジェクトのISBN番号の初期値を入れる
            var book = new Book();
            book.ISBN = "123-4-5678-9012-A";

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Title");
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,AuthorId,PublisherId,price,PublishDate,ISBN")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Title", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Title", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,PublisherId,price,PublishDate,ISBN")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Title", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return _context.Book.Any(e => e.Id == id);
        }
    }
}

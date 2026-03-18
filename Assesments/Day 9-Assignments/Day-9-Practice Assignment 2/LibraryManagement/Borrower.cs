using System;
using System.Collections.Generic;

namespace LibraryManagement
{
    /// <summary>
    /// Represents a borrower who can borrow books from the library.
    /// </summary>
    public class Borrower
    {
        // Borrower details
        public string Name { get; set; }
        public string LibraryCardNumber { get; set; }
        public List<Book> BorrowedBooks { get; set; }

        // Constructor to set up a new borrower
        public Borrower(string name, string libraryCardNumber)
        {
            Name = name;
            LibraryCardNumber = libraryCardNumber;
            BorrowedBooks = new List<Book>(); // starts with no borrowed books
        }

        /// <summary>
        /// Borrows a book and adds it to the borrower's list.
        /// </summary>
        public void BorrowBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }

            book.Borrow(); // this will throw if the book is already borrowed
            BorrowedBooks.Add(book);
        }

        /// <summary>
        /// Returns a book and removes it from the borrower's list.
        /// </summary>
        public void ReturnBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }

            if (!BorrowedBooks.Contains(book))
            {
                throw new InvalidOperationException("This borrower did not borrow this book.");
            }

            book.Return(); // marks book as available
            BorrowedBooks.Remove(book);
        }
    }
}

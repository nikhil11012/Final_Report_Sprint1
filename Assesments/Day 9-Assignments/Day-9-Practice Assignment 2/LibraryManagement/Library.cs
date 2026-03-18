using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{
    /// <summary>
    /// Represents the library that manages books and borrowers.
    /// </summary>
    public class Library
    {
        // Lists to store all books and borrowers
        public List<Book> Books { get; set; }
        public List<Borrower> Borrowers { get; set; }

        // Constructor to initialize the library
        public Library()
        {
            Books = new List<Book>();
            Borrowers = new List<Borrower>();
        }

        /// <summary>
        /// Adds a new book to the library.
        /// </summary>
        public string AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }

            Books.Add(book);
            return $"Book '{book.Title}' has been added successfully.";
        }

        /// <summary>
        /// Registers a new borrower in the library.
        /// </summary>
        public string RegisterBorrower(Borrower borrower)
        {
            if (borrower == null)
            {
                throw new ArgumentNullException(nameof(borrower), "Borrower cannot be null.");
            }

            Borrowers.Add(borrower);
            return $"Borrower '{borrower.Name}' has been registered successfully.";
        }

        /// <summary>
        /// Allows a borrower to borrow a book using ISBN and library card number.
        /// </summary>
        public string BorrowBook(string isbn, string libraryCardNumber)
        {
            // Find the book by ISBN
            Book book = Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found in the library.");
            }

            // Find the borrower by library card number
            Borrower borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);
            if (borrower == null)
            {
                throw new InvalidOperationException("Borrower not found in the library.");
            }

            // Borrow the book through the borrower
            borrower.BorrowBook(book);
            return $"Book '{book.Title}' has been borrowed by {borrower.Name}.";
        }

        /// <summary>
        /// Allows a borrower to return a book using ISBN and library card number.
        /// </summary>
        public string ReturnBook(string isbn, string libraryCardNumber)
        {
            // Find the book by ISBN
            Book book = Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found in the library.");
            }

            // Find the borrower by library card number
            Borrower borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);
            if (borrower == null)
            {
                throw new InvalidOperationException("Borrower not found in the library.");
            }

            // Return the book through the borrower
            borrower.ReturnBook(book);
            return $"Book '{book.Title}' has been returned by {borrower.Name}.";
        }

        /// <summary>
        /// Returns a list of all books with their current status.
        /// </summary>
        public List<string> ViewBooks()
        {
            List<string> bookList = new List<string>();

            foreach (var book in Books)
            {
                string status = book.IsBorrowed ? "Borrowed" : "Available";
                bookList.Add($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Status: {status}");
            }

            return bookList;
        }

        /// <summary>
        /// Returns a list of all borrowers with their borrowed books.
        /// </summary>
        public List<string> ViewBorrowers()
        {
            List<string> borrowerList = new List<string>();

            foreach (var borrower in Borrowers)
            {
                // Get the titles of borrowed books, or "None" if the list is empty
                string borrowedBooks = borrower.BorrowedBooks.Count > 0
                    ? string.Join(", ", borrower.BorrowedBooks.Select(b => b.Title))
                    : "None";

                borrowerList.Add($"Name: {borrower.Name}, Card: {borrower.LibraryCardNumber}, Borrowed Books: {borrowedBooks}");
            }

            return borrowerList;
        }
    }
}

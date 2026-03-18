using NUnit.Framework;
using LibraryManagement;
using System.Collections.Generic;

namespace LibraryManagement.Tests
{
    [TestFixture]
    public class LibraryTests
    {
        private Library _library;

        // This runs before each test to give us a fresh library
        [SetUp]
        public void Setup()
        {
            _library = new Library();
        }

        // ========== Tests for Adding a Book ==========

        [Test]
        public void AddBook_ShouldAddBookToLibrary()
        {
            // Arrange
            Book book = new Book("The Great Gatsby", "F. Scott Fitzgerald", "978-0743273565");

            // Act
            string result = _library.AddBook(book);

            // Assert
            Assert.That(_library.Books.Count, Is.EqualTo(1));
            Assert.That(_library.Books[0].Title, Is.EqualTo("The Great Gatsby"));
            Assert.That(result, Does.Contain("added successfully"));
        }

        [Test]
        public void AddBook_ShouldReturnConfirmationMessage()
        {
            // Arrange
            Book book = new Book("1984", "George Orwell", "978-0451524935");

            // Act
            string result = _library.AddBook(book);

            // Assert
            Assert.That(result, Is.EqualTo("Book '1984' has been added successfully."));
        }

        // ========== Tests for Registering a Borrower ==========

        [Test]
        public void RegisterBorrower_ShouldAddBorrowerToLibrary()
        {
            // Arrange
            Borrower borrower = new Borrower("Alice Johnson", "LC001");

            // Act
            string result = _library.RegisterBorrower(borrower);

            // Assert
            Assert.That(_library.Borrowers.Count, Is.EqualTo(1));
            Assert.That(_library.Borrowers[0].Name, Is.EqualTo("Alice Johnson"));
            Assert.That(result, Does.Contain("registered successfully"));
        }

        [Test]
        public void RegisterBorrower_ShouldReturnConfirmationMessage()
        {
            // Arrange
            Borrower borrower = new Borrower("Bob Smith", "LC002");

            // Act
            string result = _library.RegisterBorrower(borrower);

            // Assert
            Assert.That(result, Is.EqualTo("Borrower 'Bob Smith' has been registered successfully."));
        }

        // ========== Tests for Borrowing a Book ==========

        [Test]
        public void BorrowBook_ShouldMarkBookAsBorrowed()
        {
            // Arrange
            Book book = new Book("To Kill a Mockingbird", "Harper Lee", "978-0061120084");
            Borrower borrower = new Borrower("Charlie Brown", "LC003");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);

            // Act
            _library.BorrowBook("978-0061120084", "LC003");

            // Assert - book should now be marked as borrowed
            Assert.That(book.IsBorrowed, Is.True);
        }

        [Test]
        public void BorrowBook_ShouldAssociateBookWithBorrower()
        {
            // Arrange
            Book book = new Book("Pride and Prejudice", "Jane Austen", "978-0141439518");
            Borrower borrower = new Borrower("Diana Prince", "LC004");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);

            // Act
            _library.BorrowBook("978-0141439518", "LC004");

            // Assert - borrower should have the book in their list
            Assert.That(borrower.BorrowedBooks.Count, Is.EqualTo(1));
            Assert.That(borrower.BorrowedBooks[0].Title, Is.EqualTo("Pride and Prejudice"));
        }

        [Test]
        public void BorrowBook_AlreadyBorrowed_ShouldThrowException()
        {
            // Arrange
            Book book = new Book("The Hobbit", "J.R.R. Tolkien", "978-0547928227");
            Borrower borrower1 = new Borrower("Eve Adams", "LC005");
            Borrower borrower2 = new Borrower("Frank Miller", "LC006");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower1);
            _library.RegisterBorrower(borrower2);

            // Borrow the book first
            _library.BorrowBook("978-0547928227", "LC005");

            // Act & Assert - trying to borrow again should throw
            Assert.Throws<System.InvalidOperationException>(() =>
                _library.BorrowBook("978-0547928227", "LC006"));
        }

        // ========== Tests for Returning a Book ==========

        [Test]
        public void ReturnBook_ShouldMarkBookAsAvailable()
        {
            // Arrange
            Book book = new Book("Moby Dick", "Herman Melville", "978-0142437247");
            Borrower borrower = new Borrower("Grace Lee", "LC007");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);
            _library.BorrowBook("978-0142437247", "LC007");

            // Act
            _library.ReturnBook("978-0142437247", "LC007");

            // Assert - book should be available again
            Assert.That(book.IsBorrowed, Is.False);
        }

        [Test]
        public void ReturnBook_ShouldRemoveBookFromBorrowerList()
        {
            // Arrange
            Book book = new Book("War and Peace", "Leo Tolstoy", "978-0143039990");
            Borrower borrower = new Borrower("Henry Ford", "LC008");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);
            _library.BorrowBook("978-0143039990", "LC008");

            // Act
            _library.ReturnBook("978-0143039990", "LC008");

            // Assert - borrower should have no books now
            Assert.That(borrower.BorrowedBooks.Count, Is.EqualTo(0));
        }

        // ========== Tests for Viewing Books and Borrowers ==========

        [Test]
        public void ViewBooks_ShouldReturnListOfAllBooks()
        {
            // Arrange
            _library.AddBook(new Book("Book One", "Author A", "ISBN-001"));
            _library.AddBook(new Book("Book Two", "Author B", "ISBN-002"));

            // Act
            List<string> books = _library.ViewBooks();

            // Assert
            Assert.That(books.Count, Is.EqualTo(2));
            Assert.That(books[0], Does.Contain("Book One"));
            Assert.That(books[1], Does.Contain("Book Two"));
        }

        [Test]
        public void ViewBooks_ShouldShowCorrectStatus()
        {
            // Arrange
            Book book = new Book("Test Book", "Test Author", "ISBN-003");
            Borrower borrower = new Borrower("Test User", "LC009");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);

            // Act - check status before borrowing
            List<string> booksBeforeBorrow = _library.ViewBooks();
            Assert.That(booksBeforeBorrow[0], Does.Contain("Available"));

            // Borrow the book
            _library.BorrowBook("ISBN-003", "LC009");

            // Assert - check status after borrowing
            List<string> booksAfterBorrow = _library.ViewBooks();
            Assert.That(booksAfterBorrow[0], Does.Contain("Borrowed"));
        }

        [Test]
        public void ViewBorrowers_ShouldReturnListOfAllBorrowers()
        {
            // Arrange
            _library.RegisterBorrower(new Borrower("User One", "LC010"));
            _library.RegisterBorrower(new Borrower("User Two", "LC011"));

            // Act
            List<string> borrowers = _library.ViewBorrowers();

            // Assert
            Assert.That(borrowers.Count, Is.EqualTo(2));
            Assert.That(borrowers[0], Does.Contain("User One"));
            Assert.That(borrowers[1], Does.Contain("User Two"));
        }

        [Test]
        public void ViewBorrowers_ShouldShowBorrowedBooks()
        {
            // Arrange
            Book book = new Book("Sample Book", "Sample Author", "ISBN-004");
            Borrower borrower = new Borrower("Jane Doe", "LC012");
            _library.AddBook(book);
            _library.RegisterBorrower(borrower);
            _library.BorrowBook("ISBN-004", "LC012");

            // Act
            List<string> borrowers = _library.ViewBorrowers();

            // Assert - should show the borrowed book title
            Assert.That(borrowers[0], Does.Contain("Sample Book"));
        }
    }
}

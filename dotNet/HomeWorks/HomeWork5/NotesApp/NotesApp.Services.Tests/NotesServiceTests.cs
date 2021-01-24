using System;
using Xunit;
using Moq;
using NotesApp.Services.Abstractions;
using NotesApp.Services.Services;
using NotesApp.Services.Models;

namespace NotesApp.Services.Tests
{
    public class NotesServiceTests
    {
        private Mock<INotesStorage> notesStorageMock;
        private Mock<INoteEvents> notesEventsMock;
        private NotesService testNotesService;
        public NotesServiceTests()
        {
            notesEventsMock = new Mock<INoteEvents>();
            notesStorageMock = new Mock<INotesStorage>();
            testNotesService = new NotesService(notesStorageMock.Object, notesEventsMock.Object);
        }

        [Fact]
        public void AddNote_NullNote_ThrowsArgumentNullException()
        {
            //arrange
            Note testInputNote = null;
            int testUserId = 0;

            //act
            Action act = () => testNotesService.AddNote(testInputNote, testUserId);

            //assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'note')", exception.Message);
        }

        [Fact]
        public void AddNote_SuccessfullAddNote_NotifyAdded()
        {
            //arrange
            Note testInputNote = new Note();
            int testUserId = 0;

            //act
            Action act = () => testNotesService.AddNote(testInputNote, testUserId);
            var exception = Record.Exception(act);

            //assert
            Assert.Null(exception);
            notesEventsMock.Verify(mock => mock.NotifyAdded(testInputNote, testUserId), Times.Once);
        }

        [Fact]
        public void AddNote_UnsuccessfullAddNote_NotifyAddedNotInvoked()
        {
            //arrange
            Note testInputNote = new Note();
            int testUserId = 0;
            notesStorageMock.Setup(mock => mock.AddNote(testInputNote, testUserId)).Throws(new InvalidOperationException("Unsccessfull operation"));

            //act
            Action act = () => testNotesService.AddNote(testInputNote, testUserId);
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(act);

            //assert
            Assert.Equal("Unsccessfull operation", exception.Message);
            notesEventsMock.Verify(mock => mock.NotifyAdded(testInputNote, testUserId), Times.Never);
        }

        [Fact]
        public void DeleteNote_SuccessfullDeleteNote_NotifyDeleted()
        {
            //arrange
            Guid testNoteGuid = Guid.NewGuid();
            int testUserId = 0;
            notesStorageMock.Setup(mock => mock.DeleteNote(testNoteGuid)).Returns(true);

            //act
            Action act = () => testNotesService.DeleteNote(testNoteGuid, testUserId);
            var exception = Record.Exception(act);

            //assert
            Assert.Null(exception);
            notesEventsMock.Verify(mock => mock.NotifyDeleted(testNoteGuid, testUserId), Times.Once);
        }

        [Fact]
        public void DeleteNote_UnsuccessfullDeleteNote_NotifyDeletedNotInvoked()
        {
            //arrange
            Guid testNoteGuid = Guid.NewGuid();
            int testUserId = 0;
            notesStorageMock.Setup(mock => mock.DeleteNote(testNoteGuid)).Returns(false);

            //act
            Action act = () => testNotesService.DeleteNote(testNoteGuid, testUserId);
            var exception = Record.Exception(act);

            //assert
            Assert.Null(exception);
            notesEventsMock.Verify(mock => mock.NotifyDeleted(testNoteGuid, testUserId), Times.Never);
        }

    }
}

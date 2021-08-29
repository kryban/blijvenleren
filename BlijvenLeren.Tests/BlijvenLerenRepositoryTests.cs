using BlijvenLeren.Data;
using BlijvenLeren.Models;
using BlijvenLeren.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BlijvenLeren.Tests
{
    public class BlijvenLerenRepositoryTests
    {
        private const string testCommentText = "TestCommentText";
        private const string testResourceText = "TestResourceText";

        BlijvenLerenContext blijvenLerenContext;
        BlijvenLerenRepository sut;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BlijvenLerenContext>()
                .UseInMemoryDatabase(databaseName: "Foo")
                .Options;

            blijvenLerenContext = new BlijvenLerenContext(options);
            blijvenLerenContext.Database.EnsureDeleted();

            CreateGivenLearnResourceMocks(2);

            sut = new BlijvenLerenRepository(blijvenLerenContext);
        }

        [Test]
        public void GetAllLearningResources_ReturnsAllLearningResources()
        {
            var expected = 2;
            var actual = sut.GetAllLearnResources().Result.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(1)]
        [TestCase(2)]
        [Test]
        public void GetLearnResourceDetails_ReturnsDetailsOfResourceAndComment(int id)
        {
            var expectedResourceText = $"{testResourceText}{id}";
            var expectedCommentText = $"{testCommentText}{id}";

            var actualResourceText = sut.GetLearnResourceDetails(id).Result.LearnResource.Description;
            var actualCommentText = sut.GetLearnResourceDetails(id).Result.LearnResource.Comments.ToList()[id - 1].CommentText;

            Assert.AreEqual(expectedResourceText, actualResourceText);
            Assert.AreEqual(expectedCommentText, actualCommentText);
        }

        [TestCase(1)]
        [TestCase(2)]
        [Test]
        public void GetLearnResource_ReturnsRequestedResource(int id)
        {
            var expectedResourcId = id;
            var actuelResourceId = sut.GetLearnResource(id).Result.LearnResourceId;

            Assert.AreEqual(expectedResourcId, actuelResourceId);
        }

        [Test]
        public void AddLearnResource_AddsNewResource()
        {
            var before = blijvenLerenContext.LearnResource.Count();

            sut.AddLearnResource(new LearnResource()).Wait();

            var after = blijvenLerenContext.LearnResource.Count();

            Assert.AreNotEqual(before, after);
        }

        [Test]
        public void AddComment_AddsNewComment()
        {
            var before = blijvenLerenContext.Comment.Count();
            sut.AddComment(new Comment()).Wait();
            var after = blijvenLerenContext.Comment.Count();
            Assert.AreNotEqual(before, after);
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public void LearnResourceExists_ReturnsFindings(int id, bool expected)
        {
            var actual = sut.LearnResourceExists(id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void DeleteLearnResource_DeletesGivenResource(int id)
        {
            var before = blijvenLerenContext.LearnResource.Count();
            sut.DeleteLearnResource(id).Wait();
            var after = blijvenLerenContext.LearnResource.Count();

            Assert.AreNotEqual(before, after);
            Assert.IsFalse(blijvenLerenContext.LearnResource.Any(r => r.LearnResourceId == id));
        }

        private void CreateGivenLearnResourceMocks(int numberOfObjects)
        {           
            for (int i = 1; i <= numberOfObjects; i++)
            {
                var resource = new LearnResource
                {
                    Description = $"{testResourceText}{i}",
                    Comments = CreateGivenCommentMock(2)
                };

                blijvenLerenContext.LearnResource.Add(resource);
            }

            blijvenLerenContext.SaveChanges();
        }

        private List<Comment> CreateGivenCommentMock(int numberOfObjects)
        {
            var retval = new List<Comment>();

            for (int i = 1; i <= numberOfObjects; i++)
            {
                retval.Add(new Comment { CommentText = $"{testCommentText}{i}"});
            }

            return retval;
        }
    }
}
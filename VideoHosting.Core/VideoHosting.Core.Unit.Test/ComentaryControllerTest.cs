using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Core.Controllers;

namespace VideoHosting.Core.Unit.Test
{
	public class CommentaryControllerTest
	{
		private ICommentaryService _CommentaryService;
		private IEnumerable<CommentaryDto> Commentaries;

		[SetUp]
		public void Setup()
		{
			Commentaries = new CommentaryDto[]{new CommentaryDto(){Content = "Ololo",Id = Guid.NewGuid(),UserId = "1"}};

			_CommentaryService = Substitute.For<ICommentaryService>();
			_CommentaryService.GetCommentariesByVideoId(Arg.Any<Guid>()).Returns(Commentaries);
			_CommentaryService.GetCommentaryById(Arg.Any<Guid>()).Returns(Commentaries.First());
		}

		[Test]
		public async Task GivenCommentaryController_WhenGetCommentariesByVideoId_ThenShouldGetThisCommentaries()
		{
			
			CommentaryController CommentaryController = new CommentaryController(_CommentaryService);

			ActionResult actionResult = await CommentaryController.GetCommentariesByVideoId(new Guid());
			var okResult = actionResult as ObjectResult;
			IEnumerable<CommentaryDto> Commentary = okResult.Value as IEnumerable<CommentaryDto>;

			Commentary.Should().BeEquivalentTo(Commentaries);
		}
		[Test]
		public async Task GivenCommentaryController_WhenCreateCommentary_ThenShouldCallMethodCreate()
		{
			CommentaryController CommentaryController = new CommentaryController(_CommentaryService);

			ActionResult actionResult = await CommentaryController.CreateCommentary(new CommentaryDto(){Content = "ololo",UserId = "1",VideoId = new Guid()});

			await _CommentaryService.Received(1).AddCommentary(Arg.Any<CommentaryDto>());
		}
		[Test]
        public async Task GivenCommentaryController_WhenDeleteCommentary_Then_ShouldCallDeleteMethod()
        {
            ClaimsPrincipal user = Substitute.For<ClaimsPrincipal>();
            user.Identity.Name.Returns("1");

            CommentaryController CommentaryController = new CommentaryController(_CommentaryService);
            CommentaryController.ControllerContext = new ControllerContext(){HttpContext = new DefaultHttpContext(){User =user }};

            ActionResult actionResult = await CommentaryController.DeleteCommentary(new Guid());

            await _CommentaryService.Received(1).RemoveCommentary(Arg.Any<Guid>());
		}
		
	}
}
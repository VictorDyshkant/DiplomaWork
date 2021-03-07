using System;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using System.Threading.Tasks;
using VideoHosting.Services.Services;
using VideoHosting.Services.Mapper;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;

namespace VideoHisting.Services.Unit.Test
{
	[TestFixture]
	public class CommentaryServiceTest
	{
		IMapper _mapper;
		IUnitOfWork _mockUnitofwork;
		IVideoRepository _mockVideoRepository;
		IUserRepository _mockUserRepository;
		ICommentaryRepository _mockCommentaryRepository;
		Commentary Commentary;
		[SetUp]
		public void Initialize()
		{
			Commentary = new Commentary() { Id = new Guid(), User = null,Video = null, Content = "Olololo" };

			_mockUnitofwork = Substitute.For<IUnitOfWork>();

			 _mockUserRepository = Substitute.For<IUserRepository>();
			_mockUserRepository.GetUserById(Arg.Any<string>()).Returns(Task.FromResult(new User() { }));


			_mockVideoRepository = Substitute.For<IVideoRepository>();
			_mockVideoRepository.GetVideoById(Arg.Any<Guid>()).Returns(Task.FromResult(new Video { }));

			_mockCommentaryRepository = Substitute.For<ICommentaryRepository>();
			_mockCommentaryRepository.GetCommentaryById(Arg.Any<Guid>()).Returns(Task.FromResult(Commentary));
			_mockCommentaryRepository.GetCommentariesByVideoId(Arg.Any<Guid>()).Returns(Task.FromResult(new Commentary[] { Commentary } as IEnumerable<Commentary>));

			_mockUnitofwork.VideoRepository.Returns(_mockVideoRepository);
			_mockUnitofwork.UserRepository.Returns(_mockUserRepository);
			_mockUnitofwork.CommentaryRepository.Returns(_mockCommentaryRepository);

			_mapper = new MapperConfiguration(opt =>
			{
				opt.AddProfiles(new Profile[] { new MapperService(null) });
			}).CreateMapper();
		}

		[Test]
		public async Task GivenCommentaryService_WhenWeAddCommentary_ThenShouldCallAddCommentaryAndSaveMethods()
		{
			CommentaryService CommentaryService = new CommentaryService(_mockUnitofwork,_mapper);

			await CommentaryService.AddCommentary(new CommentaryDto { UserId = "1", VideoId = new Guid() });

			_mockCommentaryRepository.Received(1).AddCommentary(Arg.Any<Commentary>());
			await _mockUnitofwork.Received(1).SaveAsync();
		}

		[Test]
		public async Task GivenCommentaryService_WhenGetCommentaryByID_ThenShouldGetThisCommentary()
		{
			CommentaryDto accpected = _mapper.Map<CommentaryDto>(Commentary);

			CommentaryService CommentaryService = new CommentaryService(_mockUnitofwork, _mapper);
			CommentaryDto CommentaryDto = await CommentaryService.GetCommentaryById(new Guid());

			accpected.Should().BeEquivalentTo(CommentaryDto);
		}

		[Test]
		public async Task GivenCommentaryService_WhenRemoveCommentary_ThenShouldCallRemoveMethodAndSaveMethod()
		{
			CommentaryService CommentaryService = new CommentaryService(_mockUnitofwork, _mapper);

			await CommentaryService.RemoveCommentary(new Guid());

			_mockCommentaryRepository.Received(1).RemoveCommentary(Commentary);
			await _mockUnitofwork.Received(1).SaveAsync();
		}

		[Test]
		public async Task GivenCommentaryService_WhenGetCommentaiesByVideoId_ThenShouldGetThisCommentaries()
		{
			IEnumerable<Commentary> CommentariesAccpected = new Commentary[] { Commentary };
			IEnumerable<CommentaryDto> accpected = _mapper.Map<IEnumerable<CommentaryDto>>(CommentariesAccpected);

			CommentaryService CommentaryService = new CommentaryService(_mockUnitofwork, _mapper);
		    IEnumerable<CommentaryDto> Commentaries =	await CommentaryService.GetCommentariesByVideoId(Guid.NewGuid());

			accpected.Should().BeEquivalentTo(Commentaries);
		}

    }
}

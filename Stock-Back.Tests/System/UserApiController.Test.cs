using Stock_Back.Controllers.UserApiControllers;
using Stock_Back.DAL.Context;
using Stock_Back.DAL.Models;
using Moq.EntityFrameworkCore;
using Stock_Back.BLL.Models.UserDTO;


namespace Stock_Back.Tests
{
    public class UserApiControllerTests
    {
        [Fact]
        public void Test_GetUserById()
        {
            // Given
            var mockDbContext = new Mock<AppDbContext>();
            var data = new List<User> { new User { Id = 1 } };
            mockDbContext.Setup(x => x.Set<User>()).ReturnsDbSet(data);

            // When
            var usuarioService = new UserApiController(mockDbContext.Object);
            var usuario = usuarioService.GetUsers(1);

            // Then
            Assert.NotNull(usuario);
            Assert.Equal(1, usuario.Id);
        }
        [Fact]
        public async void Test_GetUserById_MultipleUsers()
        {
            // Given
            var mockDbContext = new Mock<AppDbContext>();
            var data = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            mockDbContext.Setup(x => x.Set<User>()).ReturnsDbSet(data);

            // When
            var usuarioService = new UserApiController(mockDbContext.Object);
            var usuario = usuarioService.GetUsers(1);

            // Then
            Assert.NotNull(usuario);
            Assert.Equal(1, usuario.Id);
        }

        [Fact]
        public void Test_InsertUser_InvalidData()
        {
            // Given
            var mockDbContext = new Mock<AppDbContext>();
            var usuarioService = new UserApiController(mockDbContext.Object);

            // When
            var user = new UserInsertDTO { Name = "" };

            // Then
            Assert.ThrowsAsync<ArgumentException>(async () => await usuarioService.InsertUser(user));
        }


        [Fact]
        public void Test_InsertUser_ExistingId()
        {
            // Given
            var mockDbContext = new Mock<AppDbContext>();
            var data = new List<User> { new User { Name = "Test", Email = "test@test.com" } };
            mockDbContext.Setup(x => x.Set<User>()).ReturnsDbSet(data);

            // When
            var usuarioService = new UserApiController(mockDbContext.Object);

            var user = new UserInsertDTO { Name = "Test", Email = "test@test.com" };

            // Then
            Assert.ThrowsAsync<ArgumentException>(async () => await usuarioService.InsertUser(user));
        }

        [Fact]
        public async void Test_UpdateUser_PartialUpdate()
        {
            // Given
            var mockDbContext = new Mock<AppDbContext>();
            var data = new List<User> { new User { Id = 1, Name = "Test" } };
            mockDbContext.Setup(x => x.Set<User>()).ReturnsDbSet(data);

            // When
            var usuarioService = new UserApiController(mockDbContext.Object);
            var userEdited = new UserEditDTO { Id = 1, Name = "Updated Name" };
            await usuarioService.UpdateUser(userEdited);

            // Verificar que el nombre se actualizó en la base de datos
            var updatedUser = mockDbContext.Object.Users.Find(1);
            Assert.Equal("Updated Name", updatedUser.Name);
        }


        /*Medio Meh*/
        /*[Fact]
        public async Task GetUser_Returns_User_When_Found()
        {
            // Arrange
            // var mockDbContext = new FakeAppDbContext();
            // var mockDbContext = new Mock<AppDbContext>();

            var expectedUser = new User { Id = 1 };
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                        .Options;
            var mockDbContext = new Mock<AppDbContext>(dbContextOptions);
            mockDbContext.Setup(x => x.FindAsync<User>(1))
                          .ReturnsAsync(expectedUser);


            var controller = new UserApiController(mockDbContext.Object);

            // Act
            var result = await controller.GetUsers(1);

            // Assert
            // Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var userDto = okResult.Value as UserDTO;
            Assert.NotNull(userDto);
            Assert.Equal(expectedUser.Id, userDto.Id);
            Assert.Equal(expectedUser.Name, userDto.Name);
        }
*/
    }
}

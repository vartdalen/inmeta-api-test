using HashidsNet;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;
using Inmeta.Test.Services;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Customers;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using MockQueryable.NSubstitute;
using System.Collections.ObjectModel;

namespace Inmeta.Test.Tests
{
	public class CustomerServiceTests
	{
		[Fact]
		public async Task Create_CustomerCreatedSuccessfully_ReturnsCreatedCustomer()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var hashidsMock = Substitute.For<IHashidsService>();
			var customerService = new CustomerService(repositoryMock, hashidsMock);

			var createDto = new CreateCustomerDto { Email = "customer@example.com" };
			var expectedCustomer = new Customer
			{
				Id = 1,
				CreatedAt = DateTimeOffset.UtcNow,
				ModifiedAt = DateTimeOffset.UtcNow,
				Email = createDto.Email,
				Orders = new Collection<Order>()
			};

			repositoryMock.Create(Arg.Any<CustomerRequest>()).Returns(expectedCustomer);

			// Act
			var result = await customerService.Create(createDto);

			// Assert
			Assert.Equal(createDto.Email, result.Email);
		}

		[Fact]
		public async Task Read_ValidId_ReturnsCustomer()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var cHashidsMock = Substitute.For<IHashids>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();

			var customerHashId = "encoded-customer-id";
			var expectedCustomer = new Customer { Email = "customer@example.com" };

			cHashidsMock.TryDecodeSingle(customerHashId, out _).Returns(true);
			hashidsServiceMock.Get(typeof(Customer)).Returns(cHashidsMock);
			repositoryMock.Read(Arg.Any<int>()).Returns(expectedCustomer);

			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			// Act
			var result = await customerService.Read(new CustomerHashId(customerHashId));

			// Assert
			Assert.Equal(expectedCustomer.Email, result.Email);
		}

		[Fact]
		public async Task Read_PageNumberAndPageSize_ReturnsPagedCustomers()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();
			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			var pageNumber = 1;
			var pageSize = 3;
			var query = "customer";
			var customers = new Collection<Customer>
			{
				new Customer
				{
					Id = 1,
					CreatedAt = DateTimeOffset.UtcNow,
					ModifiedAt = DateTimeOffset.UtcNow,
					Email = "customer1@example.com",
					Orders = new Collection<Order>()
				},
				new Customer
				{
					Id = 2,
					CreatedAt = DateTimeOffset.UtcNow,
					ModifiedAt = DateTimeOffset.UtcNow,
					Email = "customer2@example.com",
					Orders = new Collection<Order>()
				},
				new Customer
				{
					Id = 3,
					CreatedAt = DateTimeOffset.UtcNow,
					ModifiedAt = DateTimeOffset.UtcNow,
					Email = "customer2@example.com",
					Orders = new Collection<Order>()
				},
				new Customer
				{
					Id = 4,
					CreatedAt = DateTimeOffset.UtcNow,
					ModifiedAt = DateTimeOffset.UtcNow,
					Email = "customer2@example.com",
					Orders = new Collection<Order>()
				}
			};
			var pagedCustomers = await PagedList<Customer>.Create(customers.BuildMock(), pageNumber, pageSize);
			repositoryMock.Read(pageNumber, pageSize, query).Returns(pagedCustomers);

			// Act
			var result = await customerService.Read(pageNumber, pageSize, query);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(4, customers.Count);
			Assert.Equal(pageSize, result.Count);
		}

		[Fact]
		public async Task Read_CustomerEmail_ReturnsCustomer()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();
			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			var customerEmail = new CustomerEmail("test@example.com");
			var expectedCustomer = new Customer { Email = customerEmail.Value };

			repositoryMock.Read(customerEmail).Returns(expectedCustomer);

			// Act
			var result = await customerService.Read(customerEmail);

			// Assert
			Assert.Equal(customerEmail.Value, result.Email);
		}

		[Fact]
		public async Task Update_ValidId_UpdateSuccessful()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var cHashidsMock = Substitute.For<IHashids>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();

			var customerHashId = "encoded-customer-id";
			var updateDto = new UpdateCustomerDto { };
			var existingCustomer = new Customer { };

			cHashidsMock.TryDecodeSingle(customerHashId, out _).Returns(true);
			hashidsServiceMock.Get(typeof(Customer)).Returns(cHashidsMock);
			repositoryMock.Read(Arg.Any<int>()).Returns(existingCustomer);

			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			// Act
			await customerService.Update(new CustomerHashId(customerHashId), updateDto);

			// Assert
			await repositoryMock.Received().Update(Arg.Any<CustomerRequest>());
		}

		[Fact]
		public async Task Delete_ValidId_DeletionSuccessful()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var cHashidsMock = Substitute.For<IHashids>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();

			var customerHashId = "encoded-customer-id";
			var existingCustomer = new Customer { };

			cHashidsMock.TryDecodeSingle(customerHashId, out _).Returns(true);
			hashidsServiceMock.Get(typeof(Customer)).Returns(cHashidsMock);
			repositoryMock.Read(Arg.Any<int>()).Returns(existingCustomer);

			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			// Act
			await customerService.Delete(new CustomerHashId(customerHashId));

			// Assert
			await repositoryMock.Received().Delete(Arg.Any<int>());
		}

		[Fact]
		public async Task IsResourceOwner_ValidCustomerAndOrder_ReturnsTrue()
		{
			// Arrange
			var repositoryMock = Substitute.For<ICustomerRepository>();
			var cHashidsMock = Substitute.For<IHashids>();
			var oHashidsMock = Substitute.For<IHashids>();
			var hashidsServiceMock = Substitute.For<IHashidsService>();

			var customerHashId = "encoded-customer-id";
			var orderHashId = "encoded-order-id";

			cHashidsMock.TryDecodeSingle(customerHashId, out _).Returns(true);
			oHashidsMock.TryDecodeSingle(orderHashId, out _).Returns(true);
			hashidsServiceMock.Get(typeof(Customer)).Returns(cHashidsMock);
			hashidsServiceMock.Get(typeof(Order)).Returns(oHashidsMock);
			repositoryMock.IsResourceOwner(Arg.Any<CustomerId>(), Arg.Any<OrderId>()).Returns(true);

			var customerService = new CustomerService(repositoryMock, hashidsServiceMock);

			// Act
			var result = await customerService.IsResourceOwner(
				new CustomerHashId(customerHashId),
				new OrderHashId(orderHashId)
			);

			// Assert
			Assert.True(result);
		}
	}
}
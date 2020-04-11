CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@PurchaseDate datetime2
as
--[Id], [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
begin
	set nocount on;

	insert into dbo.Inventory([ProductId], [Quantity], [PurchasePrice], [PurchaseDate])
	VALUES(@ProductId, @Quantity, @PurchasePrice, @PurchaseDate);

end
